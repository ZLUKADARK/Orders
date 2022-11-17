using System;
using System.ComponentModel.DataAnnotations;

namespace Orders.ViewModels.Orders
{
    public class OrderUpdateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Номер заказа")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Заказчик")]
        public int ProviderId { get; set; }
    }
}
