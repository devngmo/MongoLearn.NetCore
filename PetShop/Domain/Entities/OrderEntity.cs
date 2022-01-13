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
            public string pet_id { get; set; }
            public int quantity { get; set; }
        }
        public string _id { get; set; }
        public string client_id { get; set; }

        public List<Record> records { get; set; }
    }
}
