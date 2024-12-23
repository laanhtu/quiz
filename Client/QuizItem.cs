using Elsa.Entities.Quiz;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class QuizItemControl : UserControl
    {
        public QuizItem QuizItem { get; private set; }
        public QuizItemControl(QuizItem quizItem)
        {
            InitializeComponent();

            QuizItem = quizItem;
        }

        private void QuizItemControl_Load(object sender, EventArgs e)
        {
            lblQuestion.Text = "Question: " + QuizItem.Question;
            int index = 1;
            foreach (var item in QuizItem.Choices)
            {
                Console.WriteLine(item);
                RadioButton radioButton = new RadioButton();
                radioButton.AutoSize = true;
                radioButton.Tag = index;
                radioButton.Text = item;

                if(index == QuizItem.CorrectChoice)
                {
                    radioButton.ForeColor = Color.DarkGreen;
                }
                
                radioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                radioButton.CheckedChanged += RadioButton_CheckedChanged;
                //var width = MeasureString(item, radioButton.Font);
                //radioButton.Width = (int) width.Width;
                flowLayoutPanel.Controls.Add(radioButton);
                index++;
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selectChoice = sender as RadioButton;

            QuizItem.AnswerChoice = (int)selectChoice.Tag;
        }

        public static SizeF MeasureString(string s, Font font)
        {
            SizeF result;
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                result = g.MeasureString(s, font, int.MaxValue, StringFormat.GenericTypographic);
            }

            return result;
        }
    }
}
