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

namespace BloodTestLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String salt = CreateSalt(10);
            String hashedpass = GenerateSHA256Hash(password.Password.ToString(), "top");
            
            if (username.Text == "nasko" && GenerateSHA256Hash(password.Password.ToString(), "top") == hashedpass)
            {
                UserWindow userW = new UserWindow();
                userW.Show();
                this.Close();
            }
            else if(username.Text=="admin" && password.Password.ToString() == "admin")
            {
                AdminWindow adminW = new AdminWindow();
                adminW.Show();
                this.Close();
            }
            else
                loginError.Content = "Wrong username or password!";
        }

        public String CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public String GenerateSHA256Hash(String input,String salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA256Managed sha256hashsring = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256hashsring.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

    
    }
}
