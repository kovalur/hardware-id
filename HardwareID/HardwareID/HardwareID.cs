using System;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace HardwareID
{
    public static class HardwareID
    {
        public static string HARDWARE_ID => ReturnHardwareID().Result;
        private static async Task<string> ReturnHardwareID()
        {
            byte[] bytes;
            byte[] hashedBytes;

            StringBuilder sb = new StringBuilder();

            Task task = Task.Run(() =>
            {
                ManagementObjectSearcher cpu = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                ManagementObjectCollection cpu_Collection = cpu.Get();

                foreach (ManagementObject obj in cpu_Collection)
                {
                    sb.Append(obj["ProcessorId"].ToString().Substring(0, 4));
                    break;
                }

                ManagementObjectSearcher hdd = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                ManagementObjectCollection hdd_Collection = hdd.Get();

                foreach (ManagementObject obj in hdd_Collection)
                {
                    sb.Append(obj["Signature"].ToString().Substring(0, 4));
                    break;
                }

                ManagementObjectSearcher bios = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                ManagementObjectCollection bios_Collection = bios.Get();

                foreach (ManagementObject obj in bios_Collection)
                {
                    sb.Append(obj["Version"].ToString().Substring(0, 4));
                    break;
                }
            });
            Task.WaitAll(task);
            bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            hashedBytes = System.Security.Cryptography.SHA256.Create().ComputeHash(bytes);

            return await Task.FromResult(Convert.ToBase64String(hashedBytes).Substring(25));
        }
    }
}
