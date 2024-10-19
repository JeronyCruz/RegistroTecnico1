using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico1.Models;

public class TrabajosDetalle
{
	[Key]
	public int DetalleId { get; set; }

	[ForeignKey("Trabajo")]
	public int TrabajoId { get; set; }
	public Trabajos? Trabajo { get; set; }

	[ForeignKey("Articulo")]
	public int ArticuloId { get; set; }
	public Articulos? Articulo { get; set; }

	[Required(ErrorMessage = "Por favor ingrese la cantidad")]
	[RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]
	public int Cantidad { get; set; }

	[Required(ErrorMessage = "Por favor ingrese el Precio")]
	[RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]
	public double Precio { get; set; }

	[Required(ErrorMessage = "Por favor ingrese el Costo")]
	[RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]
	public double Costo { get; set; }
}
