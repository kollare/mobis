namespace OMMCDP
{
    partial class frmWorkInst
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
            this.picQC = new System.Windows.Forms.PictureBox();
            this.btn_Close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picQC)).BeginInit();
            this.SuspendLayout();
            // 
            // picQC
            // 
            this.picQC.Location = new System.Drawing.Point(8, 8);
            this.picQC.Name = "picQC";
            this.picQC.Size = new System.Drawing.Size(1023, 670);
            this.picQC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picQC.TabIndex = 2;
            this.picQC.TabStop = false;
            this.picQC.Click += new System.EventHandler(this.picQC_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.LightCoral;
            this.btn_Close.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Close.Location = new System.Drawing.Point(853, 628);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(168, 40);
            this.btn_Close.TabIndex = 3;
            this.btn_Close.Text = "CLOSE";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Visible = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // frmWorkInst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1023, 670);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.picQC);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmWorkInst";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmWorkInst";
            this.Load += new System.EventHandler(this.FrmWorkInst_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWorkInst_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picQC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picQC;
        private System.Windows.Forms.Button btn_Close;
    }
}