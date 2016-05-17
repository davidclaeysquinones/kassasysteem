using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.DAO;
using Kassa.Model;

namespace Kassa.Service
{
    public class OrderService
    {
        private OrderDAO orderDAO;
        public OrderService()
        {
            orderDAO = new OrderDAO();
        }
        public int Add(Order order)
        {
            return orderDAO.Add(order);
        }
        public int OrderExists(int tafelId)
        {
            return orderDAO.OrderExists(tafelId);
        }
    }
}
