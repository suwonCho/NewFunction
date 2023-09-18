using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace function.wpf
{
    /// <summary>
    /// wucStatusProgress.xaml에 대한 상호 작용 논리
    /// 
    /// </summary>
    public partial class wucStatusProgress : UserControl
    {

        readonly double dTickness = 1.6;

        public static readonly DependencyProperty StatusDefinitionsProperty = DependencyProperty.Register("StatusDefinitions", typeof(StatusDefinitionCollection), typeof(wucStatusProgress),
    new FrameworkPropertyMetadata(new StatusDefinitionCollection()));

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StatusDefinitionCollection StatusDefinitions
        {
            get
            {
                return (StatusDefinitionCollection)GetValue(StatusDefinitionsProperty);
            }
            set
            {
                SetValue(StatusDefinitionsProperty, value);
            }
        }


        /* 변경 이벤트 발생 안함, 컨틀롤이 갱신 될때 반영
        public static readonly DependencyProperty StatusDefinitionsProperty = DependencyProperty.Register("StatusDefinitions", typeof(StatusDefinitionCollection), typeof(wucStatusProgress),
    new FrameworkPropertyMetadata(new StatusDefinitionCollection(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, StatusDefinitionsChangeCallback, StatusDefinitionsCoerceCallback));

        /// <summary>
        /// 원본 값
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StatusDefinitionCollection StatusDefinitions
        {
            get
            {
                return (StatusDefinitionCollection)GetValue(StatusDefinitionsProperty);
            }
            set
            {
                SetValue(StatusDefinitionsProperty, value);                
            }
        }


        public static StatusDefinitionCollection newStatusDefinitionCollection(wucStatusProgress w)
        {
            StatusDefinitionCollection col = new StatusDefinitionCollection();
            col.CollectionChanged += w.StatusDefinitions_CollectionChanged;
            return col;
        }


        private void StatusDefinitions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Init wucStatusProgress...");
            Init();
        }

        static void StatusDefinitionsChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            wucStatusProgress w = o as wucStatusProgress;
            w.Init();
        }

        static object StatusDefinitionsCoerceCallback(DependencyObject o, object value)
        {

            return null;
        }
        */


        /// <summary>
        /// 버튼 개수
        /// </summary>
        public int StatusCount
        {
            get { return StatusDefinitions.Count; }

        }


        event RoutedEventHandler click;
        public event RoutedEventHandler Click
        {
            add { click += value; }
            remove { click -= value; }
        }

        public wucStatusProgress()
        {           

            //Console.WriteLine("Init wucStatusProgress...");
            StatusDefinitions?.Clear();

            InitializeComponent();
            
            Init();           
        }

        /// <summary>
        /// 모든 상태를 초기화 한다.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < StatusDefinitions.Count; i++)
            {
                StatusDefinitions[i].Status = enStatus.None;
            }
        }

        public void Init()
        {
            try
            {
                pnl.Children.Clear();

                if (StatusCount < 1) return;

                //Console.WriteLine("Init Progress...");

                double wd = this.ActualWidth - this.Padding.Left - this.Padding.Right;
                double w = wd / ((StatusCount * 4) + (StatusCount - 1));
                StatusDefinition s;

                for (int i = 0; i < StatusCount; i++)
                {
                    s = StatusDefinitions[i];

                    if (s.Label == null)
                        s.Label = InitLabel();
                    else                    
                        if (s.Label.Parent != null) ((StackPanel)s.Label.Parent).Children.Remove(s.Label);
                    

                    if (s.Triangle == null)
                        s.Triangle = InitTriangle();
                    else
                        if (s.Triangle.Parent != null) ((StackPanel)s.Triangle.Parent).Children.Remove(s.Triangle);

                    pnl.Children.Add(s.Label);

                    if (i < (StatusCount - 1))
                        s.Label.Width = w * 4;
                    else
                        s.Label.Width = (w * 4) - dTickness;

                    if (i < (StatusCount - 1))
                    {
                        pnl.Children.Add(s.Triangle);
                        s.Triangle.Width = w;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[wucStatusProgress Init Error]{ex.ToString()}");
            }

        }


        private Label InitLabel()
        {
            Label lbl = new Label();
            lbl.Content = "";
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            lbl.VerticalContentAlignment = VerticalAlignment.Center;
            lbl.BorderBrush = new SolidColorBrush(Colors.Black);
            lbl.BorderThickness = new Thickness(dTickness);
            lbl.Padding = new Thickness(0);

            return lbl;
        }

        private wucTriangle InitTriangle()
        {
            wucTriangle tri = new wucTriangle();
            tri.Status = enStatus.None;

            return tri;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Init();
        }


    }   //end class



    public class StatusDefinition
    {
        enStatus status = enStatus.None;

        public enStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                enStatus old = status;
                status = value;

                if(Triangle != null) Triangle.Status = status;
                if(Label !=  null) Label.Background = new SolidColorBrush(function.wpf.wpfFnc.StatusBackColorGet(status));
            }
        }


        Label label = null;

        public Label Label
        {
            get { return label; }
            set
            {
                label = value;

                if (label != null)
                {
                    label.Content = StatusText;
                    Label.Background = new SolidColorBrush(function.wpf.wpfFnc.StatusBackColorGet(status));
                }
            }
        }

        wucTriangle triangle;
        public wucTriangle Triangle
        {
            get { return triangle; }
            set
            {
                triangle = value;
                if(triangle != null) triangle.Status = status;
            }
        }

        public StatusDefinition()
        {
            
        }

        public int Index = 0;


        string statusText = "";
        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                if (Label != null)
                    Label.Content = value;
            }
        }
        

        public object Tag
        {
            get; set;
        }

    }

    public class StatusDefinitionCollection : ObservableCollection<StatusDefinition>
    {
        public StatusDefinitionCollection()
        {
            base.CollectionChanged += StatusDefinitionCollection_CollectionChanged;
        }


        event NotifyCollectionChangedEventHandler collectionChanged;

        public new event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { collectionChanged += value; }
            remove { collectionChanged -= value; }
        }

        private void StatusDefinitionCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (collectionChanged != null)
            {
                collectionChanged?.Invoke(sender, e);
                Console.WriteLine("StatusDefinitionCollection_CollectionChanged");
            }
        }
    }

}
