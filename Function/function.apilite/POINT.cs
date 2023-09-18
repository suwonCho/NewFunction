using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace function.apilite
{
    public struct POINT
    {
        public int X;
        public int Y;

        public override bool Equals(object obj)
        {
            try
            {
                POINT pnt = (POINT)obj;


                if (X == pnt.X && Y == pnt.Y) return true;

                return false;
            }
            catch
            {
                return false;
            }

        }

    }
}
