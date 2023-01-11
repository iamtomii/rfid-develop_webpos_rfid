using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBPOS_RFIDSender.Common
{
    public class POINT
    {
        public int X { get; set; }

        public int Y { get; set; }

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public POINT()
        {
        }

        public Int32 ToPostParam()
        {
            return (Y * 65536) + (X);
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }
    }
}
