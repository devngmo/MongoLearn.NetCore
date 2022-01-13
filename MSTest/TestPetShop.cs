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
        [AssemblyInitialize()]
        public static void initAssemly(TestContext context)
        {
            PetShop.PetShop.MapBsonID();
        }
        PetShop.PetShop shop;

        [TestInitialize]
        public void setupDB()
        {
            shop = new PetShop.PetShop(TestConfig.Load("PETSHOP"));
            shop.Clear();
            shop.SetupSamples();
        }
        //===============================================
        #region Test Queries from Samples
        //===============================================
        [TestMethod]
        [TestCategory("Queries from Init Samples")]
        public void get_all_pets_in_shop()
        {
            var allAscending = shop.Pets.getAllPets();
            Assert.IsNotNull(allAscending);
            Assert.AreEqual(shop.InitSamplePetCount, allAscending.Count);
            for (int i = 0; i < allAscending.Count; i++)
            {
                Assert.AreEqual($"Dog {i + 1}", allAscending[i].name);
            }

            var allDescending = shop.Pets.getAllPets(false);
            Assert.IsNotNull(allDescending);
            Assert.AreEqual(shop.InitSamplePetCount, allDescending.Count);
            for (int i = 0; i < allDescending.Count; i++)
            {
                Assert.AreEqual($"Dog {allDescending.Count - i}", allDescending[i].name);
            }
        }

        [TestMethod]
        [TestCategory("Queries from Init Samples")]
        public void make_sure_no_client_order_in_init_sample()
        {
            var allOrders = shop.Orders.getAllOrders();
            Assert.IsNotNull(allOrders);
            Assert.AreEqual(0, allOrders.Count);

            var allClients = shop.Clients.getAllClients();
            Assert.IsNotNull(allClients);
            Assert.AreEqual(0, allClients.Count);
        }
        #endregion
        //===============================================


    }
}
