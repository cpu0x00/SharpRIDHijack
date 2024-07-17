using System;
using System.DirectoryServices.AccountManagement;

namespace SharpRIDHijack
{
    public class userapi
    {
        public static void Enable(string username, bool setpassword=false, string password=null)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
                {
                    // Find the user by username
                    UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);

                    if (user != null)
                    {
                        // Check if the user account is disabled
                        if (user.Enabled == false)
                        {
                            // Enable the user account
                            user.Enabled = true;
                            if (setpassword == true) { user.SetPassword(password); }
                            user.Save();

                            Console.WriteLine("[+] Enabled user account : '{0}' !", username);
                            if (setpassword == true) { Console.WriteLine("[+] Changed the user password"); }
                        }
                        else
                        {
                            Console.WriteLine("[i] User account '{0}' is already enabled.", username);
                        }
                    }
                    else
                    {
                        Console.WriteLine("User account '{0}' not found.", username);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return;
            }
        }



        public static bool UserExists(PrincipalContext context, string username)
        {
            UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);
            return (user != null);
        }

        public static void Create(string username, string password)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
                {
                    // Check if the user already exists
                    if (!UserExists(context, username))
                    {
                        // Create the new user
                        UserPrincipal newUser = new UserPrincipal(context);
                        newUser.SamAccountName = username;
                        newUser.SetPassword(password);
                        newUser.Enabled = true;
                        newUser.Save();

                        Console.WriteLine($"[+] Created User {username}.");
                    }
                    else
                    {
                        Console.WriteLine("User already exists.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating user: " + ex.Message);
            }
        }



    }
}
