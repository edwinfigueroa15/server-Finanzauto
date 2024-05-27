using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(70, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(250, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(250, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Password { get; set; } = null!;

        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
