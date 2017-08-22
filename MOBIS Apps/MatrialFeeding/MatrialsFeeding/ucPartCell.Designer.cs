namespace OMMCDP
{
    partial class ucPartCell
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSeq = new System.Windows.Forms.Label();
            this.lblPart = new System.Windows.Forms.Label();
            this.trpPart = new System.Windows.Forms.TableLayoutPanel();
            this.trpPart.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSeq
            // 
            this.lblSeq.BackColor = System.Drawing.Color.SteelBlue;
            this.lblSeq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSeq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSeq.Font = new System.Drawing.Font("Consolas", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeq.ForeColor = System.Drawing.Color.White;
            this.lblSeq.Location = new System.Drawing.Point(1, 1);
            this.lblSeq.Margin = new System.Windows.Forms.Padding(1);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(315, 89);
            this.lblSeq.TabIndex = 0;
            this.lblSeq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSeq.Paint += new System.Windows.Forms.PaintEventHandler(this.lblSEQ_Paint);
            // 
            // lblPart
            // 
            this.lblPart.BackColor = System.Drawing.Color.Lime;
            this.lblPart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPart.Font = new System.Drawing.Font("Consolas", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPart.ForeColor = System.Drawing.Color.Black;
            this.lblPart.Location = new System.Drawing.Point(1, 92);
            this.lblPart.Margin = new System.Windows.Forms.Padding(1);
            this.lblPart.Name = "lblPart";
            this.lblPart.Size = new System.Drawing.Size(315, 135);
            this.lblPart.TabIndex = 1;
            this.lblPart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPart.Paint += new System.Windows.Forms.PaintEventHandler(this.lblPart_Paint);
            // 
            // trpPart
            // 
            this.trpPart.ColumnCount = 1;
            this.trpPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.trpPart.Controls.Add(this.lblPart, 0, 1);
            this.trpPart.Controls.Add(this.lblSeq, 0, 0);
            this.trpPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trpPart.Location = new System.Drawing.Point(0, 0);
            this.trpPart.Margin = new System.Windows.Forms.Padding(1);
            this.trpPart.Name = "trpPart";
            this.trpPart.RowCount = 2;
            this.trpPart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.trpPart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.trpPart.Size = new System.Drawing.Size(317, 228);
            this.trpPart.TabIndex = 2;
            // 
            // ucPartCell
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.trpPart);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "ucPartCell";
            this.Size = new System.Drawing.Size(317, 228);
            this.trpPart.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblSeq;
        public System.Windows.Forms.Label lblPart;
        public System.Windows.Forms.TableLayoutPanel trpPart;
    }
}
