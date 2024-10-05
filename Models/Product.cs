using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MiMovil.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; }= "";
        [MaxLength(100)]
        public string Marca { get; set; } = "";
        [MaxLength(100)]
        public string Categoria { get; set; } = "";
        [Precision(16, 2)]
        public decimal Precio { get; set; }
        public string Descripcion{ get; set; } = "";
        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
