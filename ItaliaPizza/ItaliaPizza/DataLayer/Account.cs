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
    
    public partial class Account
    {
        public string user { get; set; }
        public string password { get; set; }
        public byte status { get; set; }
        public string email { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
