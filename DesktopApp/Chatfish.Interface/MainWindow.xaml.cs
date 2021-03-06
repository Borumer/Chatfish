﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace Chatfish.Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);
        private BooleanToVisibilityConverter hiddenOrVisible;

        public MainWindow()
        {
            InitializeComponent();
            AttachConsole(-1);
            hiddenOrVisible = new BooleanToVisibilityConverter();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(ChatList.ItemsSource);
            // Apply the filter
            cv.Filter = item =>
            {
                XmlElement readableElement = item as XmlElement;
                String name = readableElement.GetAttribute("Name");
                return name.Contains(((TextBox)sender).Text);
            };
        }

        private void SearchButton_Click(object sender, MouseButtonEventArgs e)
        {
            bool searchBoxIsVisible = MessageSearchBox.Visibility == Visibility.Visible;
            Visibility searchBoxVisibility = (Visibility)hiddenOrVisible.Convert(!searchBoxIsVisible, typeof(Visibility), null, null);
            MessageSearchBox.Visibility = searchBoxVisibility;
            ChatName.Visibility = searchBoxIsVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private void SearchButton_Hover(object sender, RoutedEventArgs e)
        {
            ((Canvas)sender).Cursor = Cursors.Hand;
            foreach (Shape item in ((Canvas)sender).Children)
            {
                item.Stroke = Brushes.Black;
            }
        }

        private void SearchButton_Leave(object sender, RoutedEventArgs e)
        {
            foreach (Shape item in ((Canvas)sender).Children)
            {
                item.Stroke = Brushes.Gray;
            }
        }
    }

    public class Person
    {
        public String Name { get; set; }
        public int Age { get; set; }
        public String Department { get; set; }

        public static Person InstantiateFromXML(string input)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Person));

            using (StringReader sr = new StringReader(input))
            {
                return (Person)ser.Deserialize(sr);
            }
        }

        public override String ToString() => $"Name: {Name}\nAge: {Age}\nDepartment: {Department}";
    }

}
