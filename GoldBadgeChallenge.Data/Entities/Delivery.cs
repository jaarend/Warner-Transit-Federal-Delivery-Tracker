using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data.Enums;

namespace GoldBadgeChallenge.Data
{
    public class Delivery
    {
      
        public Delivery() {}

        public Delivery(int id, DateOnly orderDate, DateOnly deliveryDate, Status status, int customerId)
        {
            Id = id;
            OrderDate = orderDate;
            DeliveryDate = deliveryDate;
            OrderStatus = status;
            CustomerId = customerId;
        }


        public int Id { get; set;}
        public DateOnly OrderDate { get; set;}
        public DateOnly DeliveryDate { get; set;}
        public Status OrderStatus { get; set;}

        //list of items
        public List<Item> ItemsInDelivery {get; set;} = new List<Item>();
        //associate a customer to a delivery order
        //Foreign Key to connect to customers
        public int CustomerId {get; set;}
        
    }
}