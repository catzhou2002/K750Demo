using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TzK750Demo
{
    public partial class Form1 : Form
    {

        Tz.CardRS.Usb _Usb;
        List<string> LastItems = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        System.Threading.CancellationTokenSource CTS;
        private void BtnInit_Click(object sender, EventArgs e)
        {
            this.BtnInit.Enabled = false;
            this.BtnClose.Enabled = this.BtnSendRW.Enabled = this.BtnSendTake.Enabled = this.BtnRec.Enabled = true;

            this.CTS = new System.Threading.CancellationTokenSource();
            _Usb = new Tz.CardRS.Usb();
            Task.Factory.StartNew(() =>
            {
                while (!CTS.IsCancellationRequested)
                {
                    _Usb.ExecuteCommand(Tz.CardRS.ECommand.使能前端进卡);
                    var q = _Usb.Query();
                    this.Invoke(new Action(() =>
                    {
                        this.label1.Text =DateTime.Now+ q.ToString();
                    }));
                    Task.Delay(500).Wait(); ;
                }
            }, CTS.Token);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.BtnInit.Enabled = true;
            this.BtnClose.Enabled = this.BtnSendRW.Enabled = this.BtnSendTake.Enabled = this.BtnRec.Enabled = false;

            CTS.Cancel();
            _Usb.Close();
            _Usb = null;
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (_Usb != null)
            {
                CTS.Cancel();
                _Usb.Close();
            }
        }

        private void BtnSendRW_Click(object sender, EventArgs e)
        {
            _Usb.ExecuteCommand(Tz.CardRS.ECommand.发卡到读写卡位置);
        }

        private void BtnSendTake_Click(object sender, EventArgs e)
        {
            _Usb.ExecuteCommand(Tz.CardRS.ECommand.发卡到取卡口);
        }

        private void BtnRec_Click(object sender, EventArgs e)
        {
            _Usb.ExecuteCommand(Tz.CardRS.ECommand.循环收卡);
        }
    }
}
