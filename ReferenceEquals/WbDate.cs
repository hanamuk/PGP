using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0429
{
#pragma warning disable CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
    class WbDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public override bool Equals(object obj)
        {
            //WbDate temp = (WbDate)obj;
            //obj가 가지고 있는 객체가 WbDate객체냐?
            WbDate temp = obj as WbDate;
            if (temp == null)
                return false;

            if (Year == temp.Year && Month == temp.Month && Day == temp.Day)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            String temp = String.Format("{0,4}-{1,2}-{2,2}", Year, Month, Day);
            return temp;
            //          return Year + "-" + Month + "-" + Day;
        }
    }
}
