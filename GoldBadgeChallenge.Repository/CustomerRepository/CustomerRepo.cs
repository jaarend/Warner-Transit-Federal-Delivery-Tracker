using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data;

namespace GoldBadgeChallenge.Repository.CustomerRepository
{
    public class CustomerRepo
    {
        private readonly List<Customer> _customerDbContext = new List<Customer>();
        private int _count = 0;

        //* ADD/CREATE
        public bool CreateCustomer(Customer customer)
        {
            if (customer is null)
            {
                return false;
            }
            else
            {
                _count++;
                customer.Id = _count;
                _customerDbContext.Add(customer);
                return true;
            }
        }

        public List<Customer> GetCustomers()
        {
            return _customerDbContext;
        }

        public Customer GetCustomerById(int id)
        {
            return _customerDbContext.FirstOrDefault(x => x.Id == id)!;
        }
    }
}