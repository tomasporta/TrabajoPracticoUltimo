using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentesComputadoras.Application.Dtos.Proveedor
{
    public class ProveedorRequestDto
    {
        public string RazonSocial { get; set; }
        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string CUIT { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}
