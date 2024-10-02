using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico1.Models;

public class Trabajos
{
    [Key]
    public int TrabajoId { get; set; }
    [Required(ErrorMessage = "Campo Obligatorio")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Por favor ingrese la descripccion del Trabajo")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se permiten letras")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Por favor ingrese el Monto del Trabajo")]
    [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]
    public double Monto { get; set; }

    [ForeignKey("Cliente")]
    public int ClienteId { get; set; }
    public Clientes? Cliente { get; set; }

    [ForeignKey("Tecnico")]
    public int TecnicoId { get; set; }
    public Tecnicos? Tecnico { get; set; }

    [ForeignKey("Prioridad")]
    public int PrioridadId { get; set; }
    public Prioridades? Prioridad { get; set;}


}
