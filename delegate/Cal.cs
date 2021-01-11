using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace _0504WinForm
{

    //1. delegate : 클래스를 정의하는 문법 => 함수 포인터 형식으로 사용
    delegate void CalResultDel(int n1, int n2, char op,float result);

    class Cal
    {
        //2. 레퍼런스 변수선언(아직 객체가 만들어진게 아님. new를 해야함)
        public CalResultDel DelCallback { get; set; }

        public float Result { get; private set; }

        public void Add(int n1, int n2)
        {
            Result = (float)n1 + n2;

            //callback 호출 방생
            //1. 암시적으로 invoke 호출시킴 미리 등록된 함수를 호출시키는 과정
            DelCallback(n1, n2, '+', Result);
            //2명시적으로 호출방법
           DelCallback.Invoke(n1, n2, '+', Result);
        }

        public void Sub(int n1, int n2)
        {
            Result = (float)n1 - n2;
        }


    }

}