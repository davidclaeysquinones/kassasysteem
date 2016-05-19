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

        public int OrderExists(int tafelId)
        {
            using (var db = new kassaEntities())
            {
                Order order = db.Order.FirstOrDefault(o => o.TafelId == tafelId && o.Status == 0);
                if(order == null)
                {
                    return -1;
                }
                else
                {
                    return order.Id;
                }
            }
        }

        public Order getOrderObject(int orderId)
        {
            using (var db = new kassaEntities())
            {
                return db.Order.Find(orderId);
            }
        }

        public void Remove(Order order)
        {
            using (var db = new kassaEntities())
            {
                db.Entry(order).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Update(Order order)
        {
            using (var db = new kassaEntities())
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public IEnumerable<Order> GetAllOrders()
        {
            using (var db = new kassaEntities())
            {
                return db.Order.ToList();
            }
        }
    }
}
