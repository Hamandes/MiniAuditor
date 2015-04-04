using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MiniAuditorServer
{
    public class IPConnection
    {
        Socket handler;
        Thread serverThread;
        string adressTh;
        int portTh;

        public string GetLocalEndPoint()
        {
            if (handler != null)
            {
                return handler.LocalEndPoint.ToString();
            }
            return "no connection";
        }

        public string GetRemoteEndPoint()
        {
            if (handler != null)
            {
                return handler.RemoteEndPoint.ToString();
            }
            return "no connection";
        }

        public event EventHandler OnClientConnect;

        public void Connect(string address, int port)
        {
            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry(address);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
            handler = sender;
        }

        public void StartServer(string address, int port)
        {
            adressTh = address;
            portTh = port;
            serverThread = new Thread(StartServerThread);
            serverThread.Start();
            serverThread.IsBackground = true;
        }

        public void StopServer()
        {
            CloseConnection();
            serverThread.Interrupt();
        }

        void StartServerThread()
        {
            // Устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.GetHostEntry(adressTh);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, portTh);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Начинаем слушать соединения
                while (true)
                {
                    // Программа приостанавливается, ожидая входящее соединение
                    handler = sListener.Accept();

                    if (OnClientConnect != null)
                    {
                        OnClientConnect(this, new EventArgs());
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SendMessage(string message)
        {
            if (handler != null)
            {
                byte[] msg = Encoding.UTF8.GetBytes(message);

                // Отправляем данные через сокет
                int bytesSent = handler.Send(msg);
            }            
        }

        public string ReceiveMessage()
        {
            if (handler != null)
            {
                // Буфер для входящих данных
                byte[] bytes = new byte[1024];
                // Получаем ответ от сервера
                int bytesRec = handler.Receive(bytes);
                return Encoding.UTF8.GetString(bytes, 0, bytesRec);
            }
            return "error";
        }

        public void CloseConnection()
        {
            if (handler != null)
            {
                // Освобождаем сокет
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                handler = null; 
            }      
        }
    }
}
