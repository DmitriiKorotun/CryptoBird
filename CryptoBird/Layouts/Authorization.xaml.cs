﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CryptoBird.Layouts
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window, ICloseable
    {
        public Authorization()
        {
            InitializeComponent();
        }

        //private void ToggleHeaderClick(object sender, RoutedEventArgs e)
        //{
        //    if (((FrameworkElement)sender).DataContext is TabControlViewModel tabControlVM)
        //    {
        //        tabControlVM.TabHeaderVisible = !tabControlVM.TabHeaderVisible;
        //    }
        //}

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password.ToString(); }
        }

        private void Lol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //e.Handled = true;
            //Lol.SelectedItem = Lol.Items[1];
        }
    }

    //public class TabControlViewModel : INotifyPropertyChanged
    //{
    //    private bool _tabHeaderVisible = true;

    //    public ICommand ToggleHeader
    //    {
    //        get; private set;
    //    }

    //    public bool TabHeaderVisible
    //    {
    //        get { return _tabHeaderVisible; }
    //        set
    //        {
    //            _tabHeaderVisible = value;
    //            OnPropertyChanged("TabHeaderVisible");
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private void OnPropertyChanged(string name)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    //    }
    //}
}
