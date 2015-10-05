/*
 * Created by SharpDevelop.
 * User: mrtorres
 * Date: 8/10/2015
 * Time: 1:49 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace RustServerManager
{
    /// <summary>
    /// Description of Menu.
    /// </summary>
    public class Menu
    {
		
        private ServerList m_list;
		
        // constructors
		
        // default constructor makes a fresh list and tries to deserialize it
        public Menu()
        {
            m_list = new ServerList();
            m_list.deSerialize();
        }
		
        // overloaded constructor takes a list and makes it the list for this menu
        public Menu(ref ServerList list)
        {
            m_list = list;			
        }
		
        // void methods
		
        public void listMenu()
        {
            int choice;
			
            Console.Clear();
			
            Console.WriteLine("Server List");
            Console.WriteLine("-----------\n");
			
            foreach (RustServer server in m_list.list)
            {
                Console.Write(server.hostName + "\t");
                if (server.isDev)
                {
                    Console.Write("Dev ");
                }
                else
                {
                    Console.Write("Main ");
                }
                Console.WriteLine("branch");
            }
        }

        public void mainMenu()
        {			
            int choice;
			
            Console.Clear();

            Console.WriteLine("Rust Server Menu");
            Console.WriteLine("----------------\n");
            Console.WriteLine("1 - List servers");
            Console.WriteLine("2 - Add server");
            Console.WriteLine("3 - Remove server");
            Console.WriteLine("4 - Edit server");
            Console.WriteLine("5 - Quit\n");
			
            // keep asking for input until valid entry is received
            do
            {
                // get us into a loop that will only be exited if there are no exceptions caught
                while (true)
                {
                    try
                    {
                        Console.Write("Enter choice: ");
                        choice = Convert.ToInt32(Console.ReadLine());
                        break;
                    }
                    catch (Exception e)
                    {
                    }
                }
            } while (choice < 1 || choice > 5);
			
			
            // take action depending on input.
            switch (choice)
            {
                case 1:
                    listMenu();
                    break;
                case 2:
                    addMenu();
                    break;
                case 3:
                    removeMenu();
                    break;
                case 4:
                    editMenu();
                    break;
                case 5:
                    quit();
                    break;
            }    
        }

        public void quit()
        {
            m_list.serialize();
            Console.WriteLine("Thanks for using Rust Server Manager!");
        }

        public void addMenu()
        {
            ServerDownloader.install();
            setup();
        }

        public void removeMenu()
        {
            
        }

        public void editMenu()
        {
            
        }

        public void setup()
        {
            //request initial server information for buildArgs
            Console.WriteLine("Lets setup your server startup variables");
            Console.WriteLine("----------------\n");
            //Set initial values
            Console.Write("Hostname (" + RustServer.hostName + ") : ");
            RustServer.hostName = Convert.ToString(Console.ReadLine());

            Console.Write("Identity aka save folder (" + RustServer.identity + ") : ");
            RustServer.identity = Convert.ToString(Console.ReadLine());

            Console.Write("Seed (" + RustServer.m_seed + ") : ");
            RustServer.m_seed = Convert.ToString(Console.ReadLine());

            Console.Write("World Size (" + RustServer.m_worldSize + ") : ");
            RustServer.m_worldSize = Convert.ToInt32(Console.ReadLine());

            Console.Write("Port number (" + RustServer.m_portNumber + ") : ");
            RustServer.m_portNumber = Convert.ToString(Console.ReadLine());

            Console.Write("Max players (" + RustServer.m_maxPlayers + ") : ");
            RustServer.m_maxPlayers = Convert.ToInt32(Console.ReadLine());

            Console.Write("Did you want to setup RCON (no)? ");
            var rcon = Convert.ToString(Console.ReadLine());
            if (rcon.ToLower() == "yes")
            {
                Console.Write("Rcon Port (" + RustServer.m_rconPort + ") : ");
                RustServer.m_rconPort = Convert.ToString(Console.ReadLine());
                Console.Write("Rcon Password (" + RustServer.m_rconPassword + ") : ");
                RustServer.m_rconPassword = Convert.ToString(Console.ReadLine());
            }
            Console.Write("Let's review your server start settings:\n");
            RustServer.summarize();
            Console.Write(test);
            Console.Write("\n Are these settings correct? (yes)");
            var confirm = Convert.ToString(Console.ReadLine());
            if (confirm.ToLower() == "no")
            {
                setup();
            }
            else
            {
                Console.Write("Did you want to start the server? (yes)");
                var startsvr = Convert.ToString(Console.ReadLine());
                if (startsvr.ToLower() == "no")
                {
                    var menu = new Menu();
                    //return to main menu
                    menu.mainMenu();
                    //saving settings to a flat file for reading latter would be a good idea here.
                }
                //start server
                Start();
            }
        }

        public void Start()
        {
            var process = new Process();
            process.StartInfo.WorkingDirectory = @".\Server";
            process.StartInfo.FileName = "RustDedicated.exe";
            process.StartInfo.Arguments = RustServer.buildArgs();
            process.StartInfo.UseShellExecute = true;
            process.Start();
            //need to save process ID to access it later
            Console.Write("Process ID: " + process.Id);
        }
    }
}
