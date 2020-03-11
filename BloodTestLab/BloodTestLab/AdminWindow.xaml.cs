
using BloodTestLab.adminPages;
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

namespace BloodTestLab
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void AddLabAssistant(object sender, RoutedEventArgs e)
        {
            adminMainFrame.Content = new AddNewUser();
        }

        private void AddTest(object sender, RoutedEventArgs e)
        {
            adminMainFrame.Content = new AddTest();
        }
        private void AddClinic(object sender, RoutedEventArgs e)
        {
            adminMainFrame.Content = new AddClinic();
        }

        private void MenuItem_Logout(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

        private void MenuItem_RemoveClinic(object sender, RoutedEventArgs e)
        {
            adminMainFrame.Content = new removeClinic();
        }
    }
}
