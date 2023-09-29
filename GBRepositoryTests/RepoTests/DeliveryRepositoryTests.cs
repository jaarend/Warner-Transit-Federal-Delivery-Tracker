using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data;
using GoldBadgeChallenge.Repository.DeliveryRepository;
using Xunit;

namespace GBRepositoryTests
{
    public class DeliveryRepositoryTests
    {
        private DeliveryRepo _globalRepo;

        private Delivery _deliveryA;
        private Delivery _deliveryB;
        private Delivery _deliveryC;


        private Item _item1;
        private Item _item2;
        private Item _item3;

        
        public DeliveryRepositoryTests()
        {

            //filler vars
            DateOnly fillerOrderDate = new DateOnly(2023,09,27);
            DateOnly fillerDeliveryDate = new DateOnly(2023,10,27);


            _globalRepo = new DeliveryRepo();

            _deliveryA = new Delivery(1,fillerOrderDate,fillerDeliveryDate,GoldBadgeChallenge.Data.Enums.Status.Complete,1);
            _deliveryB = new Delivery(1,fillerOrderDate,fillerDeliveryDate,GoldBadgeChallenge.Data.Enums.Status.Canceled,2);
            _deliveryC = new Delivery(1,fillerOrderDate,fillerDeliveryDate,GoldBadgeChallenge.Data.Enums.Status.EnRoute,1);

            _globalRepo.CreateDelivery(_deliveryA);
            _globalRepo.CreateDelivery(_deliveryB);
            _globalRepo.CreateDelivery(_deliveryC);

            _item1 = new Item(1,"Stakeboard","You can play with this in a park");
            _item2 = new Item(2,"Shoes","You can use this.");
            _item3 = new Item(3,"Shirt","You can wear this.");

        }

        //CREATE
        [Fact]
        public void CreateDelivery_ShouldGetCorrectBool()
        {
            //Arrange
            Delivery delivery = new Delivery();

            //Action
            bool createResult = _globalRepo.CreateDelivery(delivery);

            //Assert
            Assert.True(createResult);
        }

        //READ/GET
        [Fact]
        public void GetDeliveryById_ShouldReturnCorrectDelivery()
        {
            
            Delivery expected = _deliveryA;

            Delivery actual = _globalRepo.GetDeliveryById(1);

            Assert.Equal(expected, actual);
            
        }

        //UPDATE
        [Fact]
        public void AddItemToDelivery_ShouldGetCorrectBool()
        {

            bool createResult = _globalRepo.AddItemToDeliveryList(_deliveryA.Id,_item1);

            Assert.True(createResult);

        }

        [Fact]
        public void UpdateDeliveryInfo_ShouldReturnCorrectBool()
        {
            DateOnly fillerOrderDate = new DateOnly(2023,08,27);
            DateOnly fillerDeliveryDate = new DateOnly(2023,10,17);

            Delivery updatedDelivery = new Delivery(1,fillerOrderDate,fillerDeliveryDate,GoldBadgeChallenge.Data.Enums.Status.Canceled,2);

            bool updateResult = _globalRepo.UpdateDeliveryInfo(1,updatedDelivery);
            bool expected = true;

            Assert.Equal(expected,updateResult);

        }

        //DELETE
        [Fact]
        public void DeleteDelivery_ShouldRemoveDeliveryCorrectly()
        {
            
            bool actual = _globalRepo.DeleteDelivery(_deliveryA);
            bool expected = true;

            Assert.Equal(expected, actual);

        }

    }
}