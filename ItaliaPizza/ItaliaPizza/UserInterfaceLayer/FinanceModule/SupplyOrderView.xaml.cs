using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplyOrder.xaml
    /// </summary>
    public partial class SupplyOrderView : Page
    {
        private Supplier supplierData;
        private readonly Dictionary<string, SupplyOrderRemoveUC> suppliesDictionary = new Dictionary<string, SupplyOrderRemoveUC>();
        private bool isAnUpdate;
        public int OrderId { get; set; }
        public SupplyOrderView()
        {
            InitializeComponent();
            GetSupplies();
        }

        public void SetSupplyOrderData(SupplierOrder supplierOrder, bool isAnUpdate)
        {
            this.isAnUpdate = isAnUpdate;
            SetSupplier(supplierOrder.Supplier);
            SetResumeOrderTitle();

            if (isAnUpdate)
            {
                SetOrderSupplies();
            } else {

                GenerateSupplierOrder();
            }
        }

        private void SetResumeOrderTitle()
        {
            orderListView.Items.Clear();
            SupplyOrderRemoveUC supplyCard = new SupplyOrderRemoveUC();
            supplyCard.SupplyOrderView = this;
            supplyCard.SetTitleData();
            orderListView.Items.Add(supplyCard);
        }

        private void SetOrderSupplies()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            List<Supply> suppliesInOrder = supplyOrderDAO.GetSuppliesByOrderId(this.OrderId);
            foreach (Supply supply in suppliesInOrder)
            {
                AddSupplyToResume(supply);
                Console.WriteLine(supply.name);
            }
        }

        private void AddSupplyToResume(Supply supply)
        {
            SupplyOrderRemoveUC supplyCard = new SupplyOrderRemoveUC();
            suppliesDictionary.Add(supply.name, supplyCard);
            supplyCard.SupplyOrderView = this;
            supplyCard.SetSupplyData(supply);
            orderListView.Items.Add(supplyCard);
        }

        private void SetSupplier(Supplier supplier)
        {
            lblSupplierName.Text = supplier.manager + ": " + supplier.companyName;
            StringBuilder supplyAreasText = new StringBuilder();

            foreach (var supplyArea in supplier.SupplyAreas)
            {
                supplyAreasText.Append(supplyArea.area_name);
                supplyAreasText.Append("  ");
            }

            lblSupplyArea.Content = supplyAreasText.ToString();
            this.supplierData = supplier;
        }

        private void GetSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> availableSupplies = supplyDAO.GetSuppliesByStatus(true);

            if(availableSupplies.Count > 0 )
            {
                ShowSupplies(availableSupplies);

            } else
            {
                ShowNoSuppliesMessage();
            }
        }

        private void ShowNoSuppliesMessage()
        {
            suppliesListView.Items.Clear();
            Label lblNoSupplies = new Label();
            lblNoSupplies.Style = (Style)FindResource("NoSuppliesLabelStyle");
            lblNoSupplies.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoSupplies.VerticalAlignment = VerticalAlignment.Center;
            suppliesListView.Items.Add(lblNoSupplies);
        }

        private void ShowSupplies(List<Supply> suppliesList)
        {
            suppliesListView.Items.Clear();
            SupplyOrderAddUC supplyUC = new SupplyOrderAddUC();
            supplyUC.SupplyOrderView = this;
            supplyUC.SetTitleData();
            suppliesListView.Items.Add(supplyUC);
            foreach (Supply supply in suppliesList)
            {
                AddSupplyToList(supply);
            }
        }

        private void AddSupplyToList(Supply supply)
        {
            SupplyOrderAddUC supplyCard = new SupplyOrderAddUC();
            supplyCard.SupplyOrderView = this;
            supplyCard.SetSupplyData(supply);
            suppliesListView.Items.Add(supplyCard);
        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (IsOrderValid())
            {
                if (RegisterOrder())
                {
                    DialogManager.ShowSuccessMessageBox("Pedido de insumos registrado exitosamente");
                    NavigationService.GoBack();
                } else
                {
                    DialogManager.ShowErrorMessageBox("No se podido registrar el pedido de insumos. Intente nuevamente");
                }
            }
        }

        private bool IsOrderValid()
        {
            bool result = false;
            if(IsTotalPaymentValid() && AreSuppliesInOrder())
            {
                result = true;
            }
            return result;
        }

        private bool AreSuppliesInOrder()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            bool result = supplyOrderDAO.AreAnySuppliesInOrder(OrderId);
            if (!result)
            {
                DialogManager.ShowWarningMessageBox("No ha ingresado insumos a su pedido. Intente nuevamente");
            }
            return result;
        }

        private bool IsTotalPaymentValid()
        {
            bool result = true;
            string totalPayment = txtTotalPayment.Text;
            if (!Validations.IsTotalPaymentValid(totalPayment))
            {
                lblTotalPayment.Foreground = Brushes.Red;
                result = false;
            }

            return result;
        }

        private bool RegisterOrder()
        {
            bool result = false;
            if (decimal.TryParse(txtTotalPayment.Text, out decimal totalPayment))
            {
                SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
                result = supplyOrderDAO.UpdateStatusOrderAndPayment(OrderId, Constants.ACTIVE_STATUS, totalPayment);
            }

            return result;
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelOrder();
        }

        private void CancelOrder()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            if (supplyOrderDAO.DeleteSupplierOrder(this.OrderId))
            {
                NavigationService.GoBack();
            } else
            {
                DialogManager.ShowErrorMessageBox("No se pudo cancelar la acción, intente nuevamente");
            }
        }

        private void TxtSearchBarChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchBar.Text;
            if (searchText.Length > 3)
            {
                SearchSupplyByName(searchText);

            } else
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    GetSupplies();
                }
            }
        }

        private void SearchSupplyByName(string searchText)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> supplies = supplyDAO.SearchActiveSupplyByName(searchText);
            ShowSupplies(supplies);
        }

        public void AddSupplyToOrder(Supply supply)
        {
            AddSupplyToResume(supply);
        }

        public void RemoveSupplyFromOrder(SupplyOrderRemoveUC supplyCard)
        {
            orderListView.Items.Remove(supplyCard);
        }

        public void IncreaseAmount(Supply supply)
        {
            if (suppliesDictionary.ContainsKey(supply.name)){
                SupplyOrderRemoveUC supplyCard = new SupplyOrderRemoveUC();
                supplyCard = suppliesDictionary[supply.name];
                supplyCard.SetSupplyData(supply);
            }
        }

        private void GenerateSupplierOrder()
        {
            SupplyOrderDAO supplierDAO = new SupplyOrderDAO();
            int result = supplierDAO.AddSupplierOrder(SetNewSupplierOrder());
            if(result != Constants.UNSUCCESSFUL_RESULT || result != Constants.EXCEPTION_RESULT)
            {
                this.OrderId = result;
            } else
            {
                DialogManager.ShowDataBaseErrorMessageBox();
            }
        }

        private SupplierOrder SetNewSupplierOrder()
        {
            DateTime currentDateTime = DateTime.Now;
            SupplierOrder newSupplierOrder = new SupplierOrder()
            {
                status = Constants.INACTIVE_STATUS,
                date = currentDateTime,
                total = 0,
                modificationDate = currentDateTime,
                supplierId = supplierData.email,
            };

            return newSupplierOrder;
        }

        private void TxtTotalPaymentChanged(object sender, TextChangedEventArgs e)
        {
            lblTotalPayment.Foreground = Brushes.Black;
        }
    }
}
