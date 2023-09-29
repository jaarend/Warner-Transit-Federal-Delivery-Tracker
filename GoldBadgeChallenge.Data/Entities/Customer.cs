using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldBadgeChallenge.Data
{
    public class Customer
    {
        public Customer() { }

        public Customer(int id, string firstName, string lastName, string address)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
    }
}