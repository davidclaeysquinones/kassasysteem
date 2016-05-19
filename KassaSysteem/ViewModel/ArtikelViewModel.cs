using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.Model;

namespace KassaSysteem.ViewModel
{
    public class ArtikelViewModel
    {
        public String Name { get; set; }
        public float Price { get; set; }
        public int Position { get; set; }

        public int ArtikelId { get; set; }
    }
}
