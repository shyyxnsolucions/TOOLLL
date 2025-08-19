using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using HuaweiUnlocker.UI;

namespace HuaweiUnlocker
{
    public static class LangProc
    {
        public static TextBox LOGGBOX;
        public static ProgressBar PRG;
        public static TabControl Tab;
        public static StreamWriter se;
        public static bool debug = false;
        public static CancellationToken token;
        public static CancellationTokenSource ct;
        public static Task CurTask;
        public const string newline = "\n";

        public static bool LOG(int level, string message, string extra = "")
        {
            string text = Language.isExist(message.ToLower()) ? Language.Get(message) : message;
            if (!string.IsNullOrEmpty(extra))
                text += extra;

            try
            {
                se?.WriteLine(text);
                se?.Flush();
            }
            catch { }

            if (LOGGBOX != null)
            {
                Action act = () =>
                {
                    LOGGBOX.AppendText(text + Environment.NewLine);
                    LOGGBOX.SelectionStart = LOGGBOX.Text.Length;
                    LOGGBOX.ScrollToCaret();
                };
                if (LOGGBOX.InvokeRequired)
                    LOGGBOX.Invoke(act);
                else
                    act();
            }

            return true;
        }

        public static void Progress(int value, int max = 100)
        {
            if (PRG == null)
                return;
            Action act = () =>
            {
                PRG.Maximum = max;
                PRG.Value = Math.Min(value, max);
            };
            if (PRG.InvokeRequired)
                PRG.Invoke(act);
            else
                act();
        }
    }
}
