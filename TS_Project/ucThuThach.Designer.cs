namespace TS_Project
{
    partial class ucThuThach
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucThuThach));
            this.tmKhamPha = new System.Windows.Forms.Timer(this.components);
            this.pbGui = new System.Windows.Forms.PictureBox();
            this.pbDapAn = new System.Windows.Forms.PictureBox();
            this.txtCauTraLoi = new System.Windows.Forms.TextBox();
            this.lblTGTL = new System.Windows.Forms.Label();
            this.lblThoiGianTraLoi = new System.Windows.Forms.Label();
            this.lblThele = new System.Windows.Forms.Label();
            this.flowPanelSentences = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pbGui)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDapAn)).BeginInit();
            this.SuspendLayout();
            // 
            // tmKhamPha
            // 
            this.tmKhamPha.Interval = 1000;
            this.tmKhamPha.Tick += new System.EventHandler(this.tmKhamPha_Tick);
            // 
            // pbGui
            // 
            this.pbGui.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbGui.BackgroundImage")));
            this.pbGui.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbGui.Location = new System.Drawing.Point(755, 411);
            this.pbGui.Name = "pbGui";
            this.pbGui.Size = new System.Drawing.Size(123, 112);
            this.pbGui.TabIndex = 5;
            this.pbGui.TabStop = false;
            this.pbGui.Click += new System.EventHandler(this.pbGui_Click);
            // 
            // pbDapAn
            // 
            this.pbDapAn.BackColor = System.Drawing.Color.Transparent;
            this.pbDapAn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbDapAn.BackgroundImage")));
            this.pbDapAn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbDapAn.Location = new System.Drawing.Point(105, 403);
            this.pbDapAn.Name = "pbDapAn";
            this.pbDapAn.Size = new System.Drawing.Size(644, 154);
            this.pbDapAn.TabIndex = 6;
            this.pbDapAn.TabStop = false;
            // 
            // txtCauTraLoi
            // 
            this.txtCauTraLoi.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtCauTraLoi.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCauTraLoi.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCauTraLoi.Location = new System.Drawing.Point(263, 413);
            this.txtCauTraLoi.Multiline = true;
            this.txtCauTraLoi.Name = "txtCauTraLoi";
            this.txtCauTraLoi.Size = new System.Drawing.Size(466, 65);
            this.txtCauTraLoi.TabIndex = 0;
            this.txtCauTraLoi.TextChanged += new System.EventHandler(this.txtCauTraLoi_TextChanged);
            this.txtCauTraLoi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCauTraLoi_KeyDown);
            // 
            // lblTGTL
            // 
            this.lblTGTL.AutoSize = true;
            this.lblTGTL.BackColor = System.Drawing.SystemColors.Control;
            this.lblTGTL.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTGTL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTGTL.Location = new System.Drawing.Point(306, 485);
            this.lblTGTL.Name = "lblTGTL";
            this.lblTGTL.Size = new System.Drawing.Size(121, 19);
            this.lblTGTL.TabIndex = 8;
            this.lblTGTL.Text = "Thời gian trả lời:";
            // 
            // lblThoiGianTraLoi
            // 
            this.lblThoiGianTraLoi.AutoSize = true;
            this.lblThoiGianTraLoi.BackColor = System.Drawing.SystemColors.Control;
            this.lblThoiGianTraLoi.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThoiGianTraLoi.Location = new System.Drawing.Point(426, 488);
            this.lblThoiGianTraLoi.Name = "lblThoiGianTraLoi";
            this.lblThoiGianTraLoi.Size = new System.Drawing.Size(0, 21);
            this.lblThoiGianTraLoi.TabIndex = 9;
            // 
            // lblThele
            // 
            this.lblThele.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThele.Location = new System.Drawing.Point(22, 0);
            this.lblThele.Name = "lblThele";
            this.lblThele.Size = new System.Drawing.Size(982, 53);
            this.lblThele.TabIndex = 72;
            this.lblThele.Text = "Thể lệ phần thi:";
            // 
            // flowPanelSentences
            // 
            this.flowPanelSentences.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowPanelSentences.Location = new System.Drawing.Point(22, 55);
            this.flowPanelSentences.Margin = new System.Windows.Forms.Padding(2);
            this.flowPanelSentences.Name = "flowPanelSentences";
            this.flowPanelSentences.Size = new System.Drawing.Size(980, 356);
            this.flowPanelSentences.TabIndex = 73;
            // 
            // ucThuThach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.flowPanelSentences);
            this.Controls.Add(this.lblThele);
            this.Controls.Add(this.lblThoiGianTraLoi);
            this.Controls.Add(this.lblTGTL);
            this.Controls.Add(this.txtCauTraLoi);
            this.Controls.Add(this.pbDapAn);
            this.Controls.Add(this.pbGui);
            this.Name = "ucThuThach";
            this.Size = new System.Drawing.Size(1004, 530);
            this.Load += new System.EventHandler(this.ucKhamPha_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbGui)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDapAn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tmKhamPha;
        private System.Windows.Forms.PictureBox pbGui;
        private System.Windows.Forms.PictureBox pbDapAn;
        private System.Windows.Forms.TextBox txtCauTraLoi;
        private System.Windows.Forms.Label lblTGTL;
        private System.Windows.Forms.Label lblThoiGianTraLoi;
        private System.Windows.Forms.Label lblThele;
        private System.Windows.Forms.FlowLayoutPanel flowPanelSentences;
    }
}
