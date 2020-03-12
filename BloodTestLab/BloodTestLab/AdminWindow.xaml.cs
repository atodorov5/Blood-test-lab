
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

        private void MenuItem_AddLabAssistant(object sender, RoutedEventArgs e)
        {
            adminMainFrame.Content = new AddNewUser();
        }

        private void MenuItem_AddTest(object sender, RoutedEventArgs e)
        {
            adminMainFrame.Content = new AddTest();
        }
        private void MenuItem_AddClinic(object sender, RoutedEventArgs e)
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

        private void MenuItem_RemoveTest(object sender, RoutedEventArgs e)
        {
            adminMainFrame.Content = new RemoveTest();
        }

        private void MenuItem_UpdateClinic(object sender, RoutedEventArgs e)
        {
            adminMainFrame.Content = new UpdateClinic();
        }
        private void MenuItem_UpdateTestType(object sender, RoutedEventArgs e)
        {
            adminMainFrame.Content = new UpdateTestType();
        }
    }
}
