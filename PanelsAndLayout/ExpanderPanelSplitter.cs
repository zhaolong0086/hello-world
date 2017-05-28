using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfLearning
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfLearning"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfLearning;assembly=WpfLearning"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ExpanderPanelSplitter/>
    ///
    /// </summary>
    public class ExpanderPanelSplitter : Control
    {
        static ExpanderPanelSplitter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpanderPanelSplitter), new FrameworkPropertyMetadata(typeof(ExpanderPanelSplitter)));

			// override the Background property
            BackgroundProperty.OverrideMetadata(typeof(ExpanderPanelSplitter), new FrameworkPropertyMetadata(Brushes.Red));

		}


		#region Private fields
		public FrameworkElement element;     // element to resize (target element)
		private double width;                 // current desired width of the element, can be less than minwidth
		private double height;                // current desired height of the element, can be less than minheight
		private double previousParentWidth;   // current width of parent element, used for proportional resize
		private double previousParentHeight;  // current height of parent element, used for proportional resize
		#endregion

        /// <summary></summary>
        public ExpanderPanelSplitter()
		{
			Loaded += DockPanelSplitterLoaded;
			Unloaded += DockPanelSplitterUnloaded;

			///////////////////////////////////////////////////////////////////////
			//Border bd = new Border();
			//bd.Background = this.Background;
			//bd.BorderBrush = this.BorderBrush;
			//bd.BorderThickness = this.BorderThickness;
			////<!--<Style TargetType="{x:Type local:DockPanelSplitter}">
			////	<Setter Property="Template">
			////		<Setter.Value>
			////			<ControlTemplate TargetType="{x:Type local:DockPanelSplitter}">
			////				<Border Background="{TemplateBinding Background}"
			////						BorderBrush="{TemplateBinding BorderBrush}"
			////						BorderThickness="{TemplateBinding BorderThickness}">
			////				</Border>
			////			</ControlTemplate>
			////		</Setter.Value>
			////	</Setter>
			////</Style>-->

			///////////////////////////////////////
            //UpdateHeightOrWidth();
		}

		void DockPanelSplitterLoaded(object sender, RoutedEventArgs e)
		{
			Panel dp = Parent as Panel;
			if (dp == null) return;

			// Subscribe to the parent's size changed event
			dp.SizeChanged += ParentSizeChanged;

			// Store the current size of the parent DockPanel
			previousParentWidth = dp.ActualWidth;
			previousParentHeight = dp.ActualHeight;

			// Find the target element
			UpdateTargetElement();

		}

		void DockPanelSplitterUnloaded(object sender, RoutedEventArgs e)
		{
			Panel dp = Parent as Panel;
			if (dp == null) return;

			// Unsubscribe
			dp.SizeChanged -= ParentSizeChanged;
		}


		/// <summary>
		/// Update the target element (the element the DockPanelSplitter works on)
		/// </summary>
		private void UpdateTargetElement()
		{
			Panel dp = Parent as Panel;
			if (dp == null) return;

			int i = dp.Children.IndexOf(this);

			// The splitter cannot be the first can of the parent DockPanel
			// The splitter works on the 'older' sibling 
			if (i > 0 && dp.Children.Count > 0)
			{
				element = dp.Children[i - 1] as FrameworkElement;
			}
		}


		private void SetTargetHeight(double newHeight)
		{
			if (newHeight < element.MinHeight)
				newHeight = element.MinHeight;
			if (newHeight > element.MaxHeight)
				newHeight = element.MaxHeight;

            if (element.Height != newHeight)
            {
                Debug.WriteLine("SetTargetHeight From {0} to {1}", element.Height, newHeight);
			    element.Height = newHeight;
            }
		}

		private void ParentSizeChanged(object sender, SizeChangedEventArgs e)
		{
			DockPanel dp = Parent as DockPanel;
			if (dp == null) return;

			double sx = dp.ActualWidth / previousParentWidth;
			double sy = dp.ActualHeight / previousParentHeight;

            //if (!double.IsInfinity(sx))
            //    SetTargetWidth(element.Width * sx);
            //if (!double.IsInfinity(sy))
				SetTargetHeight(element.Height * sy);

			previousParentWidth = dp.ActualWidth;
			previousParentHeight = dp.ActualHeight;

		}

		double AdjustHeight(double dy, Dock dock)
		{
			height += dy;
			SetTargetHeight(height);

			return dy;
		}

		Point StartDragPoint;

        /// <summary></summary>
        protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			if (!IsEnabled)
                return;
            Cursor = Cursors.SizeNS;
		}

        /// <summary></summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (!IsEnabled)
                return;

			if (!IsMouseCaptured)
			{
				StartDragPoint = e.GetPosition(Parent as IInputElement);
				UpdateTargetElement();
				if (element != null)
				{
					width = element.ActualWidth;
					height = element.ActualHeight;
					CaptureMouse();
				}
			}

			base.OnMouseDown(e);
		}

        /// <summary></summary>
        protected override void OnMouseMove(MouseEventArgs e)
		{
			if (IsMouseCaptured)
			{
				Point ptCurrent = e.GetPosition(Parent as IInputElement);
                Debug.WriteLine("OnMouseMove, AdjustHeight, ptCurrent is {0}, startpoint is {1}", ptCurrent, StartDragPoint);
                Point delta = new Point(ptCurrent.X - StartDragPoint.X, ptCurrent.Y - StartDragPoint.Y);
				Dock dock = DockPanel.GetDock(this);

                Debug.WriteLine("OnMouseMove, AdjustHeight, deltaY is {0}", delta.Y);
                delta.Y = AdjustHeight(delta.Y, dock);

                StartDragPoint = new Point(StartDragPoint.X + delta.X, StartDragPoint.Y + delta.Y);
			}

			base.OnMouseMove(e);
		}

        /// <summary></summary>
        protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			if (IsMouseCaptured)
				ReleaseMouseCapture();

            base.OnMouseUp(e);
		}

         /// <summary></summary>
        public delegate void SplitterMoveingDelegate(object sender, double distance);
        /// <summary></summary>
        public event SplitterMoveingDelegate SplitterMoveing;

    }
}
