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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace function.wpf
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : baseWindow
    {
        Transform2 tr;
        windowSetScale ss;

        public MainWindow()
        {
            InitializeComponent();

            Label l = new Label();
            l.Background = Brushes.AliceBlue;

            tr = new Transform2((Window)this, grd);

            btn1.ContextMenu = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] fld = wpfFnc.FolderSelect();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tr.canDrag = true;
            tr.canScale = true;
            tr.canRotate = false;
            tr.showPosition = true;

            tr.Element_Add(lbl1);
            tr.Element_Add(lbl2);

            tr.Element_Add(btn1);
            tr.Element_Add(btn2);

            //ss = new windowSetScale(this);

            img.MediaEnded += Img_MediaEnded;

            //Uri uri = new Uri("please_wait.gif", UriKind.RelativeOrAbsolute);

            //this.img.Source = uri;
            //this.img.Play();



            string strPathGif = string.Format("file:\\{0}\\please_wait.GIF", Environment.CurrentDirectory);

            //string strPathGif = "pack://application:,,,/please_wait.gif";
            img.Source = new Uri(strPathGif);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.None;
            bitmap.UriSource = new Uri(strPathGif);
            bitmap.EndInit();
            Img_GifAnimation.ImageSource = bitmap;

        }

        private void Img_MediaEnded(object sender, RoutedEventArgs e)
        {
            img.Position = new TimeSpan(0, 0, 1);

            img.Play();
        }
    

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            //winPlzWait f = new winPlzWait(grd);

            //f.Show();

            return;

            MessageBox.Show(this, @"'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\UIAutomationTypes\v4.0_4.0.0.0__31bf3856ad364e35\UIAutomationTypes.dll'을(를) 로드했습니다. PDB 파일을 찾거나 열 수 없습니다.
'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'C:\Users\CSW\AppData\Local\Temp\VisualStudio.XamlDiagnostics.17204\Microsoft.VisualStudio.DesignTools.WpfTap.dll'을(를) 로드했습니다.PDB 파일을 찾거나 열 수 없습니다.
'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\System.Runtime.Serialization\v4.0_4.0.0.0__b77a5c561934e089\System.Runtime.Serialization.dll'을(를) 로드했습니다.PDB 파일을 찾거나 열 수 없습니다.
'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\SMDiagnostics\v4.0_4.0.0.0__b77a5c561934e089\SMDiagnostics.dll'을(를) 로드했습니다.PDB 파일을 찾거나 열 수 없습니다.
'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\System.ServiceModel.Internals\v4.0_4.0.0.0__31bf3856ad364e35\System.ServiceModel.Internals.dll'을(를) 로드했습니다.PDB 파일을 찾거나 열 수 없습니다.
'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\System.Runtime.Serialization.resources\v4.0_4.0.0.0_ko_b77a5c561934e089\System.Runtime.Serialization.resources.dll'을(를) 로드했습니다.모듈이 기호 없이 빌드되었습니다.
'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\System.Windows.Forms\v4.0_4.0.0.0__b77a5c561934e089\System.Windows.Forms.dll'을(를) 로드했습니다.PDB 파일을 찾거나 열 수 없습니다.
'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\System.Drawing\v4.0_4.0.0.0__b03f5f7f11d50a3a\System.Drawing.dll'을(를) 로드했습니다.PDB 파일을 찾거나 열 수 없습니다.
[pt]694, 446 / [pt2]520.337394564199, 222.013274336283
'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'd:\_TASK\_FUNCTION\NewFunction\Function\function.wpf\bin\Debug\Microsoft.WindowsAPICodePack.Shell.dll'을(를) 로드했습니다.PDB 파일을 찾거나 열 수 없습니다.
'function.wpf.exe'(CLR v4.0.30319: function.wpf.exe): 'd:\_TASK\_FUNCTION\NewFunction\Function\function.wpf\bin\Debug\Microsoft.WindowsAPICodePack.dll'을(를) 로드했습니다.PDB 파일을 찾거나 열 수 없습니다.", "Error", MessageBoxButton.YesNoCancel);
        }


        DispatcherTimer tmr = null;

        private void BCol_Click(object sender, RoutedEventArgs e)
        {
            if (tmr != null)
            {
                tmr.Stop();
                tmr = null;
            }
            else
            {                
                tmr = new DispatcherTimer();
                tmr.Interval = TimeSpan.FromMilliseconds(500);
                tmr.Tick += Tmr_Tick;
                tmr.Start();
            }            
        }

        private void Tmr_Tick(object sender, EventArgs e)
        {
            //tCol.Color();
        }


        int progIdx = 0;
        private void btnProg_Click(object sender, RoutedEventArgs e)
        {   

            return;

            if(progIdx >= Prog.StatusDefinitions.Count)
            {
                for(int i=0;i< Prog.StatusDefinitions.Count;i++)
                {
                    Prog.StatusDefinitions[i].Status = enStatus.None;
                }

                progIdx = 0;
            }

            Prog.StatusDefinitions[progIdx].Status = enStatus.OK;


            progIdx++;

        }

        private void btnInpChk_Click(object sender, RoutedEventArgs e)
        {
            inpChk.ShowValue = !inpChk.ShowValue;
        }
    }
}
