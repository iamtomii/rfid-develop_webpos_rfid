using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDAttendance
{
    internal class GlobalData
    {
        

        public static List<string> temp_list_rfid = new List<string>();
        public static SortedList<int, string> recent_list_rfid = new SortedList<int, string>();

    }

}
