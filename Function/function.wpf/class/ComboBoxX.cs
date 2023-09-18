using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace function.wpf
{
    public class ComboBoxX : ComboBox
    {

        Popup pop = null;
        TextBox txt = null;
        bool isFocus = false;

        event TextChangedEventHandler itemChanged;

        public event TextChangedEventHandler ItemChanged
        {
            add { itemChanged += value; }
            remove { itemChanged -= value; }
        }

        public TextBox EditableTextBox
        {
            get
            {
                if(txt == null)
                {
                    txt = (TextBox)this.Template.FindName("PART_EditableTextBox", this);
                    txt.GotFocus += Txt_GotFocus;
                    txt.LostFocus += Txt_LostFocus;
                    txt.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;

                    pop = (Popup)this.Template.FindName("PART_Popup", this);                    
                }

                return txt;

            }
        }

        private void Txt_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            isFocus = false;
            Console.WriteLine("Lost Focus.....");


            string t = EditableTextBox.Text;

            if (t != string.Empty)
            {
                ItemsAdd(t);
            }
            
            t = string.Empty;

            foreach(object o in Items)
            {
                if (t != string.Empty) t += " || ";
                t += o.ToString();
            }


            EditableTextBox.Text = t;
            //EditableTextBox.SelectAll();
        }

        private void Txt_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            isFocus = true;
            Console.WriteLine("Got Focus.....");

            EditableTextBox.Text = "";
            IsDropDownOpen = true;
        }

        public ComboBoxX() : base()
        {
            IsEditable = true;

            this.GotFocus += ComboBoxX_GotFocus;
        }

        private void ComboBoxX_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (txt != null) return;

            TextBox tb = EditableTextBox;

            Txt_GotFocus(null, null);
        }

        public void ItemsAdd(string item)
        {
            //중복 방지
            //if (!Items.Contains(item))
            //{
            //    Items.Add(item);

            //    Console.WriteLine($"ItemAdd Focus..... {isFocus}");

            //    if (isFocus)
            //        Text = "";
            //    else
            //        Txt_LostFocus(null, null);
            //}


            ComboBoxXTextItem it;

            //추가된 텍스트인지 확인 한다.
            foreach (object i in Items)
            {
                it = i as ComboBoxXTextItem;
                if (it == null) continue;

                if (it.Text == item) return;
            }


            it = new ComboBoxXTextItem(item);
            it.ClickDel += It_ClickDel;
            Items.Add(it);


            if (itemChanged != null) itemChanged.Invoke(this, null);
        }

        private void It_ClickDel(object sender, System.Windows.RoutedEventArgs e)
        {
            ComboBoxXTextItem it = (ComboBoxXTextItem)sender;

            if (Items.Contains(it))
            {
                Items.Remove(it);
                if (itemChanged != null) itemChanged.Invoke(this, null);
                IsDropDownOpen = true;

            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            string t;

            switch (e.Key)
            {
                case Key.Enter:
                    
                    t = EditableTextBox.Text;

                    if (t != string.Empty)
                    {
                        
                        ItemsAdd(t);
                        
                    }
                    IsDropDownOpen = true;
                    e.Handled = true;
                    EditableTextBox.SelectAll();
                    return;
                    


                case Key.Delete:
                case Key.Back:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        Items.Clear();

                        if (itemChanged != null) itemChanged.Invoke(this, null);
                        IsDropDownOpen = true;
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        t = EditableTextBox.Text;
                        if (t == EditableTextBox.SelectedText)
                        {

                            if (Items.Contains(t))
                            {
                                Items.Remove(t);
                                Text = "";

                                IsDropDownOpen = true;
                                e.Handled = true;

                                if (itemChanged != null) itemChanged.Invoke(this, null);
                                return;
                            }
                        }
                    }
                    
                    break;
                    
            }

            base.OnPreviewKeyDown(e);
        }


        


    }   //end class



}
