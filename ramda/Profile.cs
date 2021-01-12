using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0521
{
    class Profile
    {
        public string Name { get; set; }
        public int Height { get; set; }

        public Profile() { }
        public Profile(string n, int h)
        {
            Name = n;
            Height = h;
        }

        public override string ToString()
        {
            return Name + "," + Height;
        }


    }

    class ProfileInch
    {
        public string Name { get; set; }
        public double HeightInch { get; set; }

        public ProfileInch() { }
        public ProfileInch(string n, double h)
        {
            Name = n;
            HeightInch = h;
        }

        public override string ToString()
        {
            return Name + "," + HeightInch;
        }


    }
}
