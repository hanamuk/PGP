using _0522_ChatMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _0522WPF
{
    public class Control
    {
        public static List<string> str = new List<string>();
    
        #region 싱글톤
        static public Control Singleton { get; private set; }
        static Control()
        {
            Singleton = new Control();
        }
        private Control() { }
        #endregion

        private WbClient client = new WbClient();
        public bool ClientRun()
        {
            return client.Start("127.0.0.1", 9000);
        }

        public void Login(string name)
        {
            ChatMessage ex1 = new ChatMessage();             // 직렬화 인스턴스 생성
            ex1.Flag = 1;
            ex1.Name = name;
            ex1.Msg = "";
            ex1.Dt = DateTime.Now;

            client.SendData(ex1);
        }

        public void LogOut(string name)
        {
            ChatMessage ex1 = new ChatMessage();             // 직렬화 인스턴스 생성
            ex1.Flag = 2;
            ex1.Name = name;
            ex1.Msg = "";
            ex1.Dt = DateTime.Now;

            client.SendData(ex1);

        }

        public void Message(string name, string msg)
        {
            ChatMessage ex1 = new ChatMessage();             // 직렬화 인스턴스 생성
            ex1.Flag = 3;
            ex1.Name = name;
            ex1.Msg = msg;
            ex1.Dt = DateTime.Now;

            client.SendData(ex1);
        }

        public void RecvData(ChatMessage cmessage)
        {

            if (cmessage.Flag == 1)//로그인
            {
                string temp = string.Format("{0}님이 들어오셨습니다", cmessage.Name);
                str.Add(temp);

            }
            else if (cmessage.Flag == 2) //로그아웃
            {
                string temp = string.Format("{0}님이 나가셨습니다", cmessage.Name);
                str.Add(temp);
            }
            else if (cmessage.Flag == 3)//메세지
            {
                string temp = string.Format("[{0}]{1}{2}", cmessage.Name, cmessage.Msg, cmessage.Dt);
                str.Add(temp);
            }
            else
                return;
        }

        public void ClientStop()
        {
            client.Stop();
        }

        public static List<string> GetInstance()
        {
            List<string> temp = new List<string>();
            temp = str;
            return temp;
        }
    }
}
