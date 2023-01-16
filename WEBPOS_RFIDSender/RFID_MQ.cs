using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabbitMQ.Client;
using Rfid.Helper.Enums;
using Rfid.Helper.Services.Mq;
using InterfaceLib.DEFINE;


namespace WEBPOS_RFIDSender.MQcontrol
{
    public class RFID_MQ
    {
        private static string hostIP = "192.168.1.42";

        public void startscan(String hostIP)

        {

            MqClient clientSelfRegi = new MqClient(MqClientAppName.SELF_REGI_APP, hostIP);
            IConnection conSelfRegi;
            IModel channelSelfRegi;
            conSelfRegi = clientSelfRegi.GetRbMqConnection();
            channelSelfRegi = clientSelfRegi.CreateRbMqChannel(conSelfRegi);
            clientSelfRegi.ReceivedDataEvent += SelfRegi_ReceivedDataEvent;
            clientSelfRegi.SubcribeMessages(channelSelfRegi);
        }

        private void SelfRegi_ReceivedDataEvent(List<InventoryTagInfo> tagsInfoList)
        {

            tagsInfoList.ForEach(p => {

                Console.WriteLine(p.Epc.ToString());


            });
        }
    }

}
