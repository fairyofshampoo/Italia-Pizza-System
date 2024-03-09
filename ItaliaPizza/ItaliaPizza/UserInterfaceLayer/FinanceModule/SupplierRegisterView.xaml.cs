using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    public partial class SupplierRegisterView : Page
    {
        public ObservableCollection<SupplyAreaItem> SupplyAreaItems { get; set; }

        public SupplierRegisterView()
        {
            InitializeComponent();
            SetSupplyAreaItems();
        }

        private void SetSupplyAreaItems()
        {
            SupplyAreaItems = new ObservableCollection<SupplyAreaItem>();

            // Añadir algunos elementos de ejemplo
            SupplyAreaItems.Add(new SupplyAreaItem { SupplyAreaItems = "Área de Abastecimiento 1", SelectedSupplyArea = false });
            SupplyAreaItems.Add(new SupplyAreaItem { SupplyAreaItems = "Área de Abastecimiento 2", SelectedSupplyArea = false });

            // Establecer el DataContext del ListBox
            SupplyAreaCheckboxList.ItemsSource = SupplyAreaItems;
        }

    }

    public class SupplyAreaItem
    {
        public string SupplyAreaItems { get; set; }
        public bool SelectedSupplyArea { get; set; }
    }
}
