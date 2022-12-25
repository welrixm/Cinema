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
    /// Логика взаимодействия для TicketPage.xaml
    /// </summary>
    public partial class TicketPage : Page
    {
        public TicketPage()
        {
            InitializeComponent();
            //TicketDG.ItemsSource = DBConnect.db.Ticket.ToList();
            DBConnect.db.Ticket.Load();
            Tickets = DBConnect.db.Ticket.Local;
            if (Navigation.AuthUser.RoleId == 2)
                DeleteTicketBtn.Visibility = Visibility.Collapsed;
            //DeleteBtn.Visibility = Visibility.Collapsed;
            //else if (Navigation.AuthUser.RoleId == 3)
            //{
            //    AddNewBtn.Visibility = Visibility.Collapsed;
            //}

        }
        public ObservableCollection<Ticket> Tickets
        {
            get { return (ObservableCollection<Ticket>)GetValue(TicketsProperty); }
            set { SetValue(TicketsProperty, value); }
        }
        public static readonly DependencyProperty TicketsProperty = DependencyProperty.Register("Tickets", typeof(ObservableCollection<Ticket>), typeof(TicketPage));
        private void Refresh()
        {
            IEnumerable<Ticket> ticketL = DBConnect.db.Ticket;
            ObservableCollection<Ticket> tickets = Tickets;
            {
                

                if (SearchTb == null)
                    return;
                if (SearchTb.Text.Length > 0)
                {

                    ticketL = ticketL.Where(x => x.Session.Movie.Name.StartsWith(SearchTb.Text));
                }


                if (CbFiltration == null)
                    return;
                if (CbFiltration.SelectedItem != null)
                {
                    switch ((CbFiltration.SelectedItem as ComboBoxItem).Tag)
                    {
                        case "1":

                            ticketL = DBConnect.db.Ticket;
                            break;
                        case "2":

                            ticketL = ticketL.Where(x => x.Session.Movie.HoleId == 1);
                            break;
                        case "3":

                            ticketL = ticketL.Where(x => x.Session.Movie.HoleId == 2);
                            break;
                        default:

                            ticketL = ticketL.Where(x => x.Session.Movie.HoleId == 3);
                            break;



                    }
                }
                
            }
            
            TicketDG.ItemsSource = ticketL.ToList();

        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DBConnect.db.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                TicketDG.ItemsSource = DBConnect.db.Ticket.ToList();
            }
        }

        private void SessionBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Сеансы", new SessionPage()));
        }

        private void FilmBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Фильмы", new FilmPage()));
        }

        private void BtnEditTicket_Click(object sender, RoutedEventArgs e)
        {
            Navigation.NextPage(new Nav("Редактирование билета", new BuyTicket((sender as Button).DataContext as Ticket)));
        }

        private void DeleteTicketBtn_Click(object sender, RoutedEventArgs e)
        {
            var selTicket = TicketDG.SelectedItems.Cast<Ticket>().ToList();
            if (MessageBox.Show("Вы точно хотите отменить заказ?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DBConnect.db.Ticket.RemoveRange(selTicket);
                DBConnect.db.SaveChanges();
                MessageBox.Show("Заказ отменен!");
                TicketDG.ItemsSource = DBConnect.db.Ticket.ToList();
            }
        }

        private void SearchTb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void CbFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
