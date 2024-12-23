using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Elsa.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine(Figgle.FiggleFonts.Standard.Render("API"));

      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            //.UseKestrel()
            .UseStartup<Startup>()
            .UseUrls("http://127.0.0.1:8686/");
  }
}
