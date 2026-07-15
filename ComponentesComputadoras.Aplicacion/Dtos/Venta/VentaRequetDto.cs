using ComponentesComputadoras.Application.Dtos.VentaDetalle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentesComputadoras.Application.Dtos.Venta
{
    public class VentaRequestDto
    {
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public List<VentaDetalleRequestDto> Detalles { get; set; }
    }
}
