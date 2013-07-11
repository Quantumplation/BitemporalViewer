using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitemporalVisualization
{
    public class CoordinateTransformer
    {
        private double zoom = 1.0;
        private double originX, originY;
        private double offsetX, offsetY;
        private double windowWidth, windowHeight;
        private double scaleX, scaleY;
        private DateTime originRecord, originValid;

        public static DateTime start;
        static CoordinateTransformer()
        {
        }

        public CoordinateTransformer(double z, double oX, double oY)
        {
            zoom = z;
            windowWidth = 1200;
            windowHeight = 700;
            offsetX = oX;
            offsetY = oY;
            originRecord = start;
            originValid = start;
            originX = -1;
            originY = 500;
            scaleX = (windowWidth * zoom)/(new DateTime(2014, 1, 1).Ticks - originValid.Ticks);
            scaleY = (windowHeight * zoom)/(new DateTime(2014, 1, 1).Ticks - originRecord.Ticks);
        }

        public double RecordTimeToY(DateTime time)
        {
            if (time == DateTime.MinValue)
                return originY;
            if (time == DateTime.MaxValue)
                return originY - windowHeight;
            return (originY + offsetY) - (time.Ticks - originValid.Ticks) * scaleY;
        }

        public double ValidTimeToX(DateTime time)
        {
            if (time == DateTime.MinValue)
                return originX;
            if (time == DateTime.MaxValue)
                return windowWidth;
            return (time.Ticks - originRecord.Ticks) * scaleX + (originX + offsetX);
        }

        public DateTime YToRecordTime(double Y)
        {
            return new DateTime((long)(((originY + offsetY) - Y)/scaleY + originValid.Ticks));
        }

        public DateTime XToValidTime(double X)
        {
            return new DateTime((long)((X - (originX + offsetX)) / scaleX + originRecord.Ticks));
        }

        public bool fontVisible
        {
            get { return zoom >= 0.80f; }
        }
    }
}
