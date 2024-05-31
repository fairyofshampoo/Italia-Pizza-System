using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class ProductController
    {
        public bool AddProductExternal(Product product, Supply supply)
        {
            ProductDAO productDAO = new ProductDAO();
            return productDAO.AddProductExternal(product, supply);
        }

        public bool ModifyProduct(Product product, string code)
        {
            ProductDAO productDAO = new ProductDAO();
            return productDAO.ModifyProduct(product, code);
        }

        public bool ChangeProductStatus(Product product, int status) 
        {
            ProductDAO productDAO = new ProductDAO();
            return productDAO.ChangeStatus(product, status);
        }

        public bool IsProductCodeDuplicated(string code)
        {
            ProductDAO productDAO = new ProductDAO();
            return productDAO.IsCodeExisting(code);
        }

        public bool ValidateProductName(string productName)
        {
            bool isValid = true;

            if (!Validations.IsProductNameValid(productName))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool ValidateProductCode(string productCode)
        {
            bool isValid = true;

            if (!Validations.IsProductCodeValid(productCode))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool ValidateDescription(string description)
        {
            bool isValid = true;

            if (description ==  null)
            {
                isValid = false;
            }

            return isValid;
        }

        public bool ValidateAmountProduct(int amount)
        {
            bool isValid = true;

            if (amount <= 0) 
            {
                isValid = false; 
            }

            return isValid;
        }

        public bool ValidatePriceProduct(decimal price)
        {
            bool isValid = true;

            if (price <= 0)
            {
                isValid = false;
            }

            return isValid;
        }

        public bool IsNewExternalProductValid(Product product)
        {
            bool nameDataValid = ValidateProductName(product.name);
            bool codeDataValid = ValidateProductCode(product.productCode);
            bool descriptionDataValid = ValidateDescription(product.description);
            bool amountDataValid = ValidateAmountProduct((int)product.amount);
            bool priceDataValid = ValidatePriceProduct((int)product.price);

            return nameDataValid && codeDataValid && descriptionDataValid && 
                   amountDataValid && priceDataValid;
        }
    }
}
