using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ProductAPI.Validation;

namespace ProductAPI.Models
{
    public class Product
    {
        [Key]
        [DisplayName("Id")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Informe o nome do produto")]
        [DisplayName("Nome do Produto")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a quantidade do produto no estoque")]
        [DisplayName("Quantidade de estoque")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Informe o valor do produto")]
        [DisplayName("Valor do produto")]
        [NonNegative]
        public decimal Price { get; set; }
    }
}
