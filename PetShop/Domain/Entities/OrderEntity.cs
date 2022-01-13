using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShop.Domain.Entities
{
    public class OrderEntity
    {
        public class Record
        {
            public ObjectId pet_id { get; set; }
            public int quantity { get; set; }
        }
        [BsonId]
        public ObjectId _id { get; set; }
        public ObjectId client_id { get; set; }
        public DateTime order_time { get; set; }

        public List<Record> records { get; set; }
    }
}
