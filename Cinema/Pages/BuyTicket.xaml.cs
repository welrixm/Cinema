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
    /// Логика взаимодействия для BuyTicket.xaml
    /// </summary>
    public partial class BuyTicket : Page
    {
        Components.Ticket ticket;


        public BuyTicket(Components.Ticket _ticket)
        {
            InitializeComponent();
            SeatList.ItemsSource = DBConnect.db.Seat.ToList();
            ticket = _ticket;
            DataContext = ticket;
            FilmCbx.ItemsSource = DBConnect.db.Session.ToList();
            FilmCbx.DisplayMemberPath = "Id";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ticket.Id == 0)
            {
                DBConnect.db.Ticket.Add(ticket);

            }
            //try
            //{
            DBConnect.db.SaveChanges();
            MessageBox.Show("Покупка совершена!");
        }

        private void TotalTbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(QuanTbx.Text.Length > 0)
            {
                TotalTbx.Text = (Convert.ToInt32(CostTbx.Text) * Convert.ToInt32(QuanTbx.Text)).ToString();
            }
        }

        private void AddSeatBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SeatList.SelectedItem != null)
            {
                var itemSeat = SeatList.SelectedItem as Seat;
                TicketSeat ticketSeat = new TicketSeat
                {
                    Seat = itemSeat,
                    Ticket = ticket
                };
                DBConnect.db.TicketSeat.Add(ticketSeat);
                DBConnect.db.SaveChanges();
                MessageBox.Show("Вы выбрали кресло/кресла");
            }
            else
            {
                MessageBox.Show("Выберите кресло");
            }
        }

        private void ClearSeatBtn_Click(object sender, RoutedEventArgs e)
        {
            int seatid = 0;
            if (SeatList.SelectedItems != null)
            {
                var itemSeat = SeatList.SelectedIndex + 1;
                seatid = itemSeat;
                var ticketSeat = DBConnect.db.TicketSeat.Where(x => x.SeatId == itemSeat && x.TicketId == ticket.Id).FirstOrDefault();
                DBConnect.db.TicketSeat.Remove(ticketSeat);
                DBConnect.db.SaveChanges();
                MessageBox.Show("Вы удалили кресло");
            }
            else
            {
                MessageBox.Show("Выберите кресло");
            }
        }

    }
}
