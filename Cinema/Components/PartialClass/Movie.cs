using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Cinema.Components
{
    partial class Movie
    {
        public string ActiveText
        {
            get
            {
                if (IsActive == true)
                    return "Актуален";
                else
                    return "Нет в наличии";
                
            }
        }
        public string Color
        {
            get
            {
                if (IsActive == true)
                    return "#FDFDFD";
                else
                    return "#E7E3DF";
                
            }
        }
        public Visibility BtnVisible
        {
            get
            {
                if (Navigation.AuthUser.RoleId == 2)//client
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
    }
}
