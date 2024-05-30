using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
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
        SupplierOrderController _supplierOrderController = new SupplierOrderController();
        private Supplier supplierData;
        private Dictionary<string, SupplyOrderRemoveUC> suppliesDictionary = new Dictionary<string, SupplyOrderRemoveUC>();
        private List<Supply> suppliesInDB = new List<Supply>();
        public bool isAnUpdate;
        public int OrderId { get; set; }
        public SupplyOrderView()
        {
            InitializeComponent();
            GetSupplies();
        }

        public void SetSupplyOrderData(SupplierOrder supplierOrder, bool isInReadMode)
        {
            this.isAnUpdate = isInReadMode;
            SetSupplier(supplierOrder.Supplier);
            SetResumeOrderTitle();

            if (isInReadMode)
            {
                OrderId = supplierOrder.orderCode;
                txtTotalPayment.Text = supplierOrder.total.ToString();
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
            suppliesInDB = _supplierOrderController.GetSuppliesInOrder(OrderId);
            foreach (Supply supply in suppliesInDB)
            {
                supply.amount = _supplierOrderController.GetAmountOfSupplyInOrder(supply.name, OrderId);
                AddSupplyToResume(supply);
            }
        }

        private void AddSupplyToResume(Supply supply)
        {
            if (!suppliesDictionary.ContainsKey(supply.name))
            {
                SupplyOrderRemoveUC supplyCard = new SupplyOrderRemoveUC();
                suppliesDictionary.Add(supply.name, supplyCard);
                supplyCard.SupplyOrderView = this;
                supplyCard.SetSupplyData(supply);
                orderListView.Items.Add(supplyCard);
            }
        }

        public void RemoveSupplyFromOrder(string supplyName, SupplyOrderRemoveUC supplyCard)
        {
            suppliesDictionary.Remove(supplyName);
            orderListView.Items.Remove(supplyCard);
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
                if (isAnUpdate)
                {
                    if (UpdateOrder())
                    {
                        DialogManager.ShowSuccessMessageBox("Pedido de insumos actualizado exitosamente");
                        NavigationService.GoBack();
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("No se podido actualizar el pedido de insumos. Intente nuevamente");
                    }

                } else
                {
                    if (RegisterOrder())
                    {
                        DialogManager.ShowSuccessMessageBox("Pedido de insumos registrado exitosamente");
                        NavigationService.GoBack();
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("No se podido registrar el pedido de insumos. Intente nuevamente");
                    }
                }
            }
        }

        private bool UpdateOrder()
        {
            bool result = true;

            foreach (string item in suppliesDictionary.Keys)
            {

                decimal amountDecimal = (decimal)suppliesDictionary[item].supplyData.amount;

                if (_supplierOrderController.UpdateSuppliesInOrder(item, OrderId, amountDecimal))
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                result = UpdateRemovingSupplies() && RegisterPayment();
            }

            return result;
        }

        private bool UpdateRemovingSupplies()
        {
            bool result = true;

            foreach (Supply supplyDB in suppliesInDB)
            {
                string itemName = supplyDB.name;

                if (!suppliesDictionary.ContainsKey(itemName))
                {
                    if(!_supplierOrderController.UpdateRemovingSuppliesInOrder(itemName, OrderId))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
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

        private bool RegisterPayment()
        {
            bool result = true;

            if (decimal.TryParse(txtTotalPayment.Text, out decimal totalPayment))
            {
                if (!_supplierOrderController.RegisterPayment(totalPayment, OrderId))
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        private bool AreSuppliesInOrder()
        {
            bool result = true;
            if (!suppliesDictionary.Any())
            {
                result = false;
                DialogManager.ShowWarningMessageBox("No ha ingresado insumos a su pedido. Intente nuevamente");
            }
            return result;
        }

        private bool IsTotalPaymentValid()
        {
            bool result = true;
            if (!_supplierOrderController.ValidateTotalPayment(txtTotalPayment.Text))
            {
                lblTotalPayment.Foreground = Brushes.Red;
                result = false;
            }

            return result;
        }

        private bool RegisterOrder()
        {
            bool result = true;

            foreach (string item in suppliesDictionary.Keys)
            {
                decimal amountDecimal = (decimal)suppliesDictionary[item].supplyData.amount;
                if (!_supplierOrderController.AddSupplyToOrder(item, OrderId, amountDecimal))
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                result = RegisterPayment();
            }

            return result;
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelOrder();
        }

        private void CancelOrder()
        {
            if (isAnUpdate)
            {
                NavigationService.GoBack();
            } else
            {
                if (_supplierOrderController.DeleteSupplierOrder(OrderId))
                {
                    NavigationService.GoBack();
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("No se pudo cancelar la acción, intente nuevamente");
                }
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
            if (suppliesDictionary.ContainsKey(supply.name))
            {
                supply.amount = suppliesDictionary[supply.name].supplyData.amount;
                IncreaseAmount(supply);
            } else
            {
                decimal defaultAmount = 1;
                supply.amount = defaultAmount;
                AddSupplyToResume(supply);
            }

        }

        private void IncreaseAmount(Supply supply)
        {
            SupplyOrderRemoveUC supplyCard = new SupplyOrderRemoveUC();
            supplyCard = suppliesDictionary[supply.name];
            supplyCard.supplyData.amount = supply.amount;
            supply.amount += 1;
            supplyCard.SetSupplyData(supply);
        }

        private void GenerateSupplierOrder()
        {
            int result = _supplierOrderController.CreateSupplierOrder(supplierData.email);
            if(result != Constants.UNSUCCESSFUL_RESULT || result != Constants.EXCEPTION_RESULT)
            {
                this.OrderId = result;
            } else
            {
                DialogManager.ShowDataBaseErrorMessageBox();
            }
        }

        private void TxtTotalPaymentChanged(object sender, TextChangedEventArgs e)
        {
            lblTotalPayment.Foreground = Brushes.Black;
        }
    }
}
