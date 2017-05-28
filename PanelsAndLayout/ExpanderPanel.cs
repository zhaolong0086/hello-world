using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfLearning
{
    public class ExpanderPanelItemTag : IDisposable
    {
        public ExpanderPanelItemTag(Expander ep)
        {
            splitter = new ExpanderPanelSplitter();
            splitter.element = ep;
        }

        private double height;

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        private ExpanderPanelSplitter splitter;

        public ExpanderPanelSplitter Splitter
        {
            get { return splitter; }
        }


        public void Dispose()
        {
            splitter = null;
        }
    }

    public class ExpanderPanel : Panel
    {
        public ExpanderPanel()
        {
            Debug.Print("Expander Panel Constructor.\n" + DateTime.Now.ToString());
            Debug.Print(this.InternalChildren.Count.ToString());

            this.Initialized += ExpanderPanel_Initialized;
            this.Loaded += ExpanderPanel_Loaded;
        }



        public static bool GetExpanderSizeChangeable(DependencyObject obj)
        {
            return (bool)obj.GetValue(ExpanderSizeChangeableProperty);
        }

        public static void SetExpanderSizeChangeable(DependencyObject obj, bool value)
        {
            obj.SetValue(ExpanderSizeChangeableProperty, value);
        }

        // Using a DependencyProperty as the backing store for ExpanderSizeChangeable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExpanderSizeChangeableProperty =
            DependencyProperty.RegisterAttached("ExpanderSizeChangeable", typeof(bool), typeof(ExpanderPanel), new PropertyMetadata(false));



        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Debug.Print("Expander Panel OnInitialized.\n" + DateTime.Now.ToString());
            Debug.Print(this.InternalChildren.Count.ToString());
        }

        void ExpanderPanel_Loaded(object sender, RoutedEventArgs e)
        {
            // 先为所有 Expander 添加 Tag。
            for (int i = 0; i < InternalChildren.Count; i++)
            {
                Expander ep = InternalChildren[i] as Expander;
                if(ep==null)
                    Debug.Assert(false, "ExpanderPanel 中只接受 Expander 对象。");

                if (ep.Tag != null && !(ep.Tag is ExpanderPanelItemTag))
                    Debug.Assert(false, "ExpanderPanel 中 Expander 不可以设置 Tag。");

                ep.Expanded += ItemExpanded;
                ep.Collapsed += ItemCollapsed;
                ExpanderPanelItemTag ept = new ExpanderPanelItemTag(ep);
                ep.Tag = ept;
            }

            // 将所有展开的 Expander 后面的 Spliter 打开。
            for (int i = InternalChildren.Count-1; i >=0; i--)
            {
                Expander ep = InternalChildren[i] as Expander;

                if (ep.IsExpanded)
                    InternalChildren.Insert(i + 1, (ep.Tag as ExpanderPanelItemTag).Splitter);
            }

                Debug.Print("Expander Panel Loaded.\n" + DateTime.Now.ToString());
            Debug.Print(this.InternalChildren.Count.ToString());
        }

        void ItemExpanded(object sender, RoutedEventArgs e)
        {
            Expander ep = sender as Expander;

            int idx = InternalChildren.IndexOf(ep);

            if (idx == InternalChildren.Count - 1)  // 最后一个对象后面不需要 Splitter。
                return;

            ExpanderPanelSplitter splitter = (ep.Tag as ExpanderPanelItemTag).Splitter;
            if (InternalChildren.IndexOf(splitter) != -1)   // 如果已经可见 -- 不应该出现的现象。
                return;

            InternalChildren.Insert(idx + 1, splitter);
        }

        void ItemCollapsed(object sender, RoutedEventArgs e)
        {
            Expander ep = sender as Expander;

            int idx = InternalChildren.IndexOf(ep);

            if (idx < InternalChildren.Count - 1)
            {
                if (InternalChildren[idx + 1] is ExpanderPanelSplitter)
                    InternalChildren.RemoveAt(idx + 1);
            }

            //Size sz = new Size(0, 0);
            //string str = ep.DesiredSize.ToString();
            //ep.Measure(sz);
            //str = ep.DesiredSize.ToString();
            ////str = (ep.Header as UIElement).DesiredSize.ToString();

            ep.Height = double.NaN;
        }

        void ReArrangement()
        {
        }

        void ExpanderPanel_Initialized(object sender, EventArgs e)
        {
            Debug.Print("Expander Panel Initialized.\n" + DateTime.Now.ToString());
            Debug.Print(this.InternalChildren.Count.ToString());
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Debug.Print("Expander Panel MeasureOverride.\n" + DateTime.Now.ToString());
            //// 计算当前所有元素的总高度。
            //double totalActualHeight = 0;
            //foreach (FrameworkElement child in InternalChildren.OfType<FrameworkElement>())
            //    totalActualHeight += child.ActualHeight;

            //if (availableSize.Height > totalActualHeight)
            //{
            //    foreach (FrameworkElement child in InternalChildren.OfType<FrameworkElement>())
            //    {
            //        Debug.WriteLine("--availableSize > totalActualHeight: " + availableSize.ToString());
            //        //child.MaxHeight = double.PositiveInfinity;

            //        child.Measure(availableSize);   // 测量子元素期望布局尺寸(child.DesiredSize)  
            //    }
            //}
            //else
            {
                foreach (UIElement child in InternalChildren)
                {
                    Debug.WriteLine("--availableSize: " + availableSize.ToString());
                    child.Measure(availableSize);   // 测量子元素期望布局尺寸(child.DesiredSize)  
                }
            }

            return base.MeasureOverride(availableSize);
        }

        protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            base.OnChildDesiredSizeChanged(child);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Debug.Print("Expander Panel ArrangeOverride.\n" + DateTime.Now.ToString());
            Debug.Print("--ActualSize({0},{1}).\n", ActualWidth, ActualHeight);
            Debug.Print("--finalSize({0}).\n", finalSize);
            
            if (double.IsInfinity(finalSize.Height) || double.IsInfinity(finalSize.Width))
            {
                throw new InvalidOperationException("容器的宽和高必须是确定值");
            }

            if (InternalChildren.Count > 0)
            {
                CountItemSize();
                if (TotalExpandedItemCount == 0)
                    ArrangeWithoutAdjustment(finalSize);
                else
                {
                    if (TotalDesiredHeight > finalSize.Height)
                    {
                        double rate = (finalSize.Height - TotalNonExpandedItemDesiredHeight) / TotalExpandedItemDesiredHeight;
                        double top = 0;
                        for (int i = 0; i < InternalChildren.Count; i++)
                        {
                            FrameworkElement ui = InternalChildren[i] as FrameworkElement;
                            Expander ep = ui as Expander;
                            if (ep != null && ep.IsExpanded)
                            {
                                double h = ui.DesiredSize.Height * rate;
                                var rect = new Rect(0, top, finalSize.Width, h);
                                ui.Arrange(rect);
                                ui.MaxHeight = h;
                                top += h;
                                ui.InvalidateVisual();
                            }
                            else
                            {
                                var rect = new Rect(0, top, finalSize.Width, ui.DesiredSize.Height);
                                ui.Arrange(rect);
                                //ui.Height = h;
                                top += ui.DesiredSize.Height;
                                ui.InvalidateVisual();
                            }
                        }
                    }
                    else if (TotalDesiredHeight < finalSize.Height)
                    {
                        double rate = (finalSize.Height - TotalNonExpandedItemDesiredHeight) / TotalExpandedItemDesiredHeight;
                        double top = 0;
                        for (int i = 0; i < InternalChildren.Count; i++)
                        {
                            FrameworkElement ui = InternalChildren[i] as FrameworkElement;
                            Expander ep = ui as Expander;
                            if (ep != null && ep.IsExpanded)
                            {
                                double h = ui.DesiredSize.Height * rate;
                                var rect = new Rect(0, top, finalSize.Width, h);
                                ui.Arrange(rect);
                                ui.MaxHeight = h;
                                top += h;
                                ui.InvalidateVisual();
                            }
                            else
                            {
                                var rect = new Rect(0, top, finalSize.Width, ui.DesiredSize.Height);
                                ui.Arrange(rect);
                                //ui.Height = h;
                                top += ui.DesiredSize.Height;
                                ui.InvalidateVisual();
                            }
                        }

                        //// 尝试把所有增加的空间都给最后一个可以伸缩的对象；
                        //for (int i = InternalChildren.Count - 1; i >= 0; i--)
                        //{
                        //    Expander ep = InternalChildren[i] as Expander;

                        //    if (ep != null)
                        //    {
                        //        double h = ep.DesiredSize.Height;
                        //        Size sz = new Size(finalSize.Width, h + finalSize.Height - TotalDesiredHeight);
                        //        ep.Height = double.NaN;
                        //        ep.Measure(sz);
                        //        if (ep.DesiredSize.Height == sz.Height)
                        //        {
                        //            CountItemSize();
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            ep.Height = h;
                        //            ep.Measure(sz);
                        //        }
                        //    }
                        //}
                        //ArrangeWithoutAdjustment(finalSize);


                        //double rate = (finalSize.Height - TotalNonExpandedItemDesiredHeight) / TotalExpandedItemDesiredHeight;
                        //double top = 0;
                        //for (int i = 0; i < InternalChildren.Count; i++)
                        //{
                        //    FrameworkElement ui = InternalChildren[i] as FrameworkElement;
                        //    Expander ep = ui as Expander;
                        //    if (ep != null && ep.IsExpanded)
                        //    {
                        //        double h = ui.DesiredSize.Height * rate;
                        //        var rect = new Rect(0, top, finalSize.Width, h);
                        //        ui.Arrange(rect);
                        //        ui.Height = h;
                        //        top += h;
                        //        ui.InvalidateVisual();
                        //    }
                        //    else
                        //    {
                        //        var rect = new Rect(0, top, finalSize.Width, ui.DesiredSize.Height);
                        //        ui.Arrange(rect);
                        //        //ui.Height = h;
                        //        top += ui.DesiredSize.Height;
                        //        ui.InvalidateVisual();
                        //    }
                        //}
                    }
                    else
                        ArrangeWithoutAdjustment(finalSize);
                }


                //double childAverageHeight = finalSize.Height / InternalChildren.Count;
                //for (int childIndex = 0; childIndex < InternalChildren.Count; childIndex++)
                //{  
                //    UIElement ui = InternalChildren[childIndex];
                //    // 计算子元素将被安排的布局区域  
                //    var rect = new Rect(0, childIndex * childAverageHeight, finalSize.Width, childAverageHeight);
                //    ui.Arrange(rect);
                //    ui.UpdateLayout();
                //}
            }

            return base.ArrangeOverride(finalSize);
        }

        void ArrangeWithoutAdjustment(Size finalSize)
        {
            double top = 0;
            for (int i = 0; i < InternalChildren.Count; i++)
            {
                UIElement ui = InternalChildren[i];
                // 计算子元素将被安排的布局区域  
                var rect = new Rect(0, top, finalSize.Width, ui.DesiredSize.Height);
                ui.Arrange(rect);
                top += ui.DesiredSize.Height;
                //ui.UpdateLayout();
                //ui.InvalidateVisual();
            }
        }

        double TotalDesiredHeight;
        double TotalExpandedItemDesiredHeight;
        double TotalExpandedItemCount;
        double TotalNonExpandedItemDesiredHeight;   // include visible splitters

        void CountItemSize()
        {
            TotalDesiredHeight = 0;
            TotalExpandedItemDesiredHeight = 0;
            TotalNonExpandedItemDesiredHeight = 0;   // include visible splitters
            TotalExpandedItemCount = 0;
            for (int i = 0; i < InternalChildren.Count; i++)
            {
                Expander ep = InternalChildren[i] as Expander;
                if (ep != null && ep.IsExpanded)
                {
                    TotalExpandedItemDesiredHeight += ep.DesiredSize.Height;
                    TotalExpandedItemCount++;
                }
                else
                    TotalNonExpandedItemDesiredHeight += InternalChildren[i].DesiredSize.Height;

                TotalDesiredHeight += InternalChildren[i].DesiredSize.Height;
            }
        }


        bool HasExpandedItem
        {
            get
            {
                for(int i=0; i < InternalChildren.Count;i++)
                {
                    Expander ep = InternalChildren[i] as Expander;
                    if (ep != null && ep.IsExpanded)
                        return true;
                }

                return false;
            }
        }
   }
}
