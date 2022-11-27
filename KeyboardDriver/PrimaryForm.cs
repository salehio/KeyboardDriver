using System;
using System.Drawing;
using System.Windows.Forms;
using static KeyboardDriver.Logger;

namespace KeyboardDriver
{
    public class PrimaryForm : Form
    {
        public RichTextBox LogOutput { get; set; }

        private int _numLines = 0;

        private const int MaxLines = 1000;

        public PrimaryForm()
        {
            this.Closing += PrimaryForm_Closing;

            LogOutput = new RichTextBox
            {
                Text = $"Keyboard driver loaded...{Environment.NewLine}",
                Visible = true,
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                BackColor = Color.Black,
                ForeColor = Color.White,
                TabStop = false
            };

            this.Controls.Add(LogOutput);
        }
        private void PrimaryForm_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public void AppendLog(string value, LogSeverity severity)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string, LogSeverity>(AppendLog), new object[] { value, severity });
                return;
            }

            var newText = LogOutput.Text + value + Environment.NewLine;

            LogOutput.AppendText(value, severity.ToColor());

            if (_numLines > MaxLines)
            {
                LogOutput.Select(0, LogOutput.GetFirstCharIndexFromLine(LogOutput.Lines.Length - MaxLines));
                LogOutput.SelectedText = "";
            }
            else
            {
                _numLines++;
            }

            ScrollToBottom();
        }

        public void ScrollToBottom()
        {
            LogOutput.SelectionStart = LogOutput.TextLength;
            LogOutput.ScrollToCaret();
        }
    }
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text + Environment.NewLine);
            box.SelectionColor = box.ForeColor;
        }
    }

    public static class LogSeverityExtensions
    {
        // TODO: Consolidate with console printing color
        public static Color ToColor(this LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Debug:
                    return Color.Gray;
                case LogSeverity.Success:
                    return Color.Green;
                case LogSeverity.Warning:
                    return Color.Yellow;
                case LogSeverity.Error:
                    return Color.Red;
                case LogSeverity.Information:
                    return Color.White;
                default:
                    return Color.Purple;
            }
        }
    }



}
