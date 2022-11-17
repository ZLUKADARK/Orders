using System.ComponentModel.DataAnnotations;

namespace Orders.ViewModels.OrderItem
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public decimal Quantity { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Unit { get; set; }
        public string OrderNumber { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public int OrderId { get; set; }
    }
}
