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
        public void Remove(OrderLine orderline)
        {
            orderlineDAO.Remove(orderline);
        }
    }
}
