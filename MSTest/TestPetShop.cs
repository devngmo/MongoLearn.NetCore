using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;
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
            //PetShop.PetShop.MapBsonID();
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

        [TestMethod]
        [TestCategory("Complex queries")]
        public void list_all_clients_which_ordered_today() {
            var allPets = shop.Pets.getAllPets();
            Assert.IsNotNull(allPets);
            Assert.AreEqual(shop.InitSamplePetCount, allPets.Count);

            ObjectId AID = shop.Clients.add(new ClientEntity { name = "Client A" });
            Assert.IsNotNull(AID);

            ObjectId BID = shop.Clients.add(new ClientEntity { name = "Client B" });
            Assert.IsNotNull(BID);

            shop.Orders.add(new OrderEntity { 
                client_id = BID,
                order_time = DateTime.Now.AddDays(-1),
                records = new List<OrderEntity.Record> { 
                    new OrderEntity.Record { pet_id = allPets[0]._id, quantity = 1 },
                }
            });
            shop.Orders.add(new OrderEntity
            {
                client_id = BID,
                order_time = DateTime.Now,
                records = new List<OrderEntity.Record> {
                    new OrderEntity.Record { pet_id = allPets[0]._id, quantity = 1 },
                    new OrderEntity.Record { pet_id = allPets[1]._id, quantity = 2 }
                }
            });


            var allOrders = shop.Orders.getAllOrders();
            Assert.AreEqual(2, allOrders.Count);
            Assert.AreEqual(1, allOrders[0].records.Count);
            Assert.AreEqual(BID, allOrders[0].client_id);
            Assert.AreEqual(2, allOrders[1].records.Count);
            Assert.AreEqual(BID, allOrders[1].client_id);

            var ls = shop.Clients.getAllClientsOrderedInDayUsingLinQ(DateTime.Now);
            Assert.IsNotNull(ls);
            Assert.AreEqual(1, ls.Count);
            Assert.AreEqual(BID, ls[0]._id);
            Assert.AreEqual("Client B", ls[0].name);
            foreach (var c in ls)
            {
                Console.WriteLine(JsonConvert.SerializeObject(c, Formatting.Indented));
            }
        }
    }
}
