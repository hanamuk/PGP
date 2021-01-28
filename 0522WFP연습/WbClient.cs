using _0522_ChatMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _0522WPF
{
    class WbClient
    {

        private TcpClient server;
        private NetworkStream ns;
        private MemoryStream strm;

        public bool Start(string ip, int port)
        {
            try
            {
                server = new TcpClient(ip, port);  // 서버에 접속               

                //통신스레드
                //WorkThread 실행
                Thread tr = new Thread(new ThreadStart(WorkThread));
                tr.IsBackground = true;
                tr.Start();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void WorkThread()
        {
            try
            {
                while (true)
                {
                    ns = server.GetStream();      // NetworkStream 객체를 얻어옴
                    strm = new MemoryStream();  // 메모리 스트림 생성

                    ChatMessage cmessage = RecvData(ns, strm);

                    //Control에게 보낸다....
                    Control.Singleton.RecvData(cmessage);

                    IPEndPoint ip = (IPEndPoint)server.Client.RemoteEndPoint;
                  

                }
            }
            catch (Exception ex)        // 해당 쓰레드 종료 
            {
                MessageBox.Show(ex.Message);
        

            }
        }

        public void SendData(ChatMessage msg)
        {
            ns = server.GetStream();           // NetworkStream 얻기

            IFormatter formatter = new SoapFormatter();   // IFormatter 생성
            strm = new MemoryStream();   // 메모리 스트림 생성 

            formatter.Serialize(strm, msg);    // 메모리 스트림에 직렬화 인스턴스 생성

            byte[] data = strm.GetBuffer();
            int data_size = (int)strm.Length;
            byte[] size = BitConverter.GetBytes(data_size);  // 메모리 스트림 전체 크기전송

            ns.Write(size, 0, 4);              // 전송할 데이터 사이즈 전송
            ns.Write(data, 0, data_size);      // 메모리 스트림에 있는 데이터 전송
            ns.Flush();

            
          
        }

        private ChatMessage RecvData(NetworkStream _ns, MemoryStream _strm)
        {
            // 수신할 스트림 데이터 크기 알아내기
            byte[] data = new byte[4];
            int recv_size = ns.Read(data, 0, 4);
            int size = BitConverter.ToInt32(data, 0);

            int receive = 0;

            while (size > 0)  // 상대방이 보낸 직렬화 스트림 읽기
            {
                data = new byte[1024];
                recv_size = ns.Read(data, 0, size);
                strm.Write(data, receive, recv_size);
                receive += recv_size;
                size -= recv_size;
            }

            strm.Position = 0;

            IFormatter formatter = new SoapFormatter();
            // 직렬화 인스턴스 복원 
            return (ChatMessage)formatter.Deserialize(strm);
        }

        public void Stop()
        {
            server.Close();
        }
    }
}
