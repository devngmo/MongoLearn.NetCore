using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using PetShop.Domain.Entities;

namespace PetShop
{
    public class ClientRepository
    {
        IMongoDatabase db;
        IMongoCollection<ClientEntity> Clients => 
            db.GetCollection<ClientEntity>("CLIENTS");

        IMongoCollection<OrderEntity> Orders =>
            db.GetCollection<OrderEntity>("ORDERS");

        public ClientRepository(IMongoDatabase db)
        {
            this.db = db;
        }
        public List<ClientEntity> getAllClients(bool nameAscending = true)
        {
            var result = Clients
                .Find(Builders<ClientEntity>.Filter.Empty);
            if (nameAscending)
                result = result.Sort(Builders<ClientEntity>.Sort.Ascending("name"));
            else
                result = result.Sort(Builders<ClientEntity>.Sort.Descending("name"));
            return result.ToList();
        }

        public void Clear()
        {
            Clients.DeleteMany(Builders<ClientEntity>.Filter.Empty);
        }

        public ObjectId add(ClientEntity client)
        {
            Clients.InsertOne(client);
            return client._id;
        }

        public void getAllClientsOrderedInDayUsingFluent(DateTime now)
        {
            var dayStart = now.Date;
            var filter = Builders<OrderEntity>.Filter.Gte("order_time", dayStart);
            filter &= Builders<OrderEntity>.Filter.Lte("order_time", dayStart.AddDays(1));
            var foundOrders = Orders.Find(filter);
            foreach (var o in foundOrders.ToList())
            {
                Console.WriteLine(JsonConvert.SerializeObject(o, Formatting.Indented));
            }
        }

        public List<ClientEntity> getAllClientsOrderedInDay(DateTime now)
        {
            return getAllClientsOrderedInDayUsingLinQ(now);
        }

        public List<ClientEntity> getAllClientsOrderedInDayUsingLinQ(DateTime now)
        {
            var dayStart = now.Date;
            var result = 
                    from c in Clients.AsQueryable()
                    join o2 in Orders.AsQueryable()
                    on c._id equals o2.client_id
                    where o2.order_time >= dayStart
                    select new ClientEntity { name = c.name, _id = c._id }
                ;
            return result.ToList();
        }

        public void getAllClientsOrderedInDayUsingRawCommand(DateTime now)
        {
            string cmd = @"
{
    'aggregate': 'ORDERS',
    'allowDiskUse': true,
    'pipeline':[
    {
        '$project':{
            order_day: { $dateToString: { format: '%Y-%m-%d', date: '$order_time' } },
            client_id: '$client_id'
        }
    },
    {
        '$match': {
            order_day : '" + now.ToString("yyyy-MM-dd") + @"'
        }
    }
    ],
    'cursor': { 'batchSize': 25 }
}";
            var result = db.RunCommand<object>(cmd);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            //foreach (var c in result)
            //{
            //    Console.WriteLine(JsonConvert.SerializeObject(c, Formatting.Indented));
            //}
        }
        public void getAllClientsOrderedFrom(DateTime now)
        {
            var result = Clients.Aggregate()
                .Lookup("ORDERS", "_id", "client_id", @as: "joined")
                .Match(Builders<BsonDocument>.Filter.Gte("joined.order_time", now.Date))
                .ToList();

            foreach (var c in result)
            {
                Console.WriteLine(JsonConvert.SerializeObject(c, Formatting.Indented));
            }
        }
    }
}