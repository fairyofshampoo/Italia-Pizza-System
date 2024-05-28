using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaTest
{
    /// <summary>
    /// Summary description for RegisterSupplierTests
    /// </summary>
    /// 
    [TestClass]
    public class RegisterSupplierTests
    {
        private readonly SupplierController _supplierController = new SupplierController();

        [TestMethod]
        public void AddSupplier_ValidData_Success()
        {
            // Arrange
            var supplier = new Supplier
            {
                companyName = "PepsiCo",
                manager = "Maria Gonzalez",
                email = "maria.gonzalez@pepsico.com",
                phone = "5551234567",
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Bebidas" } }
            };


            // Act
            bool result = false;
            if (_supplierController.IsNewSupplierValid(supplier) && !_supplierController.IsEmailDuplicated(supplier.email))
            {
                if (_supplierController.RegisterSupplier(supplier))
                {
                    result = true;
                }
            }

            // Assert
            Assert.IsTrue(result, "El registro del proveedor debería ser exitoso con datos válidos.");
        }

        [TestMethod]
        public void AddSupplier_InvalidData_Failure()
        {
            // Arrange
            var invalidSupplier = new Supplier
            {
                companyName = "",  // Invalid company name
                manager = "J",  // Invalid manager name
                email = "invalidEmail",  // Invalid email format
                phone = "123abc",  // Invalid phone format
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea>()  // No supply areas selected
            };

            // Act
            bool result = false;
            if (_supplierController.IsNewSupplierValid(invalidSupplier) && !_supplierController.IsEmailDuplicated(invalidSupplier.email))
            {
                if (_supplierController.RegisterSupplier(invalidSupplier))
                {
                    result = true;
                }
            }

            // Assert
            Assert.IsFalse(result, "El registro del proveedor no debería ser exitoso con datos inválidos.");
        }

        [TestMethod]
        public void AddSupplier_DuplicateSupplier_Failure()
        {
            // Arrange
            var supplier = new Supplier
            {
                companyName = "Coca Cola",
                manager = "Juan Gutiérrez García",
                email = "juanggarcia@cocacola.com",
                phone = "2289034960",
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Verduras" } }
            };

            // Act
            bool result = false;
            if (_supplierController.IsNewSupplierValid(supplier) && !_supplierController.IsEmailDuplicated(supplier.email))
            {
                if (_supplierController.RegisterSupplier(supplier))
                {
                    result = true;
                }
            }

            // Assert
            Assert.IsFalse(result, "El registro del proveedor no debería ser exitoso si el proveedor ya existe.");
        }
    }
}
