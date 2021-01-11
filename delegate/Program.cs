using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0504WinForm
{
    class Program
    {

        static void exam1()
        {
            Cal cal = new Cal();
            cal.Add(10, 20);
            Console.WriteLine(cal.Result);
        }

        static void GetResult(int n1, int n2, char op, float result)
        {

            Console.WriteLine("{0} {1} {2} = {3}",
                n1, op, n2, result);
        }


        static void exam2()
        {
            Cal cal = new Cal();

            //3. 델리게이션 객체에 함수 등록
            //아래 두가지 방식은 동일한 결과 발생
            //3.1 명시적 : 객체를 생성하면서 생성자로 전달하는 법
            cal.DelCallback = new CalResultDel(GetResult);
            //3.2 암시적 : 함수를 대입연산
            cal.DelCallback = GetResult;

            cal.Add(10, 20);
        }

        static void GetString(string msg)
        {
            Console.WriteLine(msg);
        }

        static void exam3()
        {
            Cal1 cal = new Cal1();
            cal.StringEvent += GetString;
            cal.GeResultEvent += GetResult;

            cal.Add(10, 20);
            cal.Sub(10, 20);

        }



        static void Main(string[] args)
        {

            exam3();

        }
    }
}
