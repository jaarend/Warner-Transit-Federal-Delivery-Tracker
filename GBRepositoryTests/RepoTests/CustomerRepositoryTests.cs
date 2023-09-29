using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data;
using GoldBadgeChallenge.Repository.CustomerRepository;
using Xunit;

namespace GBRepositoryTests.RepoTests
{
    public class CustomerRepositoryTests
    {
        private CustomerRepo _globalRepo;

        private Customer _customer1;
        private Customer _customer2;
        private Customer _customer3;

        public CustomerRepositoryTests()
        {
            _globalRepo = new CustomerRepo();
            
            _customer1 = new Customer(1,"John","Smith","1234 State Street, Chicago, IL");
            _customer2 = new Customer(2,"Ashley","Smith","1235 State Street, Chicago, IL");
            _customer3 = new Customer(3,"David","Smith","1236 State Street, Chicago, IL");

        }

        [Fact]
        public void GetItemById_ShouldGetCorrectItemInfo()
        {
            Customer expected = _customer1;

            Customer actual = _globalRepo.GetCustomerById(1);

            Assert.Equal(expected, actual);
        }
    }
}