using System;
using System.ComponentModel.DataAnnotations;

namespace Orders.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Номер")]
        public string Number { get; set; }
        [Required]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        public string ProviderName { get; set; }
        [Required]
        [Display(Name = "Поставщик")]
        public int ProviderId { get; set; }
    }
}
