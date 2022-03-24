using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Title = "Tool";
            //var devices = ReadDevices();
            //foreach (var device in devices)
            //{
            //    var nums = IsDeviceNormal(device.Key);
                
            //    Console.ForegroundColor = ConsoleColor.DarkRed;
            //    Console.WriteLine($"{device.Key}======================{nums == device.Value}");
            //}
            Console.ReadLine();
        }

        private static Dictionary<string,int> ReadDevices()
        {
            Dictionary<string, int> deviceDictionary = new Dictionary<string, int>();
            try
            {
#if DEBUG   
                var configPath = @"G:\TestProjects\ConsoleApp5\Device.config";
#else
               var configPath = Path.Combine(Directory.GetCurrentDirectory(), "Device.config");
#endif

                using (StreamReader sr = new StreamReader(configPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line)) continue;
                        line = line.Trim();
                        if (!deviceDictionary.ContainsKey(line))
                        {
                            deviceDictionary.Add(line,1);
                        }
                        else
                        {
                            deviceDictionary[line] += 1;
                        }
                    }

                    return deviceDictionary;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"配置文件读取失败:{e.Message}", "Error");
                return deviceDictionary;
            }
        }

        private static int IsDeviceNormal(string deviceId)
        {
            using (var searcher = new ManagementObjectSearcher($@"Select * From Win32_PnPEntity Where DeviceID Like ""PCI%"""))
            {
                var collection = searcher.Get();
                //Console.WriteLine(collection.Count);
                int i = 0;
                foreach (var device in collection)
                {
                    if (device.GetPropertyValue("DeviceID").ToString().StartsWith(deviceId))
                    {
                        if ("OK".Equals(device.GetPropertyValue("Status").ToString()))
                        {
                            i += 1;
                        }
                    }
                }
                collection.Dispose();
                return i;
            }
        }
    }
}
