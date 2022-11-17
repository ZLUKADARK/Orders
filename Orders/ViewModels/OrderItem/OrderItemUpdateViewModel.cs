using System.ComponentModel.DataAnnotations;

namespace Orders.ViewModels.OrderItem
{
    public class OrderItemUpdateViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Количество")]
        public decimal Quantity { get; set; }
        [Required]
        [Display(Name = "Еденица измерения")]
        public string Unit { get; set; }
    }
}
