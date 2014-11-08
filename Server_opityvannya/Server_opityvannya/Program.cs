using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Server_opityvannya
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnect DBC = new DBConnect();  // Create an object
            DBC.Initialize();
            TcpListener server = null;
            try
            {
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                ThreadPool.SetMinThreads(2, 2);

                Int32 port = 9595;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                int counter = 0;
                server = new TcpListener(localAddr, port);

                server.Start();

                while (true)
                {
                    Console.Write("\n\tWaiting for a connection... ");

                    ThreadPool.QueueUserWorkItem(Connecting, server.AcceptTcpClient());
                    counter++;
                    Console.Write("\nConnection №" + counter.ToString() + "!");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
        static void Connecting(object client_obj)
        {
            DBConnect DB = new DBConnect();
            Byte[] byte1 = new Byte[256];
            Byte[] byte2 = new Byte[256];
            Byte[] byte3 = new Byte[256];
            string[] txt_msg = new string[2];
            int[] arr = new int[5];

            TcpClient client = client_obj as TcpClient;

            NetworkStream stream = client.GetStream();

            int i;
            i = stream.Read(byte1, 0, byte1.Length);
            txt_msg[0] = System.Text.Encoding.Unicode.GetString(byte1, 0, i);

            i = stream.Read(byte2, 0, byte2.Length);
            txt_msg[1] = System.Text.Encoding.Unicode.GetString(byte2, 0, i);

            i = stream.Read(byte3, 0, byte3.Length);
            arr = (from j in byte3 select (int)j).ToArray();

            DB.Insert(txt_msg[0], txt_msg[1], arr);

            client.Close();
        }
    }
}


class DBConnect
{

    public MySqlConnection connection;
    public string server;
    public string database;
    public string uid;
    public string password;

    //Constructor
    public DBConnect()
    {
        Initialize();
    }

    //Initialize values
    public void Initialize()
    {
        server = "localhost";
        database = "results_of_a_poll";
        uid = "root";
        password = "121212";
        string connectionString;
        connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
    }

    public bool OpenConnection() //
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    Console.Write("Cannot connect to server.  Contact administrator;\n");
                    break;

                case 1045:
                    Console.Write("Invalid username/password, please try again;\n");
                    break;
            }
            return false;
        }
    }
    public bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            Console.Write("Cannot close connection;\n", ex.Message);
            return false;
        }
    }

    public void Insert(string txt1, string txt2, int[] arr)
    {
        string query = "INSERT INTO pool1 (Sur, City, Answ1, Answ2, Answ3, Answ4, Answ5) VALUES('" 
            + txt1 + "', '" + txt2 + "', '" + arr[0].ToString() + "', '" + arr[1].ToString() + "', '" 
            + arr[2].ToString() + "', '" + arr[3].ToString() + "', '" + arr[4].ToString() + "')";
        if (this.OpenConnection())
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            this.CloseConnection();
        }
    }
}
