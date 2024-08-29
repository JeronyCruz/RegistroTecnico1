using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico1.Models
{
    public class Tecnicos
    {
        [Key]
        public int tecnicoId { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el nombre del Tecnico")]
        public string nombreTecnico { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el sueldo del tecnico")]
        public float sueldoHora { get; set; }
    }
}
