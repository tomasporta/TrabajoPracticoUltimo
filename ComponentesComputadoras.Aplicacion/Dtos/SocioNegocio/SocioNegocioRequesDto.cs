using ComponentesComputadoras.Enumeraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentesComputadoras.Application.Dtos.SocioNegocio
{
    public class SocioNegocioRequestDto
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public TipoSocio Tipo { get; set; }
        public int? ClienteId { get; set; }
        public int? ProveedorId { get; set; }

        // Campos adicionales
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        
    }
}










