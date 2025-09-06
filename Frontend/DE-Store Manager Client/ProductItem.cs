using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DE_Store_Manager_Client
{

    public class productItem
    {
        public int id
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public float price
        {
            get;
            set;
        }
        public string itemSale
        {
            get;
            set;
        }

        public productItem(int itemId, string itemName, float itemPrice, string Sale)
        {
            id = itemId;
            name = itemName;
            price = itemPrice;
            itemSale = Sale;
        }

    }
}
