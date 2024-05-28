using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
using System;
using System.Collections.Generic;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class SupplierController
    {
        public bool RegisterSupplier(Supplier supplier)
        {
            SupplierDAO supplierDAO = new SupplierDAO();
            supplier.status = Constants.ACTIVE_STATUS;
            return supplierDAO.AddSupplier(supplier);
        }

        public bool ValidateSupplyAreas(ICollection<SupplyArea> supplyAreas)
        {
            bool isValid = true;
            if (supplyAreas.Count == 0)
            {
                isValid = false;
            }
            return isValid;
        }

        public bool ValidateCompanyName(string companyName)
        {
            bool isValid = true;

            if (!Validations.IsCompanyNameValid(companyName))
            {
                isValid = false;
            }
            return isValid;
        }

        public bool ValidatePhone(string phone)
        {
            bool isValid = true;
            if (!Validations.IsPhoneValid(phone))
            {
                isValid = false;
            }
            return isValid;
        }

        public bool ValidateEmail(string email)
        {
            bool isValid = true;

            if (!Validations.IsEmailValid(email))
            {
                isValid = false;
            }
            return isValid;
        }

        public bool ValidateManagerName(string manager)
        {
            bool isValid = true;

            if (!Validations.IsNameValid(manager))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool IsEmailDuplicated(string email)
        {
            SupplierDAO supplierDAO = new SupplierDAO();
            bool isDuplicated = supplierDAO.IsEmailSupplierExisting(email);
            if (isDuplicated)
            {
                DialogManager.ShowWarningMessageBox("El email ingresado ya existe en el sistema, verifique que no esté duplicando al proveedor.");
            }
            return isDuplicated;
        }

        public bool IsNewSupplierValid(Supplier supplier)
        {
            bool managerDataValid = ValidateManagerName(supplier.manager);
            bool emailDataValid = ValidateEmail(supplier.email);
            bool phoneDataValid = ValidatePhone(supplier.phone);
            bool companyDataValid = ValidateCompanyName(supplier.companyName);
            bool supplyAreasValid = ValidateSupplyAreas(supplier.SupplyAreas);

            return managerDataValid && emailDataValid && phoneDataValid && companyDataValid && supplyAreasValid;
        }
    }
}