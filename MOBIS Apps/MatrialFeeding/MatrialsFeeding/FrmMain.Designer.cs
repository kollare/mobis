namespace OMMCDP
{
    partial class frmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlWork = new System.Windows.Forms.Panel();
            this.tlpParts = new System.Windows.Forms.TableLayoutPanel();
            this.lbMsg = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslblTimer = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.btn_qualityAlert = new System.Windows.Forms.Button();
            this.btn_UserLogin = new System.Windows.Forms.Button();
            this.btnPass = new System.Windows.Forms.Button();
            this.btnDataRefresh = new System.Windows.Forms.Button();
            this.btn_WorkInst = new System.Windows.Forms.Button();
            this.btnWorkInstruction = new System.Windows.Forms.Button();
            this.tmrClock = new System.Windows.Forms.Timer(this.components);
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tmrAutoSkip = new System.Windows.Forms.Timer(this.components);
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.pnlWork.SuspendLayout();
            this.tlpParts.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlWork
            // 
            this.pnlWork.Controls.Add(this.tlpParts);
            this.pnlWork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWork.Location = new System.Drawing.Point(0, 92);
            this.pnlWork.Name = "pnlWork";
            this.pnlWork.Size = new System.Drawing.Size(1008, 555);
            this.pnlWork.TabIndex = 1;
            // 
            // tlpParts
            // 
            this.tlpParts.ColumnCount = 6;
            this.tlpParts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.Controls.Add(this.lbMsg, 0, 0);
            this.tlpParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpParts.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpParts.Location = new System.Drawing.Point(0, 0);
            this.tlpParts.Name = "tlpParts";
            this.tlpParts.RowCount = 6;
            this.tlpParts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpParts.Size = new System.Drawing.Size(1008, 555);
            this.tlpParts.TabIndex = 0;
            // 
            // lbMsg
            // 
            this.lbMsg.AutoSize = true;
            this.lbMsg.Location = new System.Drawing.Point(3, 0);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(49, 14);
            this.lbMsg.TabIndex = 0;
            this.lbMsg.Text = "label1";
            this.lbMsg.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PowderBlue;
            this.panel1.Controls.Add(this.btnNextPage);
            this.panel1.Controls.Add(this.btnPrevPage);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.btn_qualityAlert);
            this.panel1.Controls.Add(this.btn_UserLogin);
            this.panel1.Controls.Add(this.btnPass);
            this.panel1.Controls.Add(this.btnDataRefresh);
            this.panel1.Controls.Add(this.btn_WorkInst);
            this.panel1.Controls.Add(this.btnWorkInstruction);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 647);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 83);
            this.panel1.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslblTimer,
            this.tsslblMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 61);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 36;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslblTimer
            // 
            this.tsslblTimer.AutoSize = false;
            this.tsslblTimer.BackColor = System.Drawing.Color.CadetBlue;
            this.tsslblTimer.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsslblTimer.ForeColor = System.Drawing.Color.White;
            this.tsslblTimer.Name = "tsslblTimer";
            this.tsslblTimer.Size = new System.Drawing.Size(80, 17);
            this.tsslblTimer.Text = "10:00:00";
            // 
            // tsslblMessage
            // 
            this.tsslblMessage.AutoSize = false;
            this.tsslblMessage.BackColor = System.Drawing.Color.LightBlue;
            this.tsslblMessage.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsslblMessage.Name = "tsslblMessage";
            this.tsslblMessage.Size = new System.Drawing.Size(913, 17);
            this.tsslblMessage.Spring = true;
            this.tsslblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_qualityAlert
            // 
            this.btn_qualityAlert.BackColor = System.Drawing.Color.LightCyan;
            this.btn_qualityAlert.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_qualityAlert.ForeColor = System.Drawing.Color.Black;
            this.btn_qualityAlert.Location = new System.Drawing.Point(238, 5);
            this.btn_qualityAlert.Name = "btn_qualityAlert";
            this.btn_qualityAlert.Size = new System.Drawing.Size(120, 53);
            this.btn_qualityAlert.TabIndex = 41;
            this.btn_qualityAlert.Text = "Quality Alert";
            this.btn_qualityAlert.UseVisualStyleBackColor = false;
            this.btn_qualityAlert.Click += new System.EventHandler(this.btn_qualityAlert_Click);
            // 
            // btn_UserLogin
            // 
            this.btn_UserLogin.BackColor = System.Drawing.Color.LightCyan;
            this.btn_UserLogin.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_UserLogin.ForeColor = System.Drawing.Color.Black;
            this.btn_UserLogin.Location = new System.Drawing.Point(477, 5);
            this.btn_UserLogin.Name = "btn_UserLogin";
            this.btn_UserLogin.Size = new System.Drawing.Size(120, 53);
            this.btn_UserLogin.TabIndex = 40;
            this.btn_UserLogin.Text = "User Login";
            this.btn_UserLogin.UseVisualStyleBackColor = false;
            this.btn_UserLogin.Click += new System.EventHandler(this.btn_UserLogin_Click);
            // 
            // btnPass
            // 
            this.btnPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPass.BackColor = System.Drawing.Color.LightCyan;
            this.btnPass.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPass.ForeColor = System.Drawing.Color.Black;
            this.btnPass.Location = new System.Drawing.Point(889, 5);
            this.btnPass.Name = "btnPass";
            this.btnPass.Size = new System.Drawing.Size(107, 53);
            this.btnPass.TabIndex = 39;
            this.btnPass.Text = "Pass";
            this.btnPass.UseVisualStyleBackColor = false;
            this.btnPass.Visible = false;
            this.btnPass.Click += new System.EventHandler(this.btnPass_Click);
            // 
            // btnDataRefresh
            // 
            this.btnDataRefresh.BackColor = System.Drawing.Color.LightCyan;
            this.btnDataRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDataRefresh.ForeColor = System.Drawing.Color.Black;
            this.btnDataRefresh.Location = new System.Drawing.Point(364, 5);
            this.btnDataRefresh.Name = "btnDataRefresh";
            this.btnDataRefresh.Size = new System.Drawing.Size(107, 53);
            this.btnDataRefresh.TabIndex = 38;
            this.btnDataRefresh.Text = "Refresh Data";
            this.btnDataRefresh.UseVisualStyleBackColor = false;
            this.btnDataRefresh.Click += new System.EventHandler(this.btnDataRefresh_Click);
            // 
            // btn_WorkInst
            // 
            this.btn_WorkInst.BackColor = System.Drawing.Color.LightCyan;
            this.btn_WorkInst.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_WorkInst.ForeColor = System.Drawing.Color.Black;
            this.btn_WorkInst.Location = new System.Drawing.Point(125, 5);
            this.btn_WorkInst.Name = "btn_WorkInst";
            this.btn_WorkInst.Size = new System.Drawing.Size(107, 53);
            this.btn_WorkInst.TabIndex = 37;
            this.btn_WorkInst.Text = "Work Instruction (English)";
            this.btn_WorkInst.UseVisualStyleBackColor = false;
            this.btn_WorkInst.Click += new System.EventHandler(this.btn_WorkInst_Click_1);
            // 
            // btnWorkInstruction
            // 
            this.btnWorkInstruction.BackColor = System.Drawing.Color.LightCyan;
            this.btnWorkInstruction.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWorkInstruction.ForeColor = System.Drawing.Color.Black;
            this.btnWorkInstruction.Location = new System.Drawing.Point(12, 5);
            this.btnWorkInstruction.Name = "btnWorkInstruction";
            this.btnWorkInstruction.Size = new System.Drawing.Size(107, 53);
            this.btnWorkInstruction.TabIndex = 35;
            this.btnWorkInstruction.Text = "Work Instruction (Spanish)";
            this.btnWorkInstruction.UseVisualStyleBackColor = false;
            this.btnWorkInstruction.Click += new System.EventHandler(this.btnWorkInstruction_Click);
            // 
            // tmrClock
            // 
            this.tmrClock.Enabled = true;
            this.tmrClock.Interval = 1000;
            this.tmrClock.Tick += new System.EventHandler(this.tmrClock_Tick);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(1008, 92);
            this.pnlTitle.TabIndex = 3;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.MidnightBlue;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Consolas", 45F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1008, 92);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Matrial feeding";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.lblTitle_Paint);
            // 
            // tmrAutoSkip
            // 
            this.tmrAutoSkip.Tick += new System.EventHandler(this.tmrAutoSkip_Tick);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.BackColor = System.Drawing.Color.LightCyan;
            this.btnPrevPage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevPage.ForeColor = System.Drawing.Color.Black;
            this.btnPrevPage.Location = new System.Drawing.Point(603, 5);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(120, 53);
            this.btnPrevPage.TabIndex = 42;
            this.btnPrevPage.Text = "Prev. Page";
            this.btnPrevPage.UseVisualStyleBackColor = false;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.BackColor = System.Drawing.Color.LightCyan;
            this.btnNextPage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextPage.ForeColor = System.Drawing.Color.Black;
            this.btnNextPage.Location = new System.Drawing.Point(729, 5);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(120, 53);
            this.btnNextPage.TabIndex = 43;
            this.btnNextPage.Text = "Next Page";
            this.btnNextPage.UseVisualStyleBackColor = false;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.pnlWork);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Matrial feeding";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlWork.ResumeLayout(false);
            this.tlpParts.ResumeLayout(false);
            this.tlpParts.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlWork;
        private System.Windows.Forms.TableLayoutPanel tlpParts;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslblTimer;
        private System.Windows.Forms.Button btnWorkInstruction;
        private System.Windows.Forms.Button btn_WorkInst;
        private System.Windows.Forms.ToolStripStatusLabel tsslblMessage;
        private System.Windows.Forms.Timer tmrClock;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnDataRefresh;
        private System.Windows.Forms.Button btnPass;
        private System.Windows.Forms.Button btn_UserLogin;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.Button btn_qualityAlert;
        private System.Windows.Forms.Timer tmrAutoSkip;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
    }
}

