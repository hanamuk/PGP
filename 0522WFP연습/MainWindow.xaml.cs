using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _0522WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent();
           
            if (Control.Singleton.ClientRun() == false)
            {
                Console.WriteLine("서버 접속 실패");
                return;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Control.Singleton.Login(name.Text);
            List<string> temp = new List<string>();
            temp= Control.GetInstance();
              listview.Items.Clear();
            for (int i = 0; i < temp.Count; i++)
            {

                listview.Items.Add(temp[i]);
            }
            //listview.ItemsSource = Control.GetInstance();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Control.Singleton.LogOut(name.Text);
            List<string> temp = new List<string>();
            listview.Items.Clear();
            temp = Control.GetInstance();
            for (int i = 0; i < temp.Count; i++)
            {

                listview.Items.Add(temp[i]);
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Control.Singleton.Message(name.Text,msg.Text);
            List<string> temp = new List<string>();
            listview.Items.Clear();
            temp = Control.GetInstance();
            for (int i = 0; i < temp.Count; i++)
            {

                listview.Items.Add(temp[i]);
            }
        }


        //public void EVENT_SET_LOG_MSG(string strMsg) // 이벤트 받는 메소드
        //{
        //    this.listview.Items.Add(strMsg);
            
        //}
        
    }
}
