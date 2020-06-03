using System.Windows.Forms;

namespace DroneDeliverySystem.DisplayUtils
{
    public class DisplayConsole
    {
        private delegate void SafeConsoleCallDelegate(string text);
        public RichTextBox Console { get; set; }

        public DisplayConsole(RichTextBox console)
        {
            this.Console = console;
        }

        public void Add(string text)
        {
            WriteToConsoleSafe(text);
        }

        private void WriteToConsoleSafe(string text)
        {
            if (Console.InvokeRequired)
            {
                var d = new SafeConsoleCallDelegate(WriteToConsoleSafe);
                Console.Invoke(d, new object[] { text });
            }
            else
            {
                Console.Text += $"{text}\n";
            }
        }
    }
}
