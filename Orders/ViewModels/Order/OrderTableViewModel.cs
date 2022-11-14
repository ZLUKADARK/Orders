using System;
using System.Diagnostics.CodeAnalysis;

namespace Orders.ViewModels.Orders
{
    public class OrderTableViewModel : IEquatable<OrderTableViewModel>
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            OrderTableViewModel objAsOrderTableViewModel = obj as OrderTableViewModel;
            if (objAsOrderTableViewModel == null) return false;
            else return Equals(objAsOrderTableViewModel);
        }
        public bool Equals([AllowNull] OrderTableViewModel other)
        {
            if (other == null) return false;
            return (this.OrderItemId.Equals(other.OrderItemId));
        }
        public override int GetHashCode()
        {
            return OrderItemId;
        }
    }                                                                        
}                                                               
                                                    