using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitemporalVisualization
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            checkpoint = new Checkpoint();
        }

        private double zoom = 1;
        private double offsetX = 0;
        private double offsetY = 0;
        private Checkpoint checkpoint;
        private Version hoverVersion;
        private int mouseX, mouseY;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int tranId = hoverVersion == null ? 0 : hoverVersion.transactionId;
            var coords = new CoordinateTransformer(zoom, offsetX, offsetY);
            checkpoint.Draw(coords, new BrushProvider(tranId), e.Graphics);

            e.Graphics.FillRectangle(new SolidBrush(Color.White), 680, 0, 400, 900 );
            e.Graphics.DrawString(hoverText, new Font("Lucida Console", 10), new SolidBrush(Color.Black), 685, 10);
            var pen = new Pen(new SolidBrush(Color.DarkGray));
            e.Graphics.DrawLine(pen, mouseX, 0, mouseX, 1000);
            e.Graphics.DrawLine(pen, 0, mouseY, 1000, mouseY);
            var now = (int)coords.RecordTimeToY(DateTime.Now);
            e.Graphics.DrawLine(pen, 0, now, 1000, now);
        }

        private string hoverText
        {
            get
            {
                if (hoverVersion != null)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendFormat("Transaction: {0}", hoverVersion.transactionId).AppendLine();
                    sb.AppendFormat("Revision: {0}", hoverVersion.revisionId).AppendLine();
                    sb.AppendFormat("Record:\n from:\t{0}\n   to:\t{1}", PresentableDate(hoverVersion.recordFrom),
                                    PresentableDate(hoverVersion.recordTo)).AppendLine();
                    sb.AppendFormat("Valid:\n from:\t{0}\n   to:\t{1}", PresentableDate(hoverVersion.validFrom),
                                    PresentableDate(hoverVersion.validTo)).AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    var coordTrans = new CoordinateTransformer(zoom, offsetX, offsetY);
                    var validTime = coordTrans.XToValidTime(mouseX);
                    var recordTime = coordTrans.YToRecordTime(mouseY);
                    sb.AppendFormat("Mouse position:\n  Valid:  {0}\n  Record: {1}", PresentableDate(validTime),
                                    PresentableDate(recordTime));
                    return sb.ToString();
                }
                return "";
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oemplus)
                zoom /= 0.9f;
            if (e.KeyCode == Keys.OemMinus)
                zoom *= 0.9f;
            if (e.KeyCode == Keys.Up)
                offsetY += 10f;
            if (e.KeyCode == Keys.Down)
                offsetY -= 10f;
            if (e.KeyCode == Keys.Left)
                offsetX += 10f;
            if (e.KeyCode == Keys.Right)
                offsetX -= 10f;
            Refresh();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            hoverVersion = checkpoint.FindVersion(new CoordinateTransformer(zoom, offsetX, offsetY), e.X, e.Y);
            mouseX = e.X;
            mouseY = e.Y;
            Refresh();
        }

        private String PresentableDate(DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return "Start of Time";
            if (dt == DateTime.MaxValue)
                return "End of Time";
            return dt.ToString();
        }
    }
}
