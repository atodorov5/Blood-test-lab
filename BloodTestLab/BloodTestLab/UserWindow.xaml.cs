
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

        private void MenuItem_MakeTest(object sender, RoutedEventArgs e){
        userMainFrame.Content = new makeTest();
    }

        private void MenuItem_Logout(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

        private void MenuItem_Checktest(object sender, RoutedEventArgs e)
        {
            userMainFrame.Content = new checkTest();
        }

        private void MenuItem_ProfileInfo(object sender, RoutedEventArgs e)
        {
            userMainFrame.Content = new ProfileInfo();
        }

        private void MenuItem_Donation(object sender, RoutedEventArgs e)
        {
            userMainFrame.Content = new Donation();
        }

        private void MenuItem_DonationRef(object sender, RoutedEventArgs e)
        {
            userMainFrame.Content = new donationrReference();
        }
    }
}
