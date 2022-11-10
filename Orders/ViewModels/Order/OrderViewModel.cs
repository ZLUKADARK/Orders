using System;

namespace Orders.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public int ProviderId { get; set; }
    }
}
