using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.Model;

namespace Kassa.DAO
{
    public class OrderLineDAO
    {
        public void Add(OrderLine orderline)
        {
            using (var db = new kassaEntities())
    {
                db.Entry(orderline).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
