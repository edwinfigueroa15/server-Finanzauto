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
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(70, ErrorMessage = "El campo {0} permite máximo {1} caractéres")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "El correo ya existe")]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
