using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0504WinForm
{
    delegate void EventHandler(int n1, int n2, char op, float result);
    delegate void EventStringHandler(string msg);

    class Cal1
    {
        public event EventHandler GeResultEvent;
        public event EventStringHandler StringEvent;


        public float Result { get; private set; }

        public void Add(int n1, int n2)
        {
            Result = (float)n1 + n2;
            StringEvent("덧셈 연산 수행");
            GeResultEvent(n1, n2, '+', Result);
        }

        public void Sub(int n1, int n2)
        {
            Result = (float)n1 - n2;
            StringEvent("뺄셈 연산 수행");
            GeResultEvent(n1, n2, '-', Result);
        }



    }
}
