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
        public Order getOrderObject(int orderId)
        {
            return orderDAO.getOrderObject(orderId);
        }
        public void Remove(Order order)
        {
            orderDAO.Remove(order);
        }
        public void Update(Order order)
        {
            orderDAO.Update(order);
        }
        public IEnumerable<Order> GetAllOrders()
        {
            return orderDAO.GetAllOrders();
        }
    }
}
