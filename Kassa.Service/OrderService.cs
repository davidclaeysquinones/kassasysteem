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
        public void Add(Order order)
        {
            orderDAO.Add(order);
        }
    }
}
