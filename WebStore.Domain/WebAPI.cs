using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain
{
    public static class WebAPI
    {

        public const string APIName = "WebStore.API";

        public const string Employees = "api/employees";
        public const string Products = "api/products";
        public const string Orders = "api/orders";

        public const string MediaTypeJSON = "application/json";

        public static class Identity
        {
            public const string Users = "api/users";
            public const string Roles = "api/roles";
        }

    }
}
