namespace TzK750Demo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.BtnClose = new System.Windows.Forms.ToolStripButton();
            this.BtnInit = new System.Windows.Forms.ToolStripButton();
            this.BtnSendRW = new System.Windows.Forms.ToolStripButton();
            this.BtnSendTake = new System.Windows.Forms.ToolStripButton();
            this.BtnRec = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnInit,
            this.BtnClose,
            this.BtnSendRW,
            this.BtnSendTake,
            this.BtnRec});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(673, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // BtnClose
            // 
            this.BtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnClose.Enabled = false;
            this.BtnClose.Image = ((System.Drawing.Image)(resources.GetObject("BtnClose.Image")));
            this.BtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(36, 22);
            this.BtnClose.Text = "关闭";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnInit
            // 
            this.BtnInit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnInit.Image = ((System.Drawing.Image)(resources.GetObject("BtnInit.Image")));
            this.BtnInit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnInit.Name = "BtnInit";
            this.BtnInit.Size = new System.Drawing.Size(71, 22);
            this.BtnInit.Text = "初始化Usb";
            this.BtnInit.Click += new System.EventHandler(this.BtnInit_Click);
            // 
            // BtnSendRW
            // 
            this.BtnSendRW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnSendRW.Enabled = false;
            this.BtnSendRW.Image = ((System.Drawing.Image)(resources.GetObject("BtnSendRW.Image")));
            this.BtnSendRW.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnSendRW.Name = "BtnSendRW";
            this.BtnSendRW.Size = new System.Drawing.Size(96, 22);
            this.BtnSendRW.Text = "发卡到读写位置";
            this.BtnSendRW.Click += new System.EventHandler(this.BtnSendRW_Click);
            // 
            // BtnSendTake
            // 
            this.BtnSendTake.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnSendTake.Enabled = false;
            this.BtnSendTake.Image = ((System.Drawing.Image)(resources.GetObject("BtnSendTake.Image")));
            this.BtnSendTake.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnSendTake.Name = "BtnSendTake";
            this.BtnSendTake.Size = new System.Drawing.Size(84, 22);
            this.BtnSendTake.Text = "发卡到取卡口";
            this.BtnSendTake.Click += new System.EventHandler(this.BtnSendTake_Click);
            // 
            // BtnRec
            // 
            this.BtnRec.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnRec.Enabled = false;
            this.BtnRec.Image = ((System.Drawing.Image)(resources.GetObject("BtnRec.Image")));
            this.BtnRec.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnRec.Name = "BtnRec";
            this.BtnRec.Size = new System.Drawing.Size(36, 22);
            this.BtnRec.Text = "收卡";
            this.BtnRec.Click += new System.EventHandler(this.BtnRec_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "Lstatus";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 400);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton BtnInit;
        private System.Windows.Forms.ToolStripButton BtnClose;
        private System.Windows.Forms.ToolStripButton BtnSendRW;
        private System.Windows.Forms.ToolStripButton BtnSendTake;
        private System.Windows.Forms.ToolStripButton BtnRec;
        private System.Windows.Forms.Label label1;
    }
}

