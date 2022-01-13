using MongoDB.Driver;
using PetShop.Domain.Entities;

namespace PetShop
{
    public class PetRepository
    {
        IMongoDatabase db;
        IMongoCollection<PetEntity> PetCollection => db.GetCollection<PetEntity>("PETS");
        public PetRepository(IMongoDatabase db)
        {
            this.db = db;
        }
        public List<PetEntity> getAllPets(bool nameAscending = true)
        {
            var result = PetCollection
                .Find(Builders<PetEntity>.Filter.Empty);
            if (nameAscending)
                result = result.Sort(Builders<PetEntity>.Sort.Ascending("name"));
            else
                result = result.Sort(Builders<PetEntity>.Sort.Descending("name"));
            return result.ToList();
        }

        internal void Clear()
        {
            PetCollection.DeleteMany(Builders<PetEntity>.Filter.Empty);
        }

        internal void add(PetEntity petEntity)
        {
            PetCollection.InsertOne(petEntity);
        }
    }
}