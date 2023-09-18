using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace function.wpf
{
    public class WpfFont
    {
        public System.Windows.Media.FontFamily FontFamily { get; set; }

        public double FontSize;

        public FontStyle FontStyle;

        public FontWeight FontWeight;

        System.Drawing.Font font;

        public WpfFont() { }

        public WpfFont(ContentControl e)
        {
            FontFamily = e.FontFamily;
            FontSize = e.FontSize;
            FontStyle = e.FontStyle;
            FontWeight = e.FontWeight;
        }

        public WpfFont(System.Drawing.Font f)
        {
            WpfFont wf = wpfFnc.Font2WpfFont(f);
        }


        public WpfFont(string text)
        {
            SetText(text);
        }

        public void SetText(string text)
        {
            string[] vv = text.Split(new char[] { ';' });

            if (vv.Length == 4)
            {
                FontFamily = new System.Windows.Media.FontFamily(vv[0]);
                FontSize = Fnc.obj2Double(vv[1]);

                switch (vv[2].ToUpper())
                {
                    case "Italic":
                        FontStyle = FontStyles.Italic;
                        break;

                    default:
                        FontStyle = FontStyles.Normal;
                        break;
                }

                switch (vv[3])
                {
                    case "Bold":
                        FontWeight = FontWeights.Bold;
                        break;

                    default:
                        FontWeight = FontWeights.Normal;
                        break;
                }

            }
        }


        public void FontApply(ContentControl e)
        {
            e.FontFamily = FontFamily;
            e.FontSize = FontSize;
            e.FontStyle = FontStyle;
            e.FontWeight = FontWeight;
        }

        public bool FontShowDialog()
        {
            System.Windows.Forms.FontDialog f = new System.Windows.Forms.FontDialog();
            f.Font = wpfFnc.WpfFont2Font(this);
            f.ShowEffects = false;
            if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK) return false;

            WpfFont ff = wpfFnc.Font2WpfFont(f.Font);

            FontFamily = ff.FontFamily;
            FontSize = ff.FontSize;
            FontStyle = ff.FontStyle;
            FontWeight = ff.FontWeight;

            return true;
        }

        public System.Drawing.Font FontGet()
        {
            return wpfFnc.WpfFont2Font(this);
        }



        public override string ToString()
        {
            string rtn = $"{FontFamily.Source};{FontSize};{FontStyle.ToString()};{FontWeight.ToString()}";

            return rtn;
        }

    }

}
