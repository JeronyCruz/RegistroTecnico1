using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico1.Models;

public class TiposTecnicos
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Por favor ingrese la descripccion del Tecnico")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se permiten letras")]
    public string Descripcion { get; set; }
}
