using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomApplication.Classes
{
    internal class BasketClass
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public double Cost { get; set; }
        public Bitmap MenuItemImage { get; set; }
        public int Tag { get; set; }
        public uint Count { get; set; }
    }
}
