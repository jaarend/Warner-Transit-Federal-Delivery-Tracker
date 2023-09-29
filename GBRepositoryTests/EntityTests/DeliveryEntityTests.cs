using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data;
using GoldBadgeChallenge.Data.Enums;
using GoldBadgeChallenge.Repository.DeliveryRepository;
using Xunit;

namespace GBRepositoryTests
{
    public class DeliveryEntityTests
    {
        [Fact]
        public void SetOrderDate_ShouldReturnCorrectDate()
        {
            DateOnly expectedDate = new DateOnly(2023,09,27);
            Delivery delivery = new Delivery();

            delivery.OrderDate = expectedDate;
            Assert.Equal(expectedDate, delivery.OrderDate);
        }
        
        [Fact]
        public void SetItemName_ShouldReturnCorrectName()
        {
            Item item = new Item
            {
                Name = "Plane"
            };

            string expected = "Plane";
            string actual = item.Name;

            Assert.Equal(expected,actual);
        }
        
        [Theory]
        [InlineData(Status.Scheduled)]
        public void SetOrderStatus_ShouldReturnCorrectStatus(Status status)
        {
            //Arrange
            DateOnly fillerDate = new DateOnly(2023,09,27);
            Delivery delivery = new Delivery(1,fillerDate,fillerDate,status,1);
            
            //Act
            int actual = (int)delivery.OrderStatus;
            int expected = 1;

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}