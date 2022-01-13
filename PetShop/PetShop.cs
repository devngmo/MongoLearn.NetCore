﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PetShop.Domain.Entities;
using Utils;

namespace PetShop
{
    public class PetShop
    {
        MongoClient _client;
        IMongoDatabase _db;
        internal IMongoDatabase DB => _db;
        public PetRepository PetRepository => new PetRepository(DB);

        public int InitSamplePetCount { get; set; }

        public PetShop(TestConfig config)
        {
            _client = new MongoClient(config.DBConnection);
            _db = _client.GetDatabase(config.DBName);
            InitSamplePetCount = config.InitSamplePetCount;
        }

        public void MapBsonID()
        {

            BsonClassMap.RegisterClassMap<PetEntity>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c._id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
        }

        public void Clear()
        {
            PetRepository.Clear();
        }

        public void SetupSamples()
        {
            Random rnd = new Random();
            for (int i = 0; i < InitSamplePetCount; i++)
            {
                PetRepository.add(new PetEntity { name = $"Dog {i + 1}", race = "dog", age = 1 + rnd.Next(5), weight = 1 + rnd.Next(5) });
            }            
        }
    }
}