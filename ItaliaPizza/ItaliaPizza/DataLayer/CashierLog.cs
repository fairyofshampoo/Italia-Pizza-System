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
    
    public partial class CashierLog
    {
        public int logId { get; set; }
        public System.DateTime creationDate { get; set; }
        public System.TimeSpan creationTime { get; set; }
        public string employeeId { get; set; }
        public byte[] report { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
