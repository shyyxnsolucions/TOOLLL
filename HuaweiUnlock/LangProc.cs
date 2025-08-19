using HuaweiUnlocker.UI;
using System;
using System.Windows.Forms;

namespace HuaweiUnlocker
{
    public static class LangProc
    {
        public static bool debug = true;
        public static TabControl Tab;
        public static TextBox LOGGBOX;
        public static NProgressBar PRG;

        public static bool LOG(int type, string message = "", object extra = null)
        {
            string prefix = type switch
            {
                1 => "[WARN] ",
                2 => "[ERROR] ",
                _ => "[INFO] "
            };
            string text = prefix + message + (extra != null ? extra.ToString() : string.Empty);
            if (LOGGBOX != null)
            {
                Action act = () =>
                {
                    LOGGBOX.AppendText(text + Environment.NewLine);
                    LOGGBOX.SelectionStart = LOGGBOX.Text.Length;
                    LOGGBOX.ScrollToCaret();
                };
                if (LOGGBOX.InvokeRequired) LOGGBOX.Invoke(act); else act();
            }
            return type == 0;
        }

        public static void Progress(int value)
        {
            if (PRG == null) return;
            Action act = () =>
            {
                value = Math.Max(PRG.ValueMinimum, Math.Min(value, PRG.ValueMaximum));
                PRG.Value = value;
            };
            if (PRG.InvokeRequired) PRG.Invoke(act); else act();
        }

        public static void Progress(int value, int max)
        {
            if (PRG == null) return;
            Action act = () =>
            {
                PRG.ValueMaximum = max;
                value = Math.Max(PRG.ValueMinimum, Math.Min(value, PRG.ValueMaximum));
                PRG.Value = value;
            };
            if (PRG.InvokeRequired) PRG.Invoke(act); else act();
        }
    }
}
