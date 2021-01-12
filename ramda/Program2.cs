using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _0521
{

    delegate int DemoDele(int a, int b);

    class Program2
    {

        static int Sum(int a, int b)
        {
            int sum = 0;
            for (; a <= b; a++)
            {
                sum += a;
                Console.WriteLine("Sum:{0}", a);
                Thread.Sleep(100);  // 0.1초
            }
            return sum;
        }


        //동기방식
        static void exam1()
        {
            DemoDele dele = Sum;

            //둘 다 동일한 동기방식 호출...
            dele(1, 100);           //내부적으로  dele.Invoke(1,100)을 호출함
            dele.Invoke(1, 100);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Main:{0}", i);
                Thread.Sleep(100);
            }
            Console.ReadLine();
        }

        //비동기방식
        static void exam2()
        {
            DemoDele dele = Sum;
            //비동기방식 호출.
            dele.BeginInvoke(1, 100, EndSum, "TEST");
            dele.Invoke(1, 100);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Main:{0}", i);
                Thread.Sleep(100);
            }
            Console.ReadLine();
        }

        static void EndSum(IAsyncResult iar)
        {
            object obj = iar.AsyncState;

            AsyncResult ar = iar as AsyncResult;
            DemoDele dele = ar.AsyncDelegate as DemoDele;
            Console.WriteLine("전달받은 인자{0}, 수행결과:{1}",
                obj, dele.EndInvoke(iar));
        }



        static void Main(string[] args)
        {

            exam2();

            
        }
    }
}
