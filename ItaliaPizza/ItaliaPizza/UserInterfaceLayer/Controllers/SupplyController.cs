using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class SupplyController
    {
        public bool AddSupply(Supply supply)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            return supplyDAO.AddSupply(supply);
        }

        public bool ModifySupply(Supply supply, string name)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            return supplyDAO.ModifySupply(supply, name);
        }

        public bool ChangeSupplyStatus(Supply supply, string status)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            return supplyDAO.ModifySupply(supply, status);
        }

        public bool IsSupplyNameDuplicated(string name)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            return supplyDAO.IsSupplyNameExisting(name);
        }

        public bool ValidateSupplyName(string name)
        {
            bool isValid = true;

            if (!Validations.IsSupplyNameValid(name))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool ValidateSupplyAmount(decimal amount)
        {
            bool isValid = true;

            if (amount < 0)
            {
                isValid = false;
            }

            return isValid;
        }

        public bool ValidateMeasurementUnit(string measurementUnit)
        {
            bool isValid = true;

            if (measurementUnit == null)
            {
                isValid = false;
            }

            return isValid;
        }

        public bool IsNewSupplyValid(Supply supply)
        {
            bool nameDataValid = ValidateSupplyName(supply.name);
            bool amountDataValid = ValidateSupplyAmount((decimal)supply.amount);
            bool measurementDataValid = ValidateMeasurementUnit(supply.measurementUnit);

            return nameDataValid && amountDataValid && measurementDataValid;
        }
    }
}
