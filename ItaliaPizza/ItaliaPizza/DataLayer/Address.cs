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
    
    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            this.HomeOrderProduct = new HashSet<HomeOrderProduct>();
        }
    
        public int addressId { get; set; }
        public string street { get; set; }
        public int number { get; set; }
        public string postalCode { get; set; }
        public string colony { get; set; }
        public byte status { get; set; }
        public string clientId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeOrderProduct> HomeOrderProduct { get; set; }
        public virtual Client Client { get; set; }
    }
}