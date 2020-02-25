using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace icar_registry
{
    class Program
    {
        static void Main(string[] args)
        {


            bool is64bit = Environment.Is64BitOperatingSystem;

            string message = "Arhitektura je " + (is64bit ? " 64 bit" : " 32 bit");

            Console.WriteLine(message);

                local_machine(is64bit);

            Console.ReadKey();

        }

        public static void local_machine(bool is64bit) {

            string autonet = @"SOFTWARE\WOW6432Node\Ceniber\AutoNet";
            string perfiles = @"SOFTWARE\WOW6432Node\Ceniber\Perfiles";
            if (!is64bit) {
                 autonet = @"SOFTWARE\Ceniber\AutoNet";
                 perfiles = @"SOFTWARE\Ceniber\Perfiles";
            }

            RegistryKey key_to_set_default_profile = Registry.LocalMachine.OpenSubKey(autonet, true);
            RegistryKey key_profile = Registry.LocalMachine.OpenSubKey(perfiles, true);

            if (key_to_set_default_profile == null || key_profile == null)
            {
                Console.WriteLine(@"Ne postoje registry..");
                return;
            }
            else
            {

                try
                {
                    Console.WriteLine(@"Postoje registry..");


                    RegistryKey key = key_profile.OpenSubKey(@"PROFILE_001", true);
                    if (key == null) key = key_profile.CreateSubKey(@"PROFILE_001");


                    using (key)
                    {

                        key.SetValue("Descrip", "ICAR");
                        key.SetValue("Database", "ICARDMS");
                        key.SetValue("DBMS", "MSS");
                        key.SetValue("ServerName", @"192.168.0.7\icardms");
                        key.SetValue("DBParm", "Async = 1, PacketSize = 16384, AppName = 'ICar', OptSelectBlob = 1, DateTimeAllowed = 'Yes', OJSyntax = 'PB'");
                        key.SetValue("LogID", "autonet");
                        key.SetValue("LogPass", "autonet");
                        key.SetValue("AppFilter", "ICAR");
                        key.SetValue("Compress", "1");
                        key.SetValue("WinAut", "0");
                        key.SetValue("Unicode", "0");


                    }


                    using (key_to_set_default_profile)
                    {
                        key_to_set_default_profile.SetValue("Perfil", "PROFILE_001");
                    }

                    Console.WriteLine(@"Gotovo!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }



            }
        }

    }
}
