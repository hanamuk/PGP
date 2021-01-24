using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;


namespace _0429
{
    class Program
    {
        static void Main(string[] args)
        {
            exam1();
           
        }
        private static void exam1()
        {
            MessageBox.Show("테스트", "캡션" , MessageBoxButtons.YesNo);
        }

        private static void NewMethod()
        {
            WbDate d1 = new WbDate()
            {
                Year = 2020,
                Month = 4,
                Day = 29
            };

            WbDate d2 = d1;

            WbDate d3 = new WbDate()
            {
                Year = 2020,
                Month = 4,
                Day = 29
            };

            //============결과 검증 ================
            Console.WriteLine(d1.Equals(d2));                   //True
            Console.WriteLine(object.ReferenceEquals(d1, d2));   //True

            //부모로부터 물려받은 Equals는 RE의 성질을 갖는다.
            //따라서 사용하려면 재정의를 해야 한다. 
            Console.WriteLine(d1.Equals(d3));                   //True
            Console.WriteLine(object.ReferenceEquals(d1, d3));  //False

            //toString
            //객체명을 출력하면 암시적으로 ToString()을 호출한다.
            //부모로부터 물려받은ToString() : 자신의 네임스페이스.클래스명
            Console.WriteLine(d1.ToString());
            Console.WriteLine(d1);
        }
    }
}
