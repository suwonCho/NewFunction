using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace function.wpf
{
    /// <summary>
    /// winElementSize2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class winElementSize2 : Window
    {

        public function.wpf.windowSetScale scale; 

        public winElementSize2()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //scale = new windowSetScale(this);
            wuiSizable s = new wuiSizable(rSize);

            wuiSizable s2 = new wuiSizable(rSize2);

            s2.PositionSet(155, 56);
            s2.SizeSet(100, 100);
        }
    }
}
