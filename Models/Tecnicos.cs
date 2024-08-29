using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico1.Models
{
    public class Tecnicos
    {
        [Key]
        public int tecnicoId { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el nombre del Tecnico")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se permiten letras")]
        public string nombreTecnico { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el sueldo del tecnico")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]

        public float sueldoHora { get; set; }
    }
}
