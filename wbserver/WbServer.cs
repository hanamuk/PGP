using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _WbServer
{
    public enum LogType { SERVERRUN, CONNECT, DISCONNECT, ERROR }

    public delegate void DelLogMessage(LogType lt, String ip, int port);

    public delegate void DelRecvData(Socket sock, String msg);

    public class WbServer
    {
        private Socket server;

        public Thread ServerThread { get; private set; }

        private List<Socket> array = new List<Socket>();

        #region CallBack처리부분
        private DelLogMessage logmsg;
        private DelRecvData rdata;

        public WbServer(DelLogMessage lm, DelRecvData rd)
        {
            logmsg = lm;
            rdata = rd;
        }
        #endregion

        // 서버소켓 생성 및 접속 Thread를 실행 
        // port = 서버에서 사용할 포트번호 
        // return = 성공시 true, 실패시 fasle를 반환 
        public bool CreateSocket(int port)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9000);
                server = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Stream, ProtocolType.Tcp);
                server.Bind(ipep);
                server.Listen(20);

                // 서버의 ListenThread 생성 실행
                ServerThread = new Thread(new ThreadStart(ListenThread));
                ServerThread.IsBackground = true;
                ServerThread.Start();
                //"61.81.99.67"
                logmsg(LogType.SERVERRUN, "127.0.0.1", ipep.Port);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int SendData(Socket s, String msg, int size)
        {
            byte[] data = Encoding.Default.GetBytes(msg);
            SendData(s, data);
            return size;
        }

        public void SendAllData(Socket sock, String msg, int size)
        {
            byte[] data = Encoding.Default.GetBytes(msg);

            // 1대 다 통신 
            foreach (Socket s in array)
            {
                SendData(s, msg, size);
            }
        }

        public void GetRemoteIpPort(Socket sock, out String ip, out int port)
        {
            IPEndPoint ep = (IPEndPoint)sock.RemoteEndPoint;
            ip = ep.Address.ToString();
            port = ep.Port;
        }

        private void ListenThread()
        {
            while (true)
            {
                try
                {
                    Socket client = server.Accept();  // 클라이언트 접속 대기
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    logmsg(LogType.CONNECT, ip.Address.ToString(), ip.Port); // 접속한 클라이언트 아이피 주소와 접속 포트번호 출력

                    array.Add(client);

                    Thread tr = new Thread(new ParameterizedThreadStart(WorkThread));
                    tr.IsBackground = true;
                    tr.Start(client);
                }
                catch (Exception ex)
                {
                    logmsg(LogType.ERROR, ex.Message, -1);

                    server.Close();
                }

            }
        }

        private void WorkThread(object obj)
        {
            Socket client = obj as Socket;

            try
            {
                byte[] data;

                while (true)
                {
                    int retval = ReceiveData(client, out data);

                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    rdata(client, Encoding.Default.GetString(data, 0, retval));
                }
            }
            catch (Exception)        // 해당 쓰레드 종료 
            {
                array.Remove(client);
                IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                logmsg(LogType.DISCONNECT, ip.Address.ToString(), ip.Port);

                client.Close();
            }
        }

        #region Send & Data 처리부
        private void SendData(Socket sock, byte[] data)
        {
            try
            {
                int total = 0;
                int size = data.Length;
                int left_data = size;
                int send_data = 0;

                // 전송할 데이터의 크기 전달
                byte[] data_size = new byte[4];
                data_size = BitConverter.GetBytes(size);
                send_data = sock.Send(data_size);

                // 실제 데이터 전송
                while (total < size)
                {
                    send_data = sock.Send(data, total, left_data, SocketFlags.None);
                    total += send_data;
                    left_data -= send_data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private int ReceiveData(Socket sock, out byte[] data)
        {
            try
            {
                int total = 0;
                int size = 0;
                int left_data = 0;
                int recv_data = 0;

                // 수신할 데이터 크기 알아내기 
                byte[] data_size = new byte[4];
                recv_data = sock.Receive(data_size, 0, 4, SocketFlags.None);
                size = BitConverter.ToInt32(data_size, 0);
                left_data = size;

                data = new byte[size];

                // 실제 데이터 수신
                while (total < size)
                {
                    recv_data = sock.Receive(data, total, left_data, 0);
                    if (recv_data == 0) break;
                    total += recv_data;
                    left_data -= recv_data;
                }
                return size;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
