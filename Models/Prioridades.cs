using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico1.Models;

public class Prioridades
{
    [Key]
    public int PrioridadId { get; set; }

    [Required(ErrorMessage = "Por favor ingrese la Descripcion de la Prioridad")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se permiten letras")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage ="Por favor ingrese el Tiempo de la Prioridad")]
    [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]

    public int Tiempo { get; set; }

    [Required(ErrorMessage = "Campo Obligatorio")]
    public DateTime Fecha { get; set; } = DateTime.Now;
}
