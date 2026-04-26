using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.IO;

namespace MD.Tools.Licensing
{
    public static class ClientId
    {
        private static bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                         RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            return isUnix;
        }

        private static string Bash(this string cmd)
        {
            string result = String.Empty;

            try
            {
                var escapedArgs = cmd.Replace("\"", "\\\"");

                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };

                    process.Start();
                    result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit(1500);
                    process.Kill();
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }

        private static string WmiOutput(this string cmd)
        {
            string output = "";

            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "wmic";
                info.Arguments = cmd;
                info.RedirectStandardOutput = true;

                using (Process process = System.Diagnostics.Process.Start(info))
                {
                    output = process.StandardOutput.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex);
            }

            return output;
        }

        private static string GetValueFromOutput(this string output, string valueName)
        {
            return output.Split("\n")
                       .Select(str => str.Split("=", StringSplitOptions.RemoveEmptyEntries))
                       .Where(str => str.Length == 2 && string.CompareOrdinal(str.First(), valueName).Equals(0))
                       .Select(str => str.Last())
                       .FirstOrDefault();
        }

        private static string CpuId
        {
            get
            {
                if (IsUnix())
                {
                    // return "dmidecode -t processor | grep -E ID | sed 's/.*: //' | head -n 1".Bash();
                    return Helpers.Core.Crypto.MD5Crypt.MD5Encrypt("cat /proc/cpuinfo | awk '{print}' ORS='\" '".Bash());
                }
                return "cpu list full".WmiOutput().GetValueFromOutput("ProcessorId");
            }
        }

        private static string MachineId
        {
            get
            {
                if (IsUnix())
                {
                    return "cat /var/lib/dbus/machine-id".Bash();
                }
                return "baseboard list full".WmiOutput().GetValueFromOutput("SerialNumber");
            }
        }

        public static string GetClientId(string majorVersion, string clientKey)
        {
            string majorVersionTrimmed = majorVersion.Split('.').First();
            return Helpers.Core.Crypto.AESCrypt.Encrypt(string.Format("{0}-{1}-{2}-{3}", majorVersionTrimmed, clientKey, CpuId, MachineId), clientKey);
        }

        public static void SaveToFile(string majorVersion, string clientKey, string filePath, string idFileName = null)
        {
            if (string.IsNullOrEmpty(majorVersion))
            {
                throw new ArgumentException("message", nameof(majorVersion));
            }

            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentException("message", nameof(clientKey));
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("message", nameof(filePath));
            }

            try
            {
                if (string.IsNullOrEmpty(idFileName))
                {
                    idFileName = Properties.Resources.clientIdFileName;
                }

                if(!Directory.Exists(Path.Join(filePath, Properties.Resources.licenseDirectory)))
                {
                    Directory.CreateDirectory(Path.Join(filePath, Properties.Resources.licenseDirectory));
                }

                if (!File.Exists(Path.Join(filePath, Properties.Resources.licenseDirectory, idFileName)))
                {
                    string majorVersionTrimmed = majorVersion.Split('.').First();
                    File.WriteAllText(Path.Join(filePath, Properties.Resources.licenseDirectory, idFileName), GetClientId(majorVersionTrimmed, clientKey));
                }
            }
            catch (Exception ex)
            {
                throw new LicensingException(LicensingException.LicensingExceptionErrorType.ClientIdFileSave, ex);
            }
        }
    }
}
