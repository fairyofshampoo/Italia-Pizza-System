//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ItaliaPizzaServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class HomeOrderProduct
    {
        public int OrderProductId { get; set; }
        public int amount { get; set; }
        public int homeOrderId { get; set; }
        public string productId { get; set; }
        public int addressId { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual HouseOrder HouseOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
