using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using ItaliaPizza.ApplicationLayer;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    public partial class SupplierRegisterView : Page
    {
        public ObservableCollection<SupplyAreaViewModel> SupplyAreas { get; set; }

        public SupplierRegisterView()
        {
            InitializeComponent();
            SetSupplyAreaItems();
        }

        private void SetSupplyAreaItems()
        {
            SupplierAreaDAO supplierAreaDAO = new SupplierAreaDAO();

            List<supplyArea> supplyAreas = supplierAreaDAO.GetAllSupplyAreas();

            SupplyAreas = new ObservableCollection<SupplyAreaViewModel>(
                supplyAreas.Select(area => new SupplyAreaViewModel { AreaName = area.area_name })
            );
            SupplyAreaCheckboxList.ItemsSource = SupplyAreas;
        }
    }
}
