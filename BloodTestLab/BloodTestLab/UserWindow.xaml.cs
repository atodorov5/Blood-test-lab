
using BloodTestLab.userPages;
using System.Windows;

namespace BloodTestLab
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {

        public UserWindow()
        {
            InitializeComponent();
        }

        private void menuMakeTest(object sender, RoutedEventArgs e){
        userMainFrame.Content = new makeTest();
    }

        private void MenuItem_Logout(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }
    }

 


}
