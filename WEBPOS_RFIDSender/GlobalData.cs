using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBPOS_RFIDSender
{
    internal class GlobalData
    {
        public static CameraController cam_check = null;
        public static List<string> list_rfid_checkin = new List<string>();
        public static List<string> list_rfid_checkout = new List<string>();
        public static List<string> temp_list_rfid = new List<string>();
        public static SortedList<int, string> recent_list_rfid = new SortedList<int, string>();

    }

}
