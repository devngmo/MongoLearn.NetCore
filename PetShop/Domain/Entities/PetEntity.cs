using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShop.Domain.Entities
{
    public class PetEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public string race { get; set; }
        /// <summary>
        /// Age in year
        /// </summary>
        public int age{ get; set; }
        /// <summary>
        /// weight in Kg
        /// </summary>
        public float weight { get; set; }
    }
}
