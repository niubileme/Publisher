using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core.Common
{
    public class CmdHelper
    {
        /// <summary>
        /// 执行Cmd命令
        /// </summary>
        public static string ExecuteCmd(string cmdstr)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();

                process.StandardInput.WriteLine(cmdstr);
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine("exit");

                StreamReader reader = process.StandardOutput;

                string output = reader.ReadToEnd();
                return output;
            }

        }

        /// <summary>
        /// 打开软件并执行命令
        /// </summary>
        /// <param name="programName">软件路径加名称（.exe文件）</param>
        /// <param name="cmd">要执行的命令</param>
        public static void RunProgram(string programName, string param)
        {
            using (Process proc = new Process())
            {
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.FileName = programName;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
                if (param.Length != 0)
                {
                    proc.StandardInput.WriteLine(param);
                }
            }
        }
    }
}
