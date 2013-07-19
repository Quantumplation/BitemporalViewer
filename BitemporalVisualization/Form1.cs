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
            checkpoint = new Checkpoint();
            var mins = checkpoint.MinCorner();
            CoordinateTransformer.start = mins.Item1.recordFrom;
            var tran = new CoordinateTransformer(zoom, offsetX, offsetY);
            if (mins.Item2.validFrom == DateTime.MinValue)
            {
                offsetX = -tran.ValidTimeToX(DateTime.Now);
            }
            else
                offsetX = -tran.ValidTimeToX(mins.Item2.validFrom) + 50;
            offsetY =  tran.RecordTimeToY(mins.Item1.recordFrom) - 500;

            drawingPanel.Focus();
        }

        private double zoom = 30495.460961325756;
        private double offsetX = 0;
        private double offsetY = 0;
        private Checkpoint checkpoint;
        private Version hoverVersion;
        private int mouseX, mouseY;

        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            long tranId = hoverVersion == null ? 0L : hoverVersion.transactionId;
            var coords = new CoordinateTransformer(zoom, offsetX, offsetY);
            checkpoint.Draw(coords, new BrushProvider(tranId), e.Graphics);

            var pen = new Pen(new SolidBrush(Color.DarkGray));
            e.Graphics.DrawLine(pen, mouseX, 0, mouseX, drawingPanel.Height);
            e.Graphics.DrawLine(pen, 0, mouseY, drawingPanel.Width, mouseY);
            var now = (int)coords.RecordTimeToY(DateTime.Now);
            e.Graphics.DrawLine(pen, 0, now, drawingPanel.Width, now);
        }

        private string hoverText
        {
            get
            {
                if (hoverVersion != null)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendFormat("Transaction: {0}", hoverVersion.transactionId).AppendLine();
                    sb.AppendFormat("Revision: {0}", String.Join(", ", hoverVersion.revIds)).AppendLine();
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

        private Point lastMousePos;
        private void drawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                offsetX += e.X - lastMousePos.X;
                offsetY += e.Y - lastMousePos.Y;
            }

            hoverVersion = checkpoint.FindVersion(new CoordinateTransformer(zoom, offsetX, offsetY), e.X, e.Y);
            mouseX = e.X;
            mouseY = e.Y;
            splitContainer.Panel1.Refresh();
            
            hoveTextLabel.Text = hoverText;

            lastMousePos = e.Location;
        }

        private String PresentableDate(DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return "Start of Time";
            if (dt == DateTime.MaxValue)
                return "End of Time";
            return dt.ToString();
        }

        private void drawingPanel_Click(object sender, EventArgs e)
        {
            drawingPanel.Focus();
        }

        private void drawingPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                zoom /= 0.9;
            else
                zoom *= 0.9;

            zoom = Math.Max(1, zoom);

            Refresh();
        }
    }
}
