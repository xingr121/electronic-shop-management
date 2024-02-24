using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopManagement
{
    public partial class Product
    {
       

        public Product(string prodName, string prodCategory, int prodQty, decimal prodPrice)
        {
            ProdName = prodName;
            ProdCategory = prodCategory;
            ProdQty = prodQty;
            ProdPrice = prodPrice;
        }
    }

}
