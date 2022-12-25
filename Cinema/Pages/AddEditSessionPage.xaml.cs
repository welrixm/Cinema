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
    /// Логика взаимодействия для AddEditSessionPage.xaml
    /// </summary>
    public partial class AddEditSessionPage : Page
    {
        Components.Session session;

        public AddEditSessionPage(Components.Session _session)
        {
            InitializeComponent();
            session = _session;
            DataContext = session;
            FilmCbx.ItemsSource = DBConnect.db.Movie.ToList();
            FilmCbx.DisplayMemberPath = "Name";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (session.Id == 0 )
            {
                DBConnect.db.Session.Add(session);

            }
            //try
            //{
            DBConnect.db.SaveChanges();
            MessageBox.Show("Успешно сохранено!");
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
        }
    }
}
