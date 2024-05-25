using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Plate { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Color { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Brand { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Line { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Year { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Kilimetres { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Cost { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string? Observations { get; set; }

        [Required]
        public string Image { get; set; }

        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
