using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using PetShop;
using PetShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace MSTest
{
    [TestClass]
    public class TestPetShop
    {
        PetShop.PetShop shop;

        [TestInitialize]
        public void setupDB()
        {
            shop = new PetShop.PetShop(TestConfig.Load("PETSHOP"));
            shop.MapBsonID();
            shop.Clear();
            shop.SetupSamples();
        }


        [TestMethod]
        [TestCategory("Queries")]
        public void get_all_pets_in_shop()
        {
            var allAscending = shop.PetRepository.getAllPets();
            Assert.IsNotNull(allAscending);
            Assert.AreEqual(shop.InitSamplePetCount, allAscending.Count);
            for (int i = 0; i < allAscending.Count; i++)
            {
                Assert.AreEqual($"Dog {i + 1}", allAscending[i].name);
            }

            var allDescending = shop.PetRepository.getAllPets(false);
            Assert.IsNotNull(allDescending);
            Assert.AreEqual(shop.InitSamplePetCount, allDescending.Count);
            for (int i = 0; i < allDescending.Count; i++)
            {
                Assert.AreEqual($"Dog {allDescending.Count - i}", allDescending[i].name);
            }
        }
    }
}
