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
using System.Collections.ObjectModel;
using System.Data.Entity;
using Cinema.Components;

namespace Cinema.Pages
{
    /// <summary>
    /// Логика взаимодействия для SessionPage.xaml
    /// </summary>
    public partial class SessionPage : Page
    {
        public SessionPage()
        {
            InitializeComponent();
            if (Navigation.AuthUser.RoleId == 2)
                AddNewBtn.Visibility = Visibility.Collapsed;
            //DeleteBtn.Visibility = Visibility.Collapsed;
            else if (Navigation.AuthUser.RoleId == 3)
            {
                AddNewBtn.Visibility = Visibility.Collapsed;
            }

            //BtnEditSession.Visibility = Visibility.Collapsed;
            //DeleteBtn.Visibility = Visibility.Collapsed;
            //SessionDG.ItemsSource = DBConnect.db.Session.ToList();
            DBConnect.db.Session.Load();
            Sessions = DBConnect.db.Session.Local;

        }
        public ObservableCollection<Session> Sessions
        {
            get { return (ObservableCollection<Session>)GetValue(SessionsProperty); }
            set { SetValue(SessionsProperty, value); }
        }
        public static readonly DependencyProperty SessionsProperty = DependencyProperty.Register("Sessions", typeof(ObservableCollection<Session>), typeof(SessionPage));
        private void Refresh()
        {
            IEnumerable<Session> sessionL = DBConnect.db.Session;
            ObservableCollection<Session> sessions = Sessions;
            {
                if (CbSort == null)
                    return;

                if (CbSort.SelectedItem != null)
                {
                    switch ((CbSort.SelectedItem as ComboBoxItem).Tag)
                    {
                        case "1":

                            sessionL = DBConnect.db.Session;
                            break;
                        case "2":

                            sessionL = sessionL.OrderBy(x => x.Movie.Name);
                            break;
                        case "3":

                            sessionL = sessionL.OrderByDescending(x => x.Movie.Name);

                            break;
                        case "4":

                            sessionL = sessionL.OrderBy(x => x.TimeOfSession);
                            break;
                        case "5":

                            sessionL = sessionL.OrderByDescending(x => x.TimeOfSession);
                            break;

                    }

                }
                
                if (SearchTb == null)
                    return;
                if (SearchTb.Text.Length > 0)
                {

                    sessionL = sessionL.Where(x => x.Movie.Name.StartsWith(SearchTb.Text));
                }
                

                if (CbFiltration == null)
                    return;
                if (CbFiltration.SelectedItem != null)
                {
                    switch ((CbFiltration.SelectedItem as ComboBoxItem).Tag)
                    {
                        case "1":

                            sessionL = DBConnect.db.Session;
                            break;
                        case "2":

                            sessionL = sessionL.Where(x => x.Movie.HoleId == 1);
                            break;
                        case "3":

                            sessionL = sessionL.Where(x => x.Movie.HoleId == 2);
                            break;
                        default:

                            sessionL = sessionL.Where(x => x.Movie.HoleId == 3);
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
            SessionDG.ItemsSource = sessionL.ToList();

        }
        private void BtnEditSession_Click(object sender, RoutedEventArgs e)
        {
            var selSession = (sender as Button).DataContext as Session;
            Navigation.NextPage(new Nav("Редактирование сеанса", new AddEditSessionPage(selSession)));
            //Navigation.NextPage(new Nav("Редактирование сеанса", new AddEditSessionPage((sender as Button).DataContext as Session)));
        }

        private void AddNewBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Добавление сеанса", new AddEditSessionPage(new Session())));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selSession = SessionDG.SelectedItems.Cast<Session>().ToList();
            if (MessageBox.Show("Вы точно хотите удалить эту запись?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DBConnect.db.Session.RemoveRange(selSession);
                DBConnect.db.SaveChanges();
                MessageBox.Show("Данные удалены");
                SessionDG.ItemsSource = DBConnect.db.Session.ToList();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                DBConnect.db.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                SessionDG.ItemsSource = DBConnect.db.Session.ToList();
            }
        }

        private void FilmBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Фильмы", new FilmPage()));
        }

        private void SearchTb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void CbFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void TicketBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Список билетов", new TicketPage()));
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.isAuth = false;
            Navigation.navs.Clear();
            Navigation.NextPage(new Nav("Авторизация", new AuthPage()));
        }
    }
}
