﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MiMovil.Models
{
    public class ProductDto

    {
        [Required, MaxLength(100)]
        public string Nombre { get; set; } = "";
        [Required, MaxLength(100)]
        public string Marca { get; set; } = "";
        [Required, MaxLength(100)]
        public string Categoria { get; set; } = "";
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public string Descripcion { get; set; } = "";
        public  IFormFile? ImageFile { get; set;}

    }
}
