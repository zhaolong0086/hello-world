using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfLearning
{
    class DemoControl : Control
    {
        protected override void OnRender(DrawingContext drawingcontext)
        {
            base.OnRender(drawingcontext);
            drawingcontext.DrawRectangle(Brush, null, new Rect(new Point(), RenderSize));
        }

        private Brush Brush;

        public void SetBrush(Brush brush)
        {
            Brush = brush;
            this.InvalidateVisual();
        }



        protected override Size ArrangeOverride(Size arrangebounds)
        {
            base.ArrangeOverride(arrangebounds);
            return new Size(100, 100);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);
            return new Size(50, 50);
        }
    }
}
