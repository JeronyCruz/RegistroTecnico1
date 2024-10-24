using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico1.Models;

public class Clientes
{
    [Key]
    public int ClienteId { get; set; }

    [Required(ErrorMessage ="Por favor ingrese el Nombre del Cliente")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage ="Solo se Permiten Letras")]
    public string Nombres { get; set; }

    [Required(ErrorMessage ="Por favor, ingrese el numero de WhatsApp")]
    [RegularExpression(@"^\d+$" , ErrorMessage ="Solo se permiten Numeros")]
    public string WhatsApp { get; set; }

    [Required(ErrorMessage = "Campo Obligatorio")]
    public DateTime Fecha { get; set; } = DateTime.Now;
}
