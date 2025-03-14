﻿using System.ComponentModel.DataAnnotations;

namespace SavonDeLilly.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à zéro.")]
        public decimal Price { get; set; }

        public string? Ingredients { get; set; }

        [Required(ErrorMessage = "La catégorie est requise.")]
        public string Category { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas être négatif.")]
        public int Stock { get; set; }
    }
}
