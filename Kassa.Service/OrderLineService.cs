using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.DAO;
using Kassa.Model;

namespace Kassa.Service
{
    public class OrderLineService
    {
        private OrderLineDAO orderlineDAO;
        public OrderLineService()
        {
            orderlineDAO = new OrderLineDAO();
        }
        public void Add(OrderLine orderline)
        {
            orderlineDAO.Add(orderline);
        }
        public IEnumerable<OrderLine> GetOpenLinesTable(int tableId)
        {
            return orderlineDAO.GetOpenLinesTable(tableId);
        }
        public IEnumerable<OrderLine> GetAllOrderlinesFromOrder(int orderId)
        {
            return orderlineDAO.GetAllOrderlinesFromOrder(orderId);
        }
        public void Remove(OrderLine orderline)
        {
            orderlineDAO.Remove(orderline);
        }
        public void Update(OrderLine orderline)
        {
            orderlineDAO.Update(orderline);
        }
        public int GetId(int orderId, int artikelId)
        {
            return orderlineDAO.GetId(orderId, artikelId);
        }
    }
}
