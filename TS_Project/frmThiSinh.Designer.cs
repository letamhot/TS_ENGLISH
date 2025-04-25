namespace TS_Project
{
    partial class frmThiSinh
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThiSinh));
            this.lblTenThiSinh = new System.Windows.Forms.Label();
            this.lblTenTruong = new System.Windows.Forms.Label();
            this.lblThoiGian = new System.Windows.Forms.Label();
            this.pnlNoiDung = new System.Windows.Forms.Panel();
            this.tmClient = new System.Windows.Forms.Timer(this.components);
            this.lblTongDiem = new System.Windows.Forms.Label();
            this.btnCloseTS = new System.Windows.Forms.PictureBox();
            this.btnMiniTS = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnCloseTS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMiniTS)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTenThiSinh
            // 
            this.lblTenThiSinh.BackColor = System.Drawing.Color.Transparent;
            this.lblTenThiSinh.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenThiSinh.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTenThiSinh.Location = new System.Drawing.Point(523, 147);
            this.lblTenThiSinh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTenThiSinh.Name = "lblTenThiSinh";
            this.lblTenThiSinh.Size = new System.Drawing.Size(388, 54);
            this.lblTenThiSinh.TabIndex = 0;
            this.lblTenThiSinh.Text = "NGUYỄN TRẦN HOÀNG GIANG";
            this.lblTenThiSinh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTenTruong
            // 
            this.lblTenTruong.BackColor = System.Drawing.Color.Transparent;
            this.lblTenTruong.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenTruong.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTenTruong.Location = new System.Drawing.Point(977, 147);
            this.lblTenTruong.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTenTruong.Name = "lblTenTruong";
            this.lblTenTruong.Size = new System.Drawing.Size(391, 58);
            this.lblTenTruong.TabIndex = 0;
            this.lblTenTruong.Text = "TRƯỜNG THCS THỊ TRẤN QUÁN HÀU";
            this.lblTenTruong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblThoiGian
            // 
            this.lblThoiGian.BackColor = System.Drawing.Color.Transparent;
            this.lblThoiGian.Font = new System.Drawing.Font("Showcard Gothic", 50.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThoiGian.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblThoiGian.Location = new System.Drawing.Point(103, 292);
            this.lblThoiGian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThoiGian.Name = "lblThoiGian";
            this.lblThoiGian.Size = new System.Drawing.Size(172, 78);
            this.lblThoiGian.TabIndex = 0;
            this.lblThoiGian.Text = "END";
            this.lblThoiGian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblThoiGian.Visible = false;
            // 
            // pnlNoiDung
            // 
            this.pnlNoiDung.BackColor = System.Drawing.Color.Transparent;
            this.pnlNoiDung.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlNoiDung.Location = new System.Drawing.Point(364, 222);
            this.pnlNoiDung.Margin = new System.Windows.Forms.Padding(4);
            this.pnlNoiDung.Name = "pnlNoiDung";
            this.pnlNoiDung.Size = new System.Drawing.Size(1004, 530);
            this.pnlNoiDung.TabIndex = 1;
            this.pnlNoiDung.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlNoiDung_Paint);
            // 
            // tmClient
            // 
            this.tmClient.Interval = 1000;
            this.tmClient.Tick += new System.EventHandler(this.tmClient_Tick);
            // 
            // lblTongDiem
            // 
            this.lblTongDiem.BackColor = System.Drawing.Color.Transparent;
            this.lblTongDiem.Font = new System.Drawing.Font("Showcard Gothic", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongDiem.ForeColor = System.Drawing.Color.Red;
            this.lblTongDiem.Location = new System.Drawing.Point(69, 528);
            this.lblTongDiem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTongDiem.Name = "lblTongDiem";
            this.lblTongDiem.Size = new System.Drawing.Size(239, 144);
            this.lblTongDiem.TabIndex = 2;
            this.lblTongDiem.Text = "0";
            this.lblTongDiem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTongDiem.Visible = false;
            // 
            // btnCloseTS
            // 
            this.btnCloseTS.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseTS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCloseTS.BackgroundImage")));
            this.btnCloseTS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseTS.Location = new System.Drawing.Point(1348, 9);
            this.btnCloseTS.Margin = new System.Windows.Forms.Padding(4);
            this.btnCloseTS.Name = "btnCloseTS";
            this.btnCloseTS.Size = new System.Drawing.Size(20, 21);
            this.btnCloseTS.TabIndex = 82;
            this.btnCloseTS.TabStop = false;
            this.btnCloseTS.Click += new System.EventHandler(this.btnCloseTS_Click);
            // 
            // btnMiniTS
            // 
            this.btnMiniTS.BackColor = System.Drawing.Color.Transparent;
            this.btnMiniTS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMiniTS.BackgroundImage")));
            this.btnMiniTS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMiniTS.Location = new System.Drawing.Point(1325, 9);
            this.btnMiniTS.Margin = new System.Windows.Forms.Padding(4);
            this.btnMiniTS.Name = "btnMiniTS";
            this.btnMiniTS.Size = new System.Drawing.Size(19, 21);
            this.btnMiniTS.TabIndex = 83;
            this.btnMiniTS.TabStop = false;
            this.btnMiniTS.Click += new System.EventHandler(this.btnMiniTS_Click);
            // 
            // frmThiSinh
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1386, 788);
            this.Controls.Add(this.btnMiniTS);
            this.Controls.Add(this.btnCloseTS);
            this.Controls.Add(this.lblTongDiem);
            this.Controls.Add(this.pnlNoiDung);
            this.Controls.Add(this.lblThoiGian);
            this.Controls.Add(this.lblTenThiSinh);
            this.Controls.Add(this.lblTenTruong);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmThiSinh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "THÍ SINH";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmThiSinh_FormClosing);
            this.Load += new System.EventHandler(this.frmThiSinh_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnCloseTS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMiniTS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlNoiDung;
        private System.Windows.Forms.Label lblTenThiSinh;
        private System.Windows.Forms.Label lblTenTruong;
        private System.Windows.Forms.Label lblThoiGian;
        private System.Windows.Forms.Timer tmClient;
        private System.Windows.Forms.Label lblTongDiem;
        private System.Windows.Forms.PictureBox btnCloseTS;
        private System.Windows.Forms.PictureBox btnMiniTS;
    }
}