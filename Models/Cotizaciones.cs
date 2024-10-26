using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico1.Models
{
    public class Cotizaciones
    {
        [Key]
        public int CotizacionId { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Clientes? Cliente { get; set; }

        [Required(ErrorMessage = "Por favor ingrese la observacion de la cotizacion")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se permiten letras")]
        public string? Observacion { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el Monto de la cotizacion")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]
        public double Monto { get; set; }

        [ForeignKey("CotizacionId")]
        public ICollection<CotizacionesDetalle> CotizacionesDetalle { get; set; } = new List<CotizacionesDetalle>();
    }
}
