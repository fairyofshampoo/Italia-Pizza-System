﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ItaliaPizzaData.DataLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ItaliaPizzaDBEntities : DbContext
    {
        public ItaliaPizzaDBEntities()
            : base("name=ItaliaPizzaDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<CashierLog> CashierLogs { get; set; }
        public virtual DbSet<Cashout> Cashouts { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ColonyCatalog> ColonyCatalogs { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<InternalOrder> InternalOrders { get; set; }
        public virtual DbSet<InternalOrderProduct> InternalOrderProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeSupply> RecipeSupplies { get; set; }
        public virtual DbSet<StatusOrder> StatusOrders { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplierOrder> SupplierOrders { get; set; }
        public virtual DbSet<Supply> Supplies { get; set; }
        public virtual DbSet<SupplyArea> SupplyAreas { get; set; }
        public virtual DbSet<SupplyOrder> SupplyOrders { get; set; }
    
        public virtual int CloseCashierLog(Nullable<decimal> finalBalance, Nullable<System.DateTime> closingDate, string employeeEmail)
        {
            var finalBalanceParameter = finalBalance.HasValue ?
                new ObjectParameter("finalBalance", finalBalance) :
                new ObjectParameter("finalBalance", typeof(decimal));
    
            var closingDateParameter = closingDate.HasValue ?
                new ObjectParameter("closingDate", closingDate) :
                new ObjectParameter("closingDate", typeof(System.DateTime));
    
            var employeeEmailParameter = employeeEmail != null ?
                new ObjectParameter("employeeEmail", employeeEmail) :
                new ObjectParameter("employeeEmail", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CloseCashierLog", finalBalanceParameter, closingDateParameter, employeeEmailParameter);
        }
    
        public virtual int ReduceIngredientsV10(string internalOrderCode)
        {
            var internalOrderCodeParameter = internalOrderCode != null ?
                new ObjectParameter("internalOrderCode", internalOrderCode) :
                new ObjectParameter("internalOrderCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ReduceIngredientsV10", internalOrderCodeParameter);
        }
    }
}