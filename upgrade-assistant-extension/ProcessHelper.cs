using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upgrade_assistant_extension
{
    public class ProcessHelper
    {
        public static bool Execute(string cmd, Action<string?, bool> output)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "upgrade-assistant.exe";
                p.StartInfo.Arguments = cmd;
                p.StartInfo.UseShellExecute = false;
                //p.StartInfo.RedirectStandardOutput = true;
                //p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.OutputDataReceived += (s, e) => output?.Invoke(e.Data, false);
                p.ErrorDataReceived += (s, e) => output?.Invoke(e.Data, true);
                p.Start();
                p.WaitForExit();
                p.Close();
                return true;
            }
            catch (Exception e)
            {
                output?.Invoke(e.Message, true);
                return false;
            }
        }
    }
}
