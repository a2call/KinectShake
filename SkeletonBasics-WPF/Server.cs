//This code is adopted from http://msdn.microsoft.com/en-us/library/6y0e13d3.aspx

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    public static class Server
    {
        public static OpenGLWin oglwin_; //use this to pass commands to openglwindow 

        // Incoming data from the client.
        public static string data = null;

        public static void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8888);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    Debug.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    // Show the data on the console.
                    Debug.WriteLine("Text received : "+ data);

                    handleMsg(data);

                    // Echo the data back to the client.
                    //Encoding.ASCII.GetBytes(statusMsg());
                    handler.Send(Encoding.ASCII.GetBytes(genMsg(oglwin_.getStatus())));

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }


        public static String genMsg(AtomsDB.StatusType s){
            switch (s)
            {
                case AtomsDB.StatusType.OK:
                    return "ok";
                case AtomsDB.StatusType.ATTRACT:
                    return "attract";
                case AtomsDB.StatusType.REPEL:
                    return "repel";
                default:
                    return "ok";
            }
        }

        public static void handleMsg(String data)
        {
            if (data == "hydrogen")
            {
                oglwin_.addAtom(Atom.AtomType.H);
            }
            if (data == "oxygen")
            {
                oglwin_.addAtom(Atom.AtomType.O);
            }
            if (data == "drop")
            {
                oglwin_.dropAtom();
            }        
        }
    }
}