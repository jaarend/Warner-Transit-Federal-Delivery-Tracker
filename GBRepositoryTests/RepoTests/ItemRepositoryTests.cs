using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data;
using GoldBadgeChallenge.Repository.ItemRepository;
using Xunit;

namespace GBRepositoryTests
{
    public class ItemRepositoryTests
    {
        private ItemRepo _globalRepo;

        private Item _item1;
        private Item _item2;
        private Item _item3;

        public ItemRepositoryTests()
        {
            _globalRepo = new ItemRepo();
            
            _item1 = new Item(1,"Stakeboard","You can play with this in a park");
            _item2 = new Item(2,"Shoes","You can use this.");
            _item3 = new Item(3,"Shirt","You can wear this.");

        }

        [Fact]
        public void GetItemById_ShouldGetCorrectItemInfo()
        {
            Item expected = _item1;

            Item actual = _globalRepo.GetItemById(1);

            Assert.Equal(expected, actual);
        }
    }
}