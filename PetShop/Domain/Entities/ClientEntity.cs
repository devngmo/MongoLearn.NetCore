using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShop.Domain.Entities
{
    public class ClientEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string name { get; set; }
    }
}
