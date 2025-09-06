using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DE_Store_Manager_Client
{
    public class stockItem
    {

        public string name
        {
            get;
            set;
        }
        public int stock
        {
            get;
            set;
        }

        public stockItem (string itemName, int itemQuantity)
        {
            name = itemName;
            stock = itemQuantity;
        }


    }
}
