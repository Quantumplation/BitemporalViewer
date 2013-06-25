using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitemporalVisualization
{
    public class Checkpoint
    {
        private Dictionary<int, Version> transactions;

        public Checkpoint()
        {
            transactions = new Dictionary<int, Version>();

            AddVersion(new Version(1, 1, SampleTimes.A, SampleTimes.C, SampleTimes.Start, SampleTimes.End));
            AddVersion(new Version(2, 1, SampleTimes.C, SampleTimes.E, SampleTimes.B, SampleTimes.C));
            AddVersion(new Version(3, 2, SampleTimes.C, SampleTimes.D, SampleTimes.C, SampleTimes.End));
            AddVersion(new Version(6, 2, SampleTimes.D, SampleTimes.F, SampleTimes.C, SampleTimes.D));
            AddVersion(new Version(7, 1, SampleTimes.D, SampleTimes.End, SampleTimes.D, SampleTimes.End));
            AddVersion(new Version(4, 1, SampleTimes.E, SampleTimes.End, SampleTimes.B, SampleTimes.B.AddDays(10)));
            AddVersion(new Version(5, 3, SampleTimes.E, SampleTimes.F, SampleTimes.B.AddDays(10), SampleTimes.C));
            AddVersion(new Version(8, 4, SampleTimes.F, SampleTimes.End, SampleTimes.B.AddDays(10), SampleTimes.D));
        }

        public Version FindVersion(CoordinateTransformer coords, int x, int y)
        {
            var validTime = coords.XToValidTime(x);
            var recordTime = coords.YToRecordTime(y);
            return transactions.SingleOrDefault(kvp => kvp.Value.recordFrom <= recordTime &&
                                                kvp.Value.recordTo >= recordTime &&
                                                kvp.Value.validFrom <= validTime &&
                                                kvp.Value.validTo >= validTime).Value;
        }

        public void AddVersion(Version v)
        {
            transactions.Add(v.transactionId, v);
        }

        public void Draw(CoordinateTransformer coordinateSystem, BrushProvider brushProvider, Graphics graphicsObj)
        {
            foreach (var version in transactions.Values)
            {
                version.Draw(coordinateSystem, brushProvider, graphicsObj);
            }
        }
    }
}
