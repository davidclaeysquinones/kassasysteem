using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.Model;

namespace Kassa.DAO
{
    public class OrderDAO
    {
        private int id;

        public int Add(Order order)
        {
            using (var db = new kassaEntities())
            {
                db.Entry(order).State = EntityState.Added;
                db.SaveChanges();

                return id = order.Id;
            }
        }

        public Order getOrder(int id)
        {
            using (var db = new kassaEntities())
            {
                return db.Order.Find(id);
            }
        }
    }
}
