using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.ViewModels
{
    public class ProductItem
    {
        public int Quantity { get; set; }

        public decimal Total { get; set; }

        public string ProductName { get; set; }

        public decimal price { get; set; }

        public Guid UniqueId { get; set; }

    }
}