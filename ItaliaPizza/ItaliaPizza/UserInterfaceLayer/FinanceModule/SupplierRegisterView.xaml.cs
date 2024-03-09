using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    public partial class SupplierRegisterView : Page
    {
        public ObservableCollection<supplyArea> SupplyAreas { get; set; }

        public SupplierRegisterView()
        {
            InitializeComponent();
            SetSupplyAreaItems();
        }

        private void SetSupplyAreaItems()
        {
            // Obtener todas las áreas de suministro desde la base de datos
            SupplierAreaDAO supplierAreaDAO = new SupplierAreaDAO(); // Suponiendo que tienes una clase llamada DataAccess con el método GetAllSupplyAreas
            List<supplyArea> supplyAreas = supplierAreaDAO.GetAllSupplyAreas();

            // Crear una ObservableCollection y asignarla como el origen de datos del ListBox
            SupplyAreas = new ObservableCollection<supplyArea>(supplyAreas);
            SupplyAreaCheckboxList.ItemsSource = SupplyAreas;
        }
    }
}
