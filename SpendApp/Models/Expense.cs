using System.ComponentModel.DataAnnotations;

namespace SpendApp.Models
{
    public class Expense
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Value is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Value must be greater than 0")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
