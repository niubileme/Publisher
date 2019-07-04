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
        public static string ExecuteCmd(string cmd)
        {
            // &:同时执行两个命令
            // |:将上一个命令的输出,作为下一个命令的输入
            // &&：当&&前的命令成功时,才执行&&后的命令
            // ||：当||前的命令失败时,才执行||后的命令]]>
            //不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            cmd = cmd.Trim().TrimEnd('&') + "&exit";

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
                process.StartInfo.CreateNoWindow = true;//不显示程序窗口
                process.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                process.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                process.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                process.Start();

                process.StandardInput.WriteLine(cmd);
                process.StandardInput.AutoFlush = true;
               
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
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
