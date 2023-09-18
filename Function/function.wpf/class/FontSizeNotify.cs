using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace function.wpf
{
    public class FontSizeNotify
    {
        private double _fontSize;

        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;

                TextBlock tb = new TextBlock();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        

    }   //end class
}
