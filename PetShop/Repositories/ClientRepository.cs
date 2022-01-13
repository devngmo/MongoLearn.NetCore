using MongoDB.Driver;
using PetShop.Domain.Entities;

namespace PetShop
{
    public class ClientRepository
    {
        IMongoDatabase db;
        IMongoCollection<ClientEntity> ClientCollection => 
            db.GetCollection<ClientEntity>("CLIENTS");
        public ClientRepository(IMongoDatabase db)
        {
            this.db = db;
        }
        public List<ClientEntity> getAllClients(bool nameAscending = true)
        {
            var result = ClientCollection
                .Find(Builders<ClientEntity>.Filter.Empty);
            if (nameAscending)
                result = result.Sort(Builders<ClientEntity>.Sort.Ascending("name"));
            else
                result = result.Sort(Builders<ClientEntity>.Sort.Descending("name"));
            return result.ToList();
        }

        public void Clear()
        {
            ClientCollection.DeleteMany(Builders<ClientEntity>.Filter.Empty);
        }

        public void add(ClientEntity client)
        {
            ClientCollection.InsertOne(client);
        }
    }
}