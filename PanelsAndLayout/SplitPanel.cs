using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace WpfLearning
{
    /// <summary>  简单的自定义容器，子元素在垂直方向布满容器，水平方向平局分配容器宽度   </summary>  
    public class SplitPanel : Panel
    {
        public SplitPanel()
        {
            Debug.Print("Split Panel Constructor.\n" + DateTime.Now.ToString());
            Debug.Print(this.Children.Count.ToString());

            this.Initialized += SplitPanel_Initialized;
            this.Loaded += SplitPanel_Loaded;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Debug.Print("Split Panel OnInitialized.\n" + DateTime.Now.ToString());
            Debug.Print(this.Children.Count.ToString());
        }

        void SplitPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.Print("Split Panel Loaded.\n" + DateTime.Now.ToString());
            Debug.Print(this.Children.Count.ToString());
        }

        void SplitPanel_Initialized(object sender, EventArgs e)
        {
            Debug.Print("Split Panel Initialized.\n" + DateTime.Now.ToString());
            Debug.Print(this.Children.Count.ToString());
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Debug.Print("Split Panel MeasureOverride.\n" + DateTime.Now.ToString());
            foreach (UIElement child in InternalChildren)
            {
                Debug.WriteLine("--availableSize: " + availableSize.ToString());
                child.Measure(availableSize);   // 测量子元素期望布局尺寸(child.DesiredSize)  
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Debug.Print("Split Panel ArrangeOverride.\n" + DateTime.Now.ToString());
            
            if (double.IsInfinity(finalSize.Height) || double.IsInfinity(finalSize.Width))
            {
                throw new InvalidOperationException("容器的宽和高必须是确定值");
            }

            if (Children.Count > 0)
            {
                double childAverageWidth = finalSize.Width / Children.Count;
                for (int childIndex = 0; childIndex < InternalChildren.Count; childIndex++)
                {  
                    UIElement ui = InternalChildren[childIndex];
                    // 计算子元素将被安排的布局区域  
                    var rect = new Rect(childIndex * childAverageWidth, 0, childAverageWidth, finalSize.Height);
                    ui.Arrange(rect);
                }
            }

            return base.ArrangeOverride(finalSize);
        }
    }
}
