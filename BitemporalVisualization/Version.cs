using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitemporalVisualization
{
    public class Version
    {
        public DateTime recordFrom, recordTo;
        public DateTime validFrom, validTo;

        public int transactionId, revisionId;

        public Version(int tranId, int revId, DateTime rf, DateTime rt, DateTime vf, DateTime vt)
        {
            recordFrom = rf;
            recordTo = rt;
            validFrom = vf;
            validTo = vt;
            transactionId = tranId;
            revisionId = revId;
        }

        public void Draw(CoordinateTransformer coordinateSystem, BrushProvider brushProvider, Graphics graphicsObj)
        {
            var bounds = new Rectangle();
            bounds.X = (int)coordinateSystem.ValidTimeToX(validFrom);
            bounds.Y = (int)coordinateSystem.RecordTimeToY(recordTo);
            bounds.Width = (int)coordinateSystem.ValidTimeToX(validTo) - bounds.X;
            bounds.Height = (int)coordinateSystem.RecordTimeToY(recordFrom) - bounds.Y;

            if (bounds.Width == 0 || bounds.Height == 0)
                return;

            var brush = brushProvider.GetBrush(transactionId, revisionId, recordFrom, recordTo, validFrom, validTo, new Rectangle(bounds.Y, bounds.X, bounds.Height, bounds.Width));
            var pen = new Pen(Color.Black, 1);
            graphicsObj.FillRectangle(brush, bounds);
            graphicsObj.DrawRectangle(pen, bounds);
            var text = String.Format("T:{0} / R:{1}", transactionId, revisionId);
            var font = new Font("Lucida Console", 10f);
            var textBrush = new SolidBrush(Color.Black);
            var height = graphicsObj.MeasureString(text, font).Height;
            if (validFrom == DateTime.MinValue)
            {
                bounds.X += 10;
            }

            if(coordinateSystem.fontVisible)
                graphicsObj.DrawString(text, font, textBrush, bounds.X, bounds.Bottom - height);
            brush.Dispose();
        }

    }
}
