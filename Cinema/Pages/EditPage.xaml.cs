using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using System.IO;

namespace Cinema.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        Components.Movie movie;
        public EditPage(Components.Movie _movie)
        {
            InitializeComponent();
            movie = _movie;
            DataContext = movie;
            GenreCbx.ItemsSource = DBConnect.db.Genre.ToList();
            GenreCbx.DisplayMemberPath = "Name";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (movie.Id == 0)
            {
                DBConnect.db.Movie.Add(movie);

            }

            DBConnect.db.SaveChanges();
            MessageBox.Show("Успешно сохранено!");
        }

        private void AddImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "*.png|*.png|*.jpg|*.jpg"
            };
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                movie.Image = File.ReadAllBytes(openFileDialog.FileName);
                MovieImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
