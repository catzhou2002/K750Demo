using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tz.CardRS
{
    public class Usb : ICardRS
    {

        private DateTime _LastQueryDt = new DateTime(1970, 1, 1);
        private byte[] ConfirmData = new byte[] { 0x05, 0x30, 0x30 };
        private List<byte[]> _CommandData = new List<byte[]>();
        private UsbApi _UsbApi;
        public Usb()
        {
            foreach (var item in new string[] { "AP", "DB", "RS", "FC7", "FC4", "FC8" })
            {
                var data = new byte[item.Length + 7];
                data[0] = 0x02;//stx
                data[1] = 0x30; //addr H
                data[2] = 0x30; //addr L
                data[3] = 0x00; //len H
                data[4] = (byte)item.Length; //len L
                int index = 5;
                foreach (var c in item)
                    data[index++] = (byte)c;
                data[index++] = 0x03; //etx
                data[index] = 0x02; //校验码
                for (int i = 1; i < data.Length - 1; i++)
                    data[index] ^= data[i];
                _CommandData.Add(data);
            }

            var fileName = Settings.Default.UsbFileName;
            if (!string.IsNullOrEmpty(fileName) && TryOpenUsb(fileName))
                return;

            foreach (var fn in UsbApi.GetUsbFileNames())
            {
                if (TryOpenUsb(fn))
                {
                    Settings.Default.UsbFileName = fn;
                    Settings.Default.Save();
                    return;
                }
            }
            _UsbApi = null;
        }
        private bool TryOpenUsb(string fileName)
        {
            try
            {
                _UsbApi = new UsbApi(fileName);
                ExecuteCommand(ECommand.复位);
                //Query();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool ExecuteCommand(ECommand eCommand)
        {
            var data = _CommandData[(int)eCommand];
            _UsbApi.Write(data);
            var dataR = _UsbApi.Read();
            if (dataR[1] == 0x06)
            {
                _UsbApi.Write(ConfirmData);
                return true;
            }
            else
                return false;
        }

        public ECardRSQueryStatus Query()
        {
            var ts = 200 - (DateTime.Now - _LastQueryDt).TotalMilliseconds;
            _LastQueryDt = DateTime.Now;
            if (ts > 0)
                Task.Delay((int)(ts)).Wait();
            if (ExecuteCommand(ECommand.查询))
            {
                var data = _UsbApi.Read();
                int result = 0;
                for (int i = 0; i < 4; i++)
                    result += ((int)data[i + 8] - 48) << ((3 - i) * 4);
                var curstatus = (ECardRSQueryStatus)result;
                return (ECardRSQueryStatus)result;
            }
            else
            {
                return ECardRSQueryStatus.查询出错;
            }
        }
        ///// <summary>
        ///// 等待插卡
        ///// </summary>
        ///// <param name="cts">取消等待插卡</param>
        ///// <param name="seconds">几秒查询一次</param>
        ///// <param name="action">有卡片在读写位置的操作,线程中</param>
        //public bool WaitInsertCard(System.Threading.CancellationTokenSource cts, int milliseconds, Action action)
        //{
        //    if (_UsbApi == null)
        //        return false;

        //    Task.Factory.StartNew(() =>
        //    {
        //        ExecuteCommand(ECommand.使能前端进卡);
        //        var status = ECardRSQueryStatus.读写位置 | ECardRSQueryStatus.取卡口;
        //        while (!cts.IsCancellationRequested)
        //        {
        //            var q = Query();
        //            if ((q & status) == status)
        //            {
        //                action();
        //                break;
        //            }
        //            Task.Delay(milliseconds).Wait();
        //        }
        //    }, cts.Token);
        //    return true;
        //}
        public void Close()
        {
            if (_UsbApi != null)
                _UsbApi.Close();
        }
    }
}
