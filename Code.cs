using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TSMStringConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Exit_Button_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void Copy_Button_Click(object sender, EventArgs e)
        {
            if (Result_Textbox.TextLength > 0)
            {
                Clipboard.SetText(Result_Textbox.Text);
                MessageBox.Show("Copied!");
            }
            else
            {
                MessageBox.Show("Nothing to save!");
            }
        }

        private async void Start_Button_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(Convert_Textbox.Text, @"^[0-9,]+$"))
            {
                MessageBox.Show("Wrong characters in the input string!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Convert_Textbox.Enabled = false;
            Result_Textbox.Text = "";
            await Task.Run(() => 
            {
                Invoke(new Action(() => { Wait_Label.Visible = true; }));
                List<string> ConvertedStrings = new List<string>();
                int CurrentCommaIndex = 0;
                int NextCommaIndex = 0;
                string String = string.Empty;
                while (NextCommaIndex != -1)
                {
                    Invoke(new Action(() => { NextCommaIndex = Convert_Textbox.Text.IndexOf(",", CurrentCommaIndex + 1); }));
                    if (NextCommaIndex != -1)
                    {
                        Invoke(new Action(() => { String = "i:" + Convert_Textbox.Text.Substring(CurrentCommaIndex, NextCommaIndex - CurrentCommaIndex); }));
                    }
                    else
                    {
                        Invoke(new Action(() => { String = "i:" + Convert_Textbox.Text.Substring(CurrentCommaIndex, Convert_Textbox.TextLength - CurrentCommaIndex); }));
                    }
                    ConvertedStrings.Add(String);
                    CurrentCommaIndex = NextCommaIndex + 1;
                }
                Invoke(new Action(() => { Wait_Label.Visible = false;}));
                int Count = 0;
                while (Count != ConvertedStrings.Count - 1)
                {
                    Invoke(new Action(() => { Result_Textbox.AppendText(ConvertedStrings[Count] + ","); }));
                    Count++;
                }
                Invoke(new Action(() => { Result_Textbox.AppendText(ConvertedStrings[Count]);}));
            });
            Result_Textbox.Enabled = true;
            Convert_Textbox.Enabled = true;
        }

        private void Convert_Textbox_TextChanged(object sender, EventArgs e)
        {
            Convert_Button.Enabled = Convert_Textbox.TextLength > 0;
            Result_Textbox.Enabled = Convert_Textbox.TextLength > 0;
        }
    }
}
