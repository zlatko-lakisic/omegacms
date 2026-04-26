using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class HardwareStatus
    {
        #region Helper Classes
        public class Drive
        {
            #region Attributes
            private string _label;
            private int _totalSizeMb;
            private int _availableSizeMb;
            private int _usedSizeMb;
            private string _format;
            #endregion

            #region Properties

            public string Label
            {
                get { return _label; }
                set { _label = value; }
            }

            public int TotalSizeMb
            {
                get { return _totalSizeMb; }
                set { _totalSizeMb = value; }
            }

            public int AvailableSizeMb
            {
                get { return _availableSizeMb; }
                set { _availableSizeMb = value; }
            }

            public int UsedSizeMb
            {
                get { return _usedSizeMb; }
                set { _usedSizeMb = value; }
            }

            public float TotalSizeGb
            {
                get 
                {
                    return MbToGb(_totalSizeMb); 
                }
            }

            public float AvailableSizeGb
            {
                get { return MbToGb(_availableSizeMb); }
            }

            public float UsedSizeGb
            {
                get { return MbToGb(_usedSizeMb); }
            }

            public string Format
            {
                get { return _format; }
                set { _format = value; }
            }
            #endregion
        }

        public class NetworkInterface
        {
            #region Attributes
            private string _name;
            private string _description;
            private int _sentMb;
            private int _receivedMb;
            #endregion

            #region Properties

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }

            public int SentMb
            {
                get { return _sentMb; }
                set { _sentMb = value; }
            }

            public int ReceivedMb
            {
                get { return _receivedMb; }
                set { _receivedMb = value; }
            }

            public float SentGb
            {
                get { return MbToGb(_sentMb); }
            }

            public float ReceivedGb
            {
                get { return MbToGb(_receivedMb); }
            }

            public double NetworkUtilization
            {
                get
                {
                    double utilization = 0;
                    try
                    {
                        PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
                        String[] instancename = category.GetInstanceNames();

                        const int numberOfIterations = 10;

                        PerformanceCounter bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", this.Description);
                        float bandwidth = bandwidthCounter.NextValue();//valor fixo 10Mb/100Mn/

                        PerformanceCounter dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", this.Description);

                        PerformanceCounter dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", this.Description);

                        float sendSum = 0;
                        float receiveSum = 0;

                        for (int index = 0; index < numberOfIterations; index++)
                        {
                            sendSum += dataSentCounter.NextValue();
                            receiveSum += dataReceivedCounter.NextValue();
                        }
                        float dataSent = sendSum;
                        float dataReceived = receiveSum;


                        utilization = (8 * (dataSent + dataReceived)) / (bandwidth * numberOfIterations) * 100;
                    }
                    catch (Exception e)
                    {

                    }
                    return utilization;
                }
            }
            #endregion
        }

        public class Process
        {
            #region Attributes
            private int _id;
            private string _name;
            #endregion

            #region Properties

            public int Id
            {
                get { return _id; }
                set { _id = value; }
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public string User
            {
                get
                {
                    try
                    {
                    string query = string.Format("Select * From Win32_Process Where ProcessID = {0}", this._id);
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                    ManagementObjectCollection processList = searcher.Get();

                        foreach (ManagementObject obj in processList)
                        {
                            string[] argList = new string[] { string.Empty, string.Empty };
                            int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                            if (returnVal == 0)
                            {
                                // return DOMAIN\user
                                return argList[1] + "\\" + argList[0];
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }

                    return "NO OWNER";
                }
            }

            public double ProcessorUsage
            {
                get
                {
                    double cpuUsageTotal = 0;
                    System.Diagnostics.Process thisProcess = System.Diagnostics.Process.GetProcessesByName(_name).FirstOrDefault();
                    if (thisProcess != null)
                    {
                        var startTime = DateTime.UtcNow;
                        var startCpuUsage = thisProcess.TotalProcessorTime;
                        var cpuUsedMs = (startCpuUsage).TotalMilliseconds;
                        cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount);
                    }
                    return cpuUsageTotal * 100;
                }
            }

            public long MemoryUsageMb
            {
                get
                {
                    return System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                }
            }
            #endregion
        }
        #endregion

        #region Properties
        /// <summary>
        /// Performance Sample Date and Time
        /// </summary>
        public string SampleDateTime
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");
            }
        }
        /// <summary>
        /// CPU Usage out of 100
        /// </summary>
        public double CpuUsage
        {
            get
            {
                try
                {
                    DateTime startTime = DateTime.UtcNow;
                    TimeSpan startCpuUsage = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;

                    DateTime endTime = DateTime.UtcNow;
                    TimeSpan endCpuUsage = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
                    double cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                    double totalMsPassed = (endTime - startTime).TotalMilliseconds;
                    double cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

                    return cpuUsageTotal *100;


                    /*PerformanceCounter cpuCounter = new PerformanceCounter();
                    cpuCounter.CategoryName = "Processor";
                    cpuCounter.CounterName = "% Processor Time";
                    cpuCounter.InstanceName = "_Total";

                    // will always start at 0
                    dynamic firstValue = cpuCounter.NextValue();
                    System.Threading.Thread.Sleep(1000);
                    // now matches task manager reading
                    dynamic secondValue = cpuCounter.NextValue();

                    return Math.Round(Convert.ToDecimal((float)secondValue));*/
                }
                catch (Exception ex)
                {

                }
                return 0;
            }
        }
        /// <summary>
        /// Free Memory
        /// </summary>
        public long FreeMemoryMb
        {
            get
            {
                try
                {
                    return System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;

                    /*PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                    return (int)ramCounter.NextValue();*/
                }
                catch (Exception ex)
                {

                }
                return 0;
            }
        }
        /// <summary>
        /// Total Memory
        /// </summary>
        public long TotalMemoryMb
        {
            get
            {

                return System.Diagnostics.Process.GetCurrentProcess().NonpagedSystemMemorySize64;
                /*ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject item in moc)
                {
                    return Convert.ToUInt64(item.Properties["TotalPhysicalMemory"].Value);
                }
                return default(ulong);*/
            }
        }
        /// <summary>
        /// Drives
        /// </summary>
        public IEnumerable<Drive> Drives
        {
            get
            {
                return DriveInfo.GetDrives().Where(d => d.IsReady).Select(d => new Drive()
                {
                    Label = d.Name,
                    Format = d.DriveFormat,
                    TotalSizeMb = ByteToMb<long>(d.TotalSize),
                    AvailableSizeMb = ByteToMb<long>(d.TotalFreeSpace),
                    UsedSizeMb = ByteToMb<long>(d.TotalSize - d.TotalFreeSpace)
                }).Where(d => d.TotalSizeGb > 0 && d.UsedSizeGb > 0);
            }
        }
        /// <summary>
        /// Network Interfaces
        /// </summary>
        public IEnumerable<NetworkInterface> NetworkInterfaces
        {
            get
            {
                return System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Select(n => new NetworkInterface()
                {
                    Name = n.Name,
                    Description = n.Description,
                    ReceivedMb = ByteToMb<long>(n.GetIPv4Statistics().BytesReceived),
                    SentMb = ByteToMb<long>(n.GetIPv4Statistics().BytesSent)
                });
            }
        }
        /// <summary>
        /// Processes
        /// </summary>
        public IEnumerable<Process> Processes
        {
            get
            {
                return System.Diagnostics.Process.GetProcesses().Select(p => new Process(){
                    Name = p.ProcessName,
                    Id = p.Id
                });
            }
        }
        #endregion

        #region Helper Methods
        internal static int ByteToMb<T>(T input)
        {
            return Convert.ToInt32((Convert.ToInt64(input) / 1024) / 1024);
        }

        internal static float MbToGb(int input)
        {
            return input / 1024;
        }
        #endregion
    }
}
