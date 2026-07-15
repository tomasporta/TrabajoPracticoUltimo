using ComponentesComputadoras.Application.Dtos.CompraDetalle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentesComputadoras.Application.Dtos.Compra
{
    public class CompraRequestDto
    {
        public DateTime Fecha { get; set; }
        public int ProveedorId { get; set; }
        public List<CompraDetalleRequestDto> Detalles { get; set; }
    }
}
