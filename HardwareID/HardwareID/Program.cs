using System;

namespace HardwareID
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string hwid = HardwareID.HARDWARE_ID;
            Console.WriteLine(hwid);
            Console.ReadKey();
        }
    }
}
