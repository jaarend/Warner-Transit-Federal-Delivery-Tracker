using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data;

namespace GoldBadgeChallenge.Repository.ItemRepository
{
    public class ItemRepo
    {
        private readonly List<Item> _itemDbContext = new List<Item>();
        private int _count = 0;

        //* ADD/CREATE
        public bool CreateItem(Item item)
        {
            if (item is null)
            {
                return false;
            }
            else
            {
                _count++;
                item.Id = _count;
                _itemDbContext.Add(item);
                return true;
            }
        }

        //READ/GET ITEMS

        public List<Item> GetItems()
        {
            return _itemDbContext;
        }

        public Item GetItemById(int id)
        {
            return _itemDbContext.FirstOrDefault(x => x.Id == id)!;
        }

    }
}