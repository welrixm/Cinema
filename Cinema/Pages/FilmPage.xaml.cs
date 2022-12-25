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
        int actualPage = 0;
        public FilmPage()
        {
            InitializeComponent();
            if (Navigation.AuthUser.RoleId == 2)
                AddNewMovieBtn.Visibility = Visibility.Collapsed;
            //FilmList.ItemsSource = DBConnect.db.Movie.ToList();
            else if (Navigation.AuthUser.RoleId == 3)
            {
                AddNewMovieBtn.Visibility = Visibility.Collapsed;
            }
            DBConnect.db.Movie.Load();
            Movies = DBConnect.db.Movie.Local;
            FilmList.ItemsSource = DBConnect.db.Movie.Where(x => x.IsActive != false).ToList();
            GeneralCount.Text = DBConnect.db.Movie.Where(x => x.IsActive != false).Count().ToString();
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
                          
                            filmL = DBConnect.db.Movie;
                            break;
                        case "2":
                           
                            filmL = filmL.OrderBy(x => x.Name);
                            break;
                        case "3":
                          
                            filmL = filmL.OrderByDescending(x => x.Name);

                            break;
                        case "4":
                           
                            filmL = filmL.OrderBy(x => x.ReleaseDate);
                            break;
                        case "5":
                            
                            filmL = filmL.OrderByDescending(x => x.ReleaseDate);
                            break;

                    }

                }
                
                if (SearchTb == null)
                    return;
                if (SearchTb.Text.Length > 0)
                {
                   
                    filmL = filmL.Where(x => x.Name.StartsWith(SearchTb.Text));
                }
                

                if (CbFiltration == null)
                    return;
                if (CbFiltration.SelectedItem != null)
                {
                    switch ((CbFiltration.SelectedItem as ComboBoxItem).Tag)
                    {
                        case "1":
                           
                            filmL = DBConnect.db.Movie;
                            break;
                        case "2":
                            
                            filmL = filmL.Where(x => x.GenreId == 1);
                            break;
                        case "3":
                           
                            filmL = filmL.Where(x => x.GenreId == 2);
                            break;
                        case "4":
                           
                            filmL = filmL.Where(x => x.GenreId == 3);
                            break;
                        case "5":
                        
                            filmL = filmL.Where(x => x.GenreId == 4);
                            break;
                        case "6":
                            
                            filmL = filmL.Where(x => x.GenreId == 5);
                            break;
                        default:
                           
                            filmL = filmL.Where(x => x.GenreId == 6);
                            break;
                            
                    }
                }
                // ListProduct.ItemsSource = products.ToList();
                if (CbCount.SelectedIndex > 0 && filmL.Count() > 0)
                {
                    int selCount = Convert.ToInt32((CbCount.SelectedItem as ComboBoxItem).Content);
                    filmL = new ObservableCollection<Movie>(Movies.Skip(selCount * actualPage).Take(selCount));
                    if (movies.Count() == 0)
                    {
                        actualPage--;

                    }
                }

                FoundCount.Text = filmL.Count().ToString() + " из ";
            }
            //ListProduct.ItemsSource = products.ToList();
            FilmList.ItemsSource = filmL.ToList();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Фильмы", new FilmPage()));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selMovie = (sender as Button).DataContext as Movie;
            Navigation.NextPage(new Nav("Редактирование фильма", new EditPage(selMovie)));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selMovie = FilmList.SelectedItems.Cast<Movie>().ToList();
            if (MessageBox.Show("Вы точно хотите удалить эту запись?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DBConnect.db.Movie.RemoveRange(selMovie);
                DBConnect.db.SaveChanges();
                MessageBox.Show("Данные удалены");
                FilmList.ItemsSource = DBConnect.db.Movie.ToList();
            }
        }

        private void AddNewMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Добавление нового фильма", new EditPage(new Movie())));
        }

       

        private void BuyTicketBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Покупка билета", new BuyTicket(new Ticket())));
        }

        //private void TicketBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    Navigation.NextPage(new Nav("Список билетов", new TicketPage()));
        //}

        private void SearchTb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }

        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }

        private void CbCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void CbFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }

        private void SessionBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Сеансы", new SessionPage()));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DBConnect.db.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                FilmList.ItemsSource = DBConnect.db.Movie.ToList();
            }
        }

        private void TicketBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Список билетов", new TicketPage()));
        }

        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {
            actualPage--;
            if (actualPage < 0)
                actualPage = 0;
            Refresh();
        }

        private void RightBtn_Click(object sender, RoutedEventArgs e)
        {
            actualPage++;
            Refresh();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.isAuth = false;
            Navigation.navs.Clear();
            Navigation.NextPage(new Nav("Авторизация", new AuthPage()));
        }
    }
}
