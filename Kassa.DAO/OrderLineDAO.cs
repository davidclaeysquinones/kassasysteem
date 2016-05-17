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

        public IEnumerable<OrderLine> GetOpenLinesTable(int tableId) 
        {
            using (var db = new kassaEntities())
            {

                return db.OrderLine.Where(o => o.Order.Status == 0 && o.Order.TafelId == tableId).Include( o=> o.Order).ToList();
            }
        }

        public IEnumerable<OrderLine> GetAllOrderlinesFromOrder(int orderId)
        {
            using (var db = new kassaEntities())
            {

                return db.OrderLine.Where(o => o.OrderId == orderId).ToList();
            }
        }
        public void Remove(OrderLine orderline)
        {
            using (var db = new kassaEntities())
            {
                db.Entry(orderline).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public void Update(OrderLine orderline)
        {
            using (var db = new kassaEntities())
            {
                db.Entry(orderline).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public int GetId(int orderId,int artikelId)
        {
            using (var db = new kassaEntities())
            {
                OrderLine orderline = db.OrderLine.FirstOrDefault(o => o.OrderId == orderId && o.ArticleId == artikelId);
                if(orderline != null)
                {
                    return orderline.Id;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
