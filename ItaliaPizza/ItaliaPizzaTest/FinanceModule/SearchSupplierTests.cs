using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ItaliaPizzaTest.FinanceModule
{
    [TestClass]
    public class SearchSupplierTests
    {
        private readonly SupplierController _supplierController = new SupplierController();

        [TestMethod]
        public void SearchSupplierByName_ValidSearch_ReturnsSuppliers()
        {
            // Arrange
            string searchText = "Maria Gonzalez";

            // Insert test data
            var supplier = new Supplier
            {
                companyName = "PepsiCo",
                manager = "Maria Gonzalez",
                email = "maria.gonzalez@pepsico.com",
                phone = "5557654321",
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Bebidas" } }
            };
            _supplierController.RegisterSupplier(supplier);

            // Act
            List<Supplier> suppliers = _supplierController.SearchSupplierByName(searchText);

            // Assert
            Assert.IsNotNull(suppliers, "La lista de proveedores no debería ser nula.");
            Assert.IsTrue(suppliers.Count > 0, "Debería haber al menos un proveedor que coincida con el nombre especificado.");
        }

        [TestMethod]
        public void SearchSupplierByName_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            string searchText = "NombreInexistente";

            // Act
            List<Supplier> suppliers = _supplierController.SearchSupplierByName(searchText);

            // Assert
            Assert.AreEqual(0, suppliers.Count, "La lista de proveedores debería estar vacía cuando no hay coincidencias en la búsqueda.");
        }

        [TestMethod]
        public void SearchSupplierByArea_ValidSearch_ReturnsSuppliers()
        {
            // Arrange
            var supplyArea = new SupplyArea { area_name = "Lácteos" };

            var supplier1 = new Supplier
            {
                companyName = "Distribuidora Lácteos",
                manager = "Carlos Lopez",
                email = "carlos.lopez@lacteos.com",
                phone = "5559876543",
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Lácteos" } }
            };
            var supplier2 = new Supplier
            {
                companyName = "Quesos de Oro",
                manager = "Ana Perez",
                email = "ana.perez@quesosdeoro.com",
                phone = "5551234567",
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Lácteos" } }
            };
            _supplierController.RegisterSupplier(supplier1);
            _supplierController.RegisterSupplier(supplier2);

            // Act
            List<Supplier> suppliers = _supplierController.SearchSupplierByArea(supplyArea);

            // Assert
            Assert.IsNotNull(suppliers, "La lista de proveedores no debería ser nula.");
            Assert.IsTrue(suppliers.Count > 0, "Debería haber al menos un proveedor que coincida con el área de suministro especificada.");
            Assert.IsTrue(suppliers.Exists(s => s.companyName == "Distribuidora Lácteos"), "La lista de proveedores debería incluir 'Distribuidora Lácteos'.");
            Assert.IsTrue(suppliers.Exists(s => s.companyName == "Quesos de Oro"), "La lista de proveedores debería incluir 'Quesos de Oro'.");
        }

        [TestMethod]
        public void SearchSupplierByArea_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            var supplyArea = new SupplyArea { area_name = "Producto externo" };

            // Ensure there are suppliers in the database
            var supplier = new Supplier
            {
                companyName = "Carnicería Lupe",
                manager = "Guadalupe Gonzalez",
                email = "guadalupeggonz@gmail.com",
                phone = "5551234567",
                status = Constants.ACTIVE_STATUS,
                SupplyAreas = new List<SupplyArea> { new SupplyArea { area_name = "Carnes frías" } }
            };
            _supplierController.RegisterSupplier(supplier);

            // Act
            List<Supplier> suppliers = _supplierController.SearchSupplierByArea(supplyArea);

            // Assert
            Assert.AreEqual(0, suppliers.Count, "La lista de proveedores debería estar vacía cuando no hay coincidencias en la búsqueda por área de suministro.");
        }
    }
}
