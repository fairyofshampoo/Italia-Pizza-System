//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ItaliaPizza.DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class HomeOrderProduct
    {
        public int OrderProductId { get; set; }
        public int amount { get; set; }
        public string homeOrderId { get; set; }
        public byte isConfirmed { get; set; }
        public string productId { get; set; }
        public int addressId { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual HomeOrder HomeOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
