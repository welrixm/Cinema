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
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace Cinema.Pages
{
    /// <summary>
    /// Логика взаимодействия для FilmPage.xaml
    /// </summary>
    public partial class FilmPage : Page
    {
        public FilmPage()
        {
            InitializeComponent();
            FilmList.ItemsSource = DBConnect.db.Movie.ToList();
            DBConnect.db.Movie.Load();
            Movies = DBConnect.db.Movie.Local;
        }
        public ObservableCollection<Movie> Movies
        {
            get { return (ObservableCollection<Movie>)GetValue(MoviesProperty); }
            set { SetValue(MoviesProperty, value); }
        }
        public static readonly DependencyProperty MoviesProperty = DependencyProperty.Register("Movies", typeof(ObservableCollection<Movie>), typeof(FilmPage));
        private void Refresh()
        {
            IEnumerable<Movie> filmL = DBConnect.db.Movie;
            ObservableCollection<Movie> movies = Movies;
            {
                if (CbSort == null)
                    return;

                if (CbSort.SelectedItem != null)
                {
                    switch ((CbSort.SelectedItem as ComboBoxItem).Tag)
                    {
                        case "1":
                            //products = DBConnect.db.Product.Local;
                            filmL = DBConnect.db.Movie;
                            break;
                        case "2":
                            //products = new ObservableCollection<Product>(Products.OrderBy(x => x.Name));
                            filmL = filmL.OrderBy(x => x.Name);
                            break;
                        case "3":
                            //products = new ObservableCollection<Product>(Products.OrderByDescending(x => x.Name));
                            filmL = filmL.OrderByDescending(x => x.Name);

                            break;
                        case "4":
                            //products = new ObservableCollection<Product>(Products.OrderBy(x => x.DateOfAddition));
                            filmL = filmL.OrderBy(x => x.ReleaseDate);
                            break;
                        case "5":
                            //products = new ObservableCollection<Product>(Products.OrderByDescending(x => x.DateOfAddition));
                            filmL = filmL.OrderByDescending(x => x.ReleaseDate);
                            break;

                    }

                }
                // ListProduct.ItemsSource = products.ToList();

                if (SearchTb == null)
                    return;
                if (SearchTb.Text.Length > 0)
                {
                    //products = new ObservableCollection<Product>(Products.Where(x => x.Name.ToLower().StartsWith(TxtSearch.Text.ToLower()) || x.Description.ToLower().StartsWith(TxtSearch.Text.ToLower())));
                    filmL = filmL.Where(x => x.Name.StartsWith(SearchTb.Text));
                }
                //ListProduct.ItemsSource = products.ToList();

                if (CbFiltration == null)
                    return;
                if (CbFiltration.SelectedItem != null)
                {
                    switch ((CbFiltration.SelectedItem as ComboBoxItem).Tag)
                    {
                        case "1":
                            //products = DBConnect.db.Product.Local;
                            filmL = DBConnect.db.Movie;
                            break;
                        case "2":
                            //products = new ObservableCollection<Product>(Products.Where(x => x.UnitId == 2));
                            filmL = filmL.Where(x => x.GenreId == 1);
                            break;
                        case "3":
                            //products = new ObservableCollection<Product>(Products.Where(x => x.UnitId == 1));
                            filmL = filmL.Where(x => x.GenreId == 2);
                            break;
                        case "4":
                            //products = new ObservableCollection<Product>(Products.Where(x => x.UnitId == 1));
                            filmL = filmL.Where(x => x.GenreId == 3);
                            break;
                        case "5":
                            //products = new ObservableCollection<Product>(Products.Where(x => x.UnitId == 1));
                            filmL = filmL.Where(x => x.GenreId == 4);
                            break;
                        case "6":
                            //products = new ObservableCollection<Product>(Products.Where(x => x.UnitId == 1));
                            filmL = filmL.Where(x => x.GenreId == 5);
                            break;
                        default:
                            //products = new ObservableCollection<Product>(Products.Where(x => x.UnitId == 1));
                            filmL = filmL.Where(x => x.GenreId == 6);
                            break;
                            
                    }
                }
                // ListProduct.ItemsSource = products.ToList();
                //if (CbCount.SelectedIndex > 0 && products.Count() > 0)
                //{
                //    int selCount = Convert.ToInt32((CbCount.SelectedItem as ComboBoxItem).Content);
                //    products = new ObservableCollection<Product>(Products.Skip(selCount * actualPage).Take(selCount));
                //    if (products.Count() == 0)
                //    {
                //        actualPage--;

                //    }
                //}

                //FoundCount.Text = products.Count().ToString() + " из ";
            }
            //ListProduct.ItemsSource = products.ToList();
            FilmList.ItemsSource = filmL.ToList();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Кинотеатр", new FilmPage()));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selMovie = (sender as Button).DataContext as Movie;
            Navigation.NextPage(new Nav("Редактирование фильма", new EditPage(selMovie)));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selMovie = (sender as Button).DataContext as Movie;
            if (MessageBox.Show("Вы точно хотите удалить эту запись?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                selMovie.IsActive = false;
                MessageBox.Show("Запись удалена");
                DBConnect.db.SaveChanges();
                FilmList.ItemsSource = DBConnect.db.Movie.ToList();
            }
        }

        private void AddNewMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Добавление нового фильма", new EditPage(new Movie())));
        }

       

        private void BuyTicketBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TicketBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void CbCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // Refresh();
        }

        private void CbFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }
    }
}
