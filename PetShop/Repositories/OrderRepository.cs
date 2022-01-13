using MongoDB.Driver;
using PetShop.Domain.Entities;

namespace PetShop
{
    public class OrderRepository
    {
        IMongoDatabase db;
        IMongoCollection<OrderEntity> OrderCollection => 
            db.GetCollection<OrderEntity>("ORDERS");
        public OrderRepository(IMongoDatabase db)
        {
            this.db = db;
        }
        public List<OrderEntity> getOrderHistoryForClient(string clientID)
        {
            var result = OrderCollection
                .Find(Builders<OrderEntity>.Filter.Eq("client_id", clientID));
            return result.ToList();
        }
        public List<OrderEntity> getAllOrders()
        {
            var result = OrderCollection
                .Find(Builders<OrderEntity>.Filter.Empty);
            return result.ToList();
        }
        public void Clear()
        {
            OrderCollection.DeleteMany(Builders<OrderEntity>.Filter.Empty);
        }

        public void add(OrderEntity order)
        {
            OrderCollection.InsertOne(order);
        }
    }
}