using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data;
using GoldBadgeChallenge.Data.Enums;

namespace GoldBadgeChallenge.Repository.DeliveryRepository
{
    public class DeliveryRepo
    {
        private readonly List<Delivery> _deliveryDbContext = new List<Delivery>();
        private readonly List<Item> _itemDbContext = new List<Item>();
        private int _count = 0;

        //* ADD/CREATE
        public bool CreateDelivery(Delivery delivery)
        {
            if (delivery is null)
            {
                return false;
            }
            else
            {
                _count++;
                delivery.Id = _count;
                _deliveryDbContext.Add(delivery);
                return true;
            }
        }

        //* READ/GET

        public List<Delivery> GetDeliveries()
        {
            return _deliveryDbContext;
        }

        public List<Item> GetItemList()
        {
            return _itemDbContext;
        }

        public Delivery GetDeliveryById(int id)
        {
            return _deliveryDbContext.FirstOrDefault(x => x.Id == id)!;
        }

        public List<Delivery> GetDeliveryByStatus(Status status)
        {
            return _deliveryDbContext.Where(devlivery => devlivery.OrderStatus == status).ToList();
        }

        public int CountItemInDelivery(Delivery delivery, Item item)
        {
            int itemCount = 0;
            foreach (var deliveryItem in delivery.ItemsInDelivery)
            {
                if (deliveryItem.Id == item.Id)
                {
                    itemCount++;
                }
            }
            return itemCount;
        }

        //* UPDATE

        public bool AddItemToDeliveryList(int deliveryId, Item item)
        {
            if (deliveryId > 0 && item != null)
            {
                Delivery delivery = GetDeliveryById(deliveryId);
                if (delivery != null)
                {
                    item.DeliveryId = delivery.Id;
                    delivery.ItemsInDelivery.Add(item);
                    return true;
                }
            }
            return false;
        }
        public bool RemoveItemFromDeliveryList(int deliveryId, Item item)
        {
            if (deliveryId > 0 && item != null)
            {
                Delivery delivery = GetDeliveryById(deliveryId);
                if (delivery != null)
                {
                    item.DeliveryId = delivery.Id;
                    delivery.ItemsInDelivery.Remove(item);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateDeliveryInfo(int deliveryId, Delivery newDeliveryInfo)
        {
            Delivery deliveryInDb = GetDeliveryById(deliveryId);
            if (deliveryInDb is not null)
            {
                deliveryInDb!.DeliveryDate = newDeliveryInfo.DeliveryDate;
                deliveryInDb!.OrderStatus = newDeliveryInfo.OrderStatus;

                return true;
            }
            return false;
        }

        //* DELETE

        public bool DeleteDelivery(Delivery delivery)
        {
            return _deliveryDbContext.Remove(delivery);
        }
    }
}