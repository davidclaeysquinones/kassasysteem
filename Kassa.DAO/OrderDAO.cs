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
        public void Add(Order order)
        {
            using (var db = new kassaEntities())
            {
                db.Entry(order).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
