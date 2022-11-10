using System;

namespace Orders.ViewModels.Orders
{
    public class OrderTableViewModel
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
    }
}
