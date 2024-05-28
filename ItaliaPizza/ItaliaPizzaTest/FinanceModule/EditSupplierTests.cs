using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaTest.FinanceModule
{
    [TestClass]
    public class EditSupplierTests
    {
        private readonly SupplierController _supplierController = new SupplierController();

        [TestMethod]
        public void EditSupplier_ValidData_Success()
        {
            // Arrange
            var supplier = new Supplier
            {
                companyName = "Carnicería Lupe",
                manager = "Guadalupe Gonzalez",
                email = "guadalupeggonz@gmail.com",
                phone = "5551234567",
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Carnes frías" } }
            };

            // Register supplier first
            bool initialResult = _supplierController.RegisterSupplier(supplier);
            Assert.IsTrue(initialResult, "El registro inicial del proveedor debería ser exitoso.");

            // Act - Simulate edit
            supplier.phone = "5557654321"; // Changed phone number
            bool editResult = _supplierController.UpdateSupplier(supplier, supplier.email);

            // Assert
            Assert.IsTrue(editResult, "La modificación del proveedor debería ser exitosa con datos válidos.");
        }

        [TestMethod]
        public void EditSupplier_InvalidData_Fail()
        {
            // Arrange
            var supplier = new Supplier
            {
                companyName = "Frutería El Paraíso",
                manager = "Ana Ramirez",
                email = "anita@fruteriaparaiso.com",
                phone = "5512345678",
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Verduras" } }
            };

            // Register supplier first
            bool initialResult = _supplierController.RegisterSupplier(supplier);
            Assert.IsTrue(initialResult, "El registro inicial del proveedor debería ser exitoso.");

            // Act - Simulate edit with invalid email
            supplier.email = "invalidemail";
            bool editResult = _supplierController.UpdateSupplier(supplier, supplier.email);

            // Assert
            Assert.IsFalse(editResult, "La modificación del proveedor debería fallar con datos inválidos.");
        }


        [TestMethod]
        public void DeactivateSupplier_Success()
        {
            // Arrange
            var supplier = new Supplier
            {
                companyName = "Lacteos S.A.",
                manager = "Juan Perez",
                email = "juanperez@lacteossa.com",
                phone = "5559876543",
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Lácteos" } }
            };

            // Register supplier first
            bool initialResult = _supplierController.RegisterSupplier(supplier);
            Assert.IsTrue(initialResult, "El registro inicial del proveedor debería ser exitoso.");

            // Act - Simulate deactivate
            bool deactivateResult = _supplierController.UpdateSupplierStatus(supplier.email, Constants.INACTIVE_STATUS);

            // Assert
            Assert.IsTrue(deactivateResult, "La desactivación del proveedor debería ser exitosa.");
        }

        [TestMethod]
        public void ActivateSupplier_Success()
        {
            // Arrange
            var supplier = new Supplier
            {
                companyName = "Especias del Mundo",
                manager = "Ana Ramirez",
                email = "ana@especiasdelmundo.com",
                phone = "5558765432",
                status = Constants.INACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Especias" } }
            };

            // Register supplier first as inactive
            bool initialRegisterResult = _supplierController.RegisterSupplier(supplier);
            Assert.IsTrue(initialRegisterResult, "El registro inicial del proveedor debería ser exitoso.");

            // Act - Simulate activation
            bool activateResult = _supplierController.UpdateSupplierStatus(supplier.email, Constants.ACTIVE_STATUS);

            // Assert
            Assert.IsTrue(activateResult, "La activación del proveedor debería ser exitosa.");
        }

    }
}