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
    
    public partial class MovieSeat
    {
        public int Id { get; set; }
        public Nullable<int> MovieId { get; set; }
        public Nullable<int> SeatId { get; set; }
    
        public virtual Movie Movie { get; set; }
        public virtual Seat Seat { get; set; }
    }
}