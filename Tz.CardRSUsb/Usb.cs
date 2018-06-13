using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tz.CardRS
{
    public class Usb
    {
        private UsbApi _UsbApi;
        public Usb()
        {

            //var fileName = Properties.Settings.Default.UsbFileName;

        }
        private bool TryOpenUsb(string fileName)
        {
            try
            {
                _UsbApi = new UsbApi(fileName);
                Query();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Query()
        {
            byte[] b = new byte[]
            {
                0x02,//stx
                0x30, //addr H
                0x30, //addr L
                0x0,//len H
                0x2,//len L
                (byte)'A',
                (byte)'P',
                0x03,//etx;
                0x0
            };
            for (int i = 1; i < b.Length - 1; i++)
                b[b.Length - 1] ^= b[i];
            _UsbApi.Write(b);


            for (int i = 4; i < b.Length; i++)
                b[i] = 0;
            b[1] = 0x05;
            Task.Delay(300).Wait();
            var ret = _UsbApi.WriteThenRead(b);

        }

        public static void Reset()
        {

            ////var t = HidDevice.ReadAsync(inputBuff, 0, 65);


            //byte[] b = new byte[65];
            //b[0] = 1;
            //b[1] = 0x02;//stx
            //b[2] = 0x30; //addr H
            //b[3] = 0x30; //addr L
            //b[4] = 0;//len H
            //b[5] = 2;//len L
            //b[6] = (byte)'R';// 52;//R
            //b[7] = (byte)'S';// 53;//S
            //b[8] = 0x03;//etx;
            //b[9] = 0;
            //for (int i = 1; i < 9; i++)
            //    b[9] ^= b[i];
            //WriteUsbHid(b);

            //for (int i = 4; i < b.Length; i++)
            //    b[i] = 0;
            //b[1] = 0x05;
            //System.Threading.Thread.Sleep(300);

            //WriteUsbHid(b);

        }

    }
}
