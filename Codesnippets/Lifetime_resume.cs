using Microsoft.Win32.SafeHandles;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Principal;
using System.Reflection;
using System.Security.Permissions;

namespace Lifetime_Resume
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hProcess);

        public static byte[] ReadMemory(Process process, int address, int numOfBytes, out int bytesRead)
        {
            IntPtr hProc = OpenProcess(ProcessAccessFlags.All, false, process.Id);

            byte[] buffer = new byte[numOfBytes];

            ReadProcessMemory(hProc, new IntPtr(address), buffer, numOfBytes, out bytesRead);
            return buffer;
        }

        public static bool WriteMemory(Process process, int address, float value)
        {
            int bytesWritten;

            IntPtr hProc = OpenProcess(ProcessAccessFlags.All, false, process.Id);

            byte[] val = BitConverter.GetBytes(value);

            bool worked = WriteProcessMemory(hProc, new IntPtr(address), val, (uint)val.LongLength, out bytesWritten);

            CloseHandle(hProc);

            return worked;
        }

        public int bytesRead;
        public int EngineLife;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] GTR2 = Process.GetProcessesByName("GTR2");
            int lifeAdr = 0x00B68370;
            if (GTR2.Length > 0)
            {
                byte[] valueInPit = ReadMemory(GTR2[0], lifeAdr, 128, out bytesRead);
                EngineLife = (int)BitConverter.ToDouble(valueInPit, 0);
                int seconds = EngineLife;
                int minutes = seconds / 60; seconds = seconds - (minutes * 60); int hours = minutes / 60; minutes = minutes - (hours * 60);
                label1.Text = hours.ToString() + " hours, " + minutes.ToString() + " minutes and " + seconds.ToString() + " seconds";
            }
        }
    }
}
