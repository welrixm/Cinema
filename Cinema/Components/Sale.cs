//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cinema.Components
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sale
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> DateOdSale { get; set; }
        public Nullable<int> TicketId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Place { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}