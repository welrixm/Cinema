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
using Cinema.Components;

namespace Cinema.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistPage.xaml
    /// </summary>
    public partial class RegistPage : Page
    {
        public RegistPage()
        {
            InitializeComponent();
        }

        private void RegistBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTb.Text.Trim();
            string password = PasswordTb.Password.Trim();
            if(login.Length > 0 && password.Length > 0)
            {
                DBConnect.db.User.Add(new User
                {
                    Login = login,
                    Password = password
                }) ;
                DBConnect.db.SaveChanges();
                MessageBox.Show("Вы зарегистрированы!");

            }
            else
            {
                MessageBox.Show("Пусто! Заполните поля.");
            }
        }
    }
}
