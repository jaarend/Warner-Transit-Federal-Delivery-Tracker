using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GoldBadgeChallenge.Data
{
    public class Item
    {
        public Item() {}

        public Item (int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public int Id {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}

        //add a foreign key to tie into delivery
        public int DeliveryId {get; set;}
    }
}