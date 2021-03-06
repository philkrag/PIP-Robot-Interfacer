﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PIP_Robotic_Interfacer
{
    class CL_TCPListener
    {

        public static void Run_Listener(object Address)
        {
            int Device_ID = Convert.ToInt32(Address);


            if (CL_Global_Variables.IP_Address[Device_ID] != null && CL_Global_Variables.Port_Address[Device_ID]!=null)
            {

                Int32 Port_Address = Convert.ToInt32(CL_Global_Variables.Port_Address[Device_ID]);
                IPAddress IP_Address = IPAddress.Parse(CL_Global_Variables.IP_Address[Device_ID]);

                TcpListener server = null;
                try
                {
                    // Set the TcpListener on port 13000.
                    //Int32 port = 13000;
                    //IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                    


                    // TcpListener server = new TcpListener(port);
                    server = new TcpListener(IP_Address, Port_Address);

                    // Start listening for client requests.
                    server.Start();

                    // Buffer for reading data
                    Byte[] bytes = new Byte[256];
                    String data = null;

                    // Enter the listening loop.
                    while (true)
                    {
                        Console.ResetColor();
                        Console.WriteLine("[00]\t[" + IP_Address + "][" + Port_Address + "]\tWaiting for a connection.");

                        // Perform a blocking call to accept requests.
                        // You could also user server.AcceptSocket() here.
                        TcpClient client = server.AcceptTcpClient();
                        Console.ResetColor();
                        Console.WriteLine("[00]\t[" + IP_Address + "][" + Port_Address + "]\tConnection Established.");

                        data = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i;

                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("[30]\t[" + IP_Address + "][" + Port_Address + "]\tReceived: " + data);
                            CL_Global_Variables.Received_Data = data;
                            
                            // Process the data sent by the client.
                            data = data.ToUpper();

                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            //Console.ResetColor();
                            //Console.WriteLine("[00] Data Sent: {0}", data);

                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("[30]\t[" + IP_Address + "][" + Port_Address + "]\tSent: " + data);


                        }

                        // Shutdown and end connection
                        client.Close();
                    }
                }
                catch (SocketException e)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("[99] [" + IP_Address + "][" + Port_Address + "] SocketException: {0}", e);
                }

                finally
                {
                    // Stop listening for new clients.
                    server.Stop();
                }

                Console.WriteLine("\nHit enter to continue...");
                Console.Read();
            }
        }



    }
}
