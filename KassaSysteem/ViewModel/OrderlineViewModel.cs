using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSysteem.ViewModel
{
    public class OrderlineViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ArticleId { get; set; }
        public string ArticleName { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }
        public float Total { get; set; }
        public System.DateTime CreatedDate { get; set; }

    }
}
