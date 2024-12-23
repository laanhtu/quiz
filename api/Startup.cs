using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CP.Api.Helpers;
using CP.Api.Services;
using Microsoft.Extensions.Options;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Logging;
using Chroniton;
using Microsoft.Extensions.Caching.Memory;
using Chroniton.Jobs;
using Chroniton.Schedules;
using System;
using Elsa.entities.cache;
using System.Threading.Tasks;
using System.Collections.Generic;
using CP.Api.Models;
using System.IO;
using Elsa.Api.Helpers;
using Elsa.Repository.Services;
using Elsa.Repository.Interfaces;
using System.Linq;

namespace Elsa.Api
{
    public class Startup
    {
        Singularity singularity = Singularity.Instance;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));

            /*
            * The IBookstoreDatabaseSettings interface is registered in DI with a singleton service lifetime. 
            * When injected, the interface instance resolves to a BookstoreDatabaseSettings object.
            */
            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            // Add converter to DI
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.UTF8.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });

            // configure DI for application services
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IUserService, UserService>();

            services.AddScoped<IQuizzesService, QuizzesService>();
            services.AddTransient<IQuizzesService, QuizzesService>();

            // Quiz service instances
            services.AddTransient<IQuizSessionsServiceTransient, QuizSessionsService>();
            services.AddScoped<IQuizSessionsServiceScoped, QuizSessionsService>();
            services.AddSingleton<IQuizSessionsServiceSingleton, QuizSessionsService>();

            // Quiz leader instance
            services.AddTransient<IQuizLeadersServiceTransient, QuizLeadersService>();
            services.AddScoped<IQuizLeadersServiceScoped, QuizLeadersService>();
            services.AddSingleton<IQuizLeadersServiceSingleton, QuizLeadersService>();

            services.AddScoped<IAIQuizService, AIQuizService>();

            services.AddSingleton<MemoryCache>();
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseHttpsRedirection();

            //loggerFactory.AddLog4Net();

            // app.UseMvc();

            app.UseWebSockets();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            //setupSchedulers(app);

            loadCache(app);
        }

        private void loadCache(IApplicationBuilder app)
        {
            IUserService service = app.ApplicationServices.GetService(typeof(IUserService)) as IUserService;

            var users = service.GetAll();

            MemoryCache cached = app.ApplicationServices.GetService(typeof(MemoryCache)) as MemoryCache;
            Dictionary<string, User> userDict = users.ToDictionary(x => x.Id, x => x);
            cached.Set(CacheKeys.Users, userDict);
        }

        private void setupSchedulers(IApplicationBuilder app)
        {
            // Job 1
            var gettingJob = new SimpleParameterizedJob<string>((parameter, scheduledTime) =>
            {
                //Console.WriteLine($"You says every 5s: {parameter}\tscheduled: {scheduledTime.ToString("o")}");
            });
            var greetingSchedule = new EveryXTimeSchedule(TimeSpan.FromSeconds(5));
            var gettingScheduledJob = singularity.ScheduleParameterizedJob(greetingSchedule, gettingJob, "Hello World", true); //starts immediately

            singularity.Start();
            //Console.WriteLine("Scheduler is started: {0}", DateTime.Now);
        }
    }
}
