using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBPOS_RFIDSender
{
    internal class Check
    {
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        //public Boolean In { get; set; }
        public Check()
        {
            this.Checkin = DateTime.Now;
            this.Checkout = DateTime.Now;
            //this.In = cIn;
        }
    }
}
