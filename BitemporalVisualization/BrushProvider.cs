using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gradient = System.Tuple<System.Drawing.Color, System.Drawing.Color>;

namespace BitemporalVisualization
{
    public class BrushProvider
    {
        private List<Gradient> gradients; 
        private Dictionary<int, Gradient> gradientMappings;
        private int highlightedTransaction;

        public BrushProvider(int tId)
        {
            highlightedTransaction = tId;
            gradients = new List<Gradient>();
            gradientMappings = new Dictionary<int, Gradient>();
            gradients.Add(new Gradient(Color.FromArgb(214, 54, 201), Color.FromArgb(173, 0, 159)));
            gradients.Add(new Gradient(Color.FromArgb(255, 126, 54), Color.FromArgb(255, 83, 0)));
            gradients.Add(new Gradient(Color.FromArgb(53, 213, 157), Color.FromArgb(0, 171, 111)));
            gradients.Add(new Gradient(Color.FromArgb(214, 250, 63), Color.FromArgb(157, 184, 46)));
        }

        public Brush GetBrush(int transactionId, int revisionId, DateTime recordFrom, DateTime recordTo, DateTime validFrom, DateTime validTo, Rectangle bounds)
        {
            if (bounds.Width == 0 || bounds.Height == 0)
                return new SolidBrush(Color.Black);
            if (!gradientMappings.ContainsKey(revisionId))
                gradientMappings[revisionId] = gradients[revisionId%gradients.Count];

            LinearGradientBrush br = new LinearGradientBrush(bounds, Color.Black, Color.Black, 0, false);
            ColorBlend cb = new ColorBlend();
            var grad = gradientMappings[revisionId];
            if (transactionId == highlightedTransaction)
                grad = Tuple.Create(grad.Item2, grad.Item1);
            cb.Positions = recordTo != DateTime.MaxValue ? new[] { 0, 1f } : new[] { 0, 0.1f, 1f };
            cb.Colors = recordTo != DateTime.MaxValue ? new[] { grad.Item2, grad.Item2 } : new[] { Color.White, Color.White, grad.Item2 };
            br.InterpolationColors = cb;
            // rotate
            br.RotateTransform(90);

            return br;
        }
    }
}
