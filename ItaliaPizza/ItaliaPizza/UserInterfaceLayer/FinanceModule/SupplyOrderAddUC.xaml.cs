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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplyOrderAddUC.xaml
    /// </summary>
    public partial class SupplyOrderAddUC : UserControl
    {
        public SupplyOrderView SupplyOrderView { get; set; }
        private Supply supplyData;
        public SupplyOrderAddUC()
        {
            InitializeComponent();
        }
        public void SetTitleData()
        {
            int fontSize = 25;
            txtAmount.FontWeight = FontWeights.Bold;
            txtName.FontWeight = FontWeights.Bold;
            txtSupplyArea.FontWeight = FontWeights.Bold;
            txtUnit.FontWeight = FontWeights.Bold;
            txtAmount.FontSize = fontSize;
            txtName.FontSize = fontSize;
            txtSupplyArea.FontSize = fontSize;
            txtUnit.FontSize = fontSize;
            btnAddSupply.Visibility = Visibility.Hidden;
        }
        public void SetSupplyData(Supply supply)
        {
            supplyData = supply;
            txtName.Text = supply.name;
            txtAmount.Text = supply.amount.ToString();
            txtSupplyArea.Text = supply.SupplyArea.area_name;
            txtUnit.Text = supply.measurementUnit;
        }

        private void BtnAddSupply_Click(object sender, RoutedEventArgs e)
        {
            if (IsSupplyRegistered(this.supplyData.name))
            {
                IncreaseAmount();
            }
            else
            {
                AddSupply();
            }

        }

        private void IncreaseAmount()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            bool result = supplyOrderDAO.IncreaseSupplyAmountInOrder(supplyData, this.SupplyOrderView.OrderId);
            if (result)
            {
                this.SupplyOrderView.IncreaseAmount(supplyData);
            }
        }

        private void AddSupply()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            string supplyName = supplyData.name;
            decimal defaultAmount = 1;
            bool result = supplyOrderDAO.AddSupplyToOrder(supplyName, this.SupplyOrderView.OrderId, defaultAmount);
            if (result)
            {
                this.SupplyOrderView.AddSupplyToOrder(supplyData);
            }
        }

        private bool IsSupplyRegistered(string supplyName)
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            bool isRegister = supplyOrderDAO.IsSupplyAlreadyInOrder(supplyName, this.SupplyOrderView.OrderId);
            return isRegister;
        }
    }
}
