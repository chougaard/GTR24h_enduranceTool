using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

//This class is not at all done.
//It comes from another program, I wrote earlier.
//It's only intended for proof of concept. Probably not even working.


namespace Server
{
    class TCPConnection
    {
        public const bool DEBUG = false;

        public const int BUFFER_SIZE = 900000;				//set size of receive buffer

        static public TcpClient ClientSocket;				//Tcp Client til data
        static public NetworkStream serverStream;			//Network stream

        static byte[] outStream;							//Outgoing message
        static byte[] inStream = new byte[BUFFER_SIZE];		//Incoming message
        static string inString;

        static string ServerIp;
        static int ServerPort;

        public static bool ConnectToIp(string ip, int port)
        {
            //Tcp Socket:
            ClientSocket = new TcpClient();

            //Connection:
            ServerIp = ip != "" ? ip : "127.0.0.1";

            if (port != 0)
                ServerPort = port;
            else
                ServerPort = 9000;

            try
            {
                ClientSocket.Connect(ServerIp, ServerPort);
                serverStream = ClientSocket.GetStream();
                Console.WriteLine("Connected to server!");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not create connection");
                Console.WriteLine(e.Message);
                return false;
            }

        }


        //Outgoing communication:
        public static void SendTCPMessage(string msg)
        {
            outStream = System.Text.Encoding.ASCII.GetBytes(msg);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }


        //Incoming communication:
        public static string ReceiveTCPMessage()
        {
            serverStream.Read(inStream, 0, (int)ClientSocket.ReceiveBufferSize);
            inString = System.Text.Encoding.ASCII.GetString(inStream);
            serverStream.Flush();			//Clear Stream
            return inString;
        }
    }
}
