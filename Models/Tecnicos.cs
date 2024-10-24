using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico1.Models;
    public class Tecnicos
    {
        [Key]
        public int TecnicoId { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el nombre del Tecnico")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se permiten letras")]
        public string NombresTecnico { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el sueldo del tecnico")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]

        public float SueldoHora { get; set; }

        [ForeignKey("TipoTecnico")]
        public int Id { get; set; }
        public TiposTecnicos? TipoTecnico { get; set; }


        [Required(ErrorMessage = "Campo Obligatorio")]
        public DateTime Fecha { get; set; } = DateTime.Now;
}