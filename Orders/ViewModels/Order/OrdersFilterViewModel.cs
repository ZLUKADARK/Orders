using System;

namespace Orders.ViewModels.Order
{
    public class OrdersFilterViewModel
    {
        public DateTime DateNow { get; set; }
        public DateTime DatePast { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Number { get; set; }
        public string ProviderName { get; set; }
    }
}
