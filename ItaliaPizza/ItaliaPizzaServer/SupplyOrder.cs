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
    
    public partial class SupplyOrder
    {
        public int supplyCode { get; set; }
        public int quantityOrdered { get; set; }
        public string supplyId { get; set; }
        public int supplierOrderId { get; set; }
    
        public virtual SupplierOrder SupplierOrder { get; set; }
        public virtual Supply Supply { get; set; }
    }
}
