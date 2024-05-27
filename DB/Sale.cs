using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Name { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Document { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Email { get; set; }

        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
