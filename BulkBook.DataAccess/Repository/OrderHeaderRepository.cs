using BulkBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BulkBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly AppDbContext _db;
        public OrderHeaderRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void updateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(i => i.Id == id);
            if (orderFromDb!=null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                 orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        public void updateStripePaymentID(int id, string sessionId, string paymentintentId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(i => i.Id == id);
           orderFromDb.SessionId=sessionId;
            orderFromDb.PaymentIntentId = paymentintentId;
            orderFromDb.PaymentDate = DateTime.Now;

        }
    }
}
