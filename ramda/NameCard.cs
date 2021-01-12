using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0521
{
    class NameCard
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public NameCard() { }
        public NameCard(string _n, string _p)
        {
            Name = _n;
            Phone = _p;
        }

        public override string ToString()
        {
            return Name + "," + Phone;
        }

    }
}
