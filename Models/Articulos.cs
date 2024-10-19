using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico1.Models;

public class Articulos
{
	[Key]
	public int ArticuloId { get; set; }

	[Required(ErrorMessage = "Por favor ingrese la Descripcion del Articulo")]
	[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se Permiten Letras")]
	public string Descripcion { get; set; }

	[Required(ErrorMessage = "Por favor ingrese el costo del articulo")]
	[RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]
	public double Costo { get; set; }

	[Required(ErrorMessage = "Por favor ingrese el Precio del articulo")]
	[RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]
	public double Precio { get; set; }

	[Required(ErrorMessage = "Por favor ingrese la cantidad en existecia del articulo")]
	[RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]
	public int Existencia { get; set; }

}
