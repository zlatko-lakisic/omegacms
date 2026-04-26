using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using MD.Tools.Helpers.Core.Logging;

namespace MD.Tools.Helpers.Core.ProcessH
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProcessHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public static void ExecuteExeFileSimple(string command)
        {
            Process.Start(command);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public static void ExecuteExeFile(string command)
        {
            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(command);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = psi;

            process.Start();
            process.WaitForExit();
            process.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="workingDirectory"></param>
        public static void ExecuteExeFile(string command, string workingDirectory)
        {
            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(command);
            psi.WorkingDirectory = workingDirectory;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = psi;

            process.Start();
            process.WaitForExit();
            process.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public static void ExecuteCommand(string command)
        {
            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe", @"/C" + command);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = psi;

            process.Start();
            process.WaitForExit();
            process.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="workingDirectory"></param>
        public static void ExecuteCommand(string command, string workingDirectory)
        {
            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe", @"/C" + command);
            psi.WorkingDirectory = workingDirectory;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = psi;

            process.Start();
            process.WaitForExit();
            process.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="args"></param>
        public static void ExecuteBatchFile(string file, string args)
        {
            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(file, args);
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Maximized;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = psi;

            process.Start();
            process.WaitForExit();
            process.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="args"></param>
        /// <param name="workingDirectory"></param>
        public static void ExecuteBatchFile(string file, string args, string workingDirectory)
        {
            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(file, args);
            psi.UseShellExecute = false;
            psi.WorkingDirectory = workingDirectory;
            psi.WindowStyle = ProcessWindowStyle.Maximized;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = psi;

            process.Start();
            process.WaitForExit();
            process.Close();
        }
    }
}
