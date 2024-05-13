using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.ApplicationLayer
{
    public static class Constants
    {
        public const int ACTIVE_STATUS = 1;

        public const int INACTIVE_STATUS = 0;

        public const int COMPLETE_STATUS = 2;

        public const int EXTERNAL_PRODUCT = 1;

        public const int INTERNAL_PRODUCT = 0;

        public const int SUCCESSFUL_RESULT = 1;

        public const int UNSUCCESSFUL_RESULT = 0;

        public const int EXCEPTION_RESULT = -1;

        public const string CHEF_ROLE = "Cocinero";

        public const string CASHIER_ROLE = "Cajero";

        public const string WAITER_ROLE = "Mesero";

        public const string MANAGER_ROLE = "Gerente";
    }
}
