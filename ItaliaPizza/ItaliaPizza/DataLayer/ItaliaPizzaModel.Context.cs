﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
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
    
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Cashin> Cashin { get; set; }
        public virtual DbSet<Cashout> Cashout { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Dealer> Dealer { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<HomeOrderProduct> HomeOrderProduct { get; set; }
        public virtual DbSet<HouseOrder> HouseOrder { get; set; }
        public virtual DbSet<InternalOrder> InternalOrder { get; set; }
        public virtual DbSet<InternalOrderProduct> InternalOrderProduct { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<RecipeSupply> RecipeSupply { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<SupplierOrder> SupplierOrder { get; set; }
        public virtual DbSet<Supply> Supply { get; set; }
        public virtual DbSet<SupplyOrder> SupplyOrder { get; set; }
    }
}