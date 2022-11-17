using System.ComponentModel.DataAnnotations;

namespace Orders.ViewModels.OrderItem
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Количество")]
        public decimal Quantity { get; set; }
        [Required]
        [Display(Name = "Единица измерения")]
        public string Unit { get; set; }
        public string OrderNumber { get; set; }
        [Required]
        [Display(Name = "Номер заказа")]
        public int OrderId { get; set; }
    }
}
