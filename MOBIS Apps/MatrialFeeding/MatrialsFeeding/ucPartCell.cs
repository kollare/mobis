using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;

namespace OMMCDP
{
    public enum CellStatus
    {
        Ok,
        NG,
        NextWorks,
        Pass,
        CurrentWork
    }
    public partial class ucPartCell : UserControl
    {
        private Point _cellPoint = new Point();

        public Point CellPoint
        {
            get { return _cellPoint; }
            set { _cellPoint = value; }
        }

        private CellStatus _cellStatus = CellStatus.NextWorks;

        public CellStatus Status
        {
            get { return _cellStatus; }
            set 
            { 
                _cellStatus = value;
            }
        }

        private string _displaySeq = string.Empty;

        public string DisplaySeq
        {
            get { return _displaySeq; }
        }
        private string _displayPart = string.Empty;

        public string DisplayPart
        {
            get { return _displayPart; }
        }

        private string _seq = string.Empty;

        public string Seq
        {
            get { return _seq; }
            set 
            { 
                _seq = value.Trim();
                _displaySeq = _seq.Length > 5 ? _seq.Substring(_seq.Length - 5, 5) : _seq;
            }
        }

        private string _part = string.Empty;

        public string Part
        {
            get { return _part; }
            set 
            {
                _part = value.Trim();

                _displayPart = _part.Length > 5 ? _part.Substring(_part.Length - 5, 5) : _part;
            }
        }

        private string _part_CN_New = string.Empty;

        public string Part_CN_New
        {
            get { return _part_CN_New; }
            set { _part_CN_New = value.Trim(); }
        }

        private string _part_CN_Old = string.Empty;

        public string Part_CN_Old
        {
            get { return _part_CN_Old; }
            set { _part_CN_Old = value.Trim(); }
        }

        private int _minPartLength = 5;

        public int MinPartLength
        {
            get { return _minPartLength; }
            set { _minPartLength = value; }
        }
        private int _minSeqLength = 5;

        public int MinSeqLength
        {
            get { return _minSeqLength; }
            set { _minSeqLength = value; }
        }

        private int _workingIndex = -1;

        public int WorkingIndex
        {
            get { return _workingIndex; }
            set { _workingIndex = value; }
        }

        private int _groupHVIndex = 0;

        public int GroupHVIndex
        {
            get { return _groupHVIndex; }
            set { _groupHVIndex = value; }
        }

        private int _groupVINIndex = -2;

        public int GroupVINIndex
        {
            get { return _groupVINIndex; }
            set { _groupVINIndex = value; }
        }

        private static int _currentWorkingCellIndex = -1;

        public static int CurrentWorkingCellIndex
        {
            get { return ucPartCell._currentWorkingCellIndex; }
            set { ucPartCell._currentWorkingCellIndex = value; }
        }

        private static int _currentWorkingHVGroupIndex = -1;

        public static int CurrentWorkingHVGroupIndex
        {
            get { return ucPartCell._currentWorkingHVGroupIndex; }
            set { ucPartCell._currentWorkingHVGroupIndex = value; }
        }

        private static int _currentWorkingVINGroupIndex = -1;

        public static int CurrentWorkingVINGroupIndex
        {
            get { return ucPartCell._currentWorkingVINGroupIndex; }
            set { ucPartCell._currentWorkingVINGroupIndex = value; }
        }

        public ucPartCell(int seqRatio, int partRatio)
        {
            InitializeComponent();

            _seq = "00000";
            _part = "ABCDEF";

            trpPart.RowStyles[0].Height = seqRatio;
            trpPart.RowStyles[1].Height = partRatio;

        }

        public static float FitFontSize(Graphics g, Size z, Font f, string s)
        {
            SizeF p = g.MeasureString(s, f);
            float hRatio = z.Height / p.Height;
            float wRatio = z.Width / p.Width;
            float ratio = Math.Min(hRatio, wRatio);
            return f.Size * ratio;
        }

        private void lblSEQ_Paint(object sender, PaintEventArgs e)
        {
            DrawString((Label)sender, e, _displaySeq, _minSeqLength);
        }

        private void lblPart_Paint(object sender, PaintEventArgs e)
        {
            DrawString((Label)sender, e, _displayPart, _minPartLength);
        }

        public static void DrawString(Control sender, PaintEventArgs e, string data, int iMinLength)
        {
            if (data == "") data = " ";

            using (Font f = (Font)sender.Font.Clone())
            {
                using (StringFormat st = new StringFormat())
                {
                    st.Alignment = StringAlignment.Center;
                    st.LineAlignment = StringAlignment.Center;

                    float fsize = 0;

                    if (iMinLength > 0 && data.Length < iMinLength)
                    {
                        fsize = FitFontSize(e.Graphics, sender.ClientRectangle.Size, f, data.PadRight(iMinLength, 'X'));
                    }
                    else
                    {
                        fsize = FitFontSize(e.Graphics, sender.ClientRectangle.Size, f, data);
                    }

                    using (LinearGradientBrush lgbCell = new LinearGradientBrush(
                                Point.Empty,
                                new PointF(0, sender.Height),
                                Color.FromArgb(Math.Min((int)sender.BackColor.R + 100, 255), Math.Min((int)sender.BackColor.G + 100, 255), Math.Min((int)sender.BackColor.B + 100, 255)), sender.BackColor))
                    {
                        e.Graphics.FillRectangle(lgbCell, new RectangleF(PointF.Empty, sender.Size));
                    }

                    using (Font f2 = new Font(f.FontFamily.Name, fsize, f.Style))
                    {
                        using (SolidBrush b = new SolidBrush(sender.ForeColor))
                        {
                            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                            e.Graphics.DrawString(data, f2, b, sender.ClientRectangle, st);
                        }
                    }
                }
            }
        }
    }
}
