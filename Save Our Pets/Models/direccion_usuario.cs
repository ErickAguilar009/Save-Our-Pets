//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Save_Our_Pets.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class direccion_usuario
    {
        public int id_direccion { get; set; }
        public Nullable<int> id_usuario { get; set; }
        [Display(Name = "Departamento")]
        
        public string departamento { get; set; }
        [Display(Name = "Municipio")]
        
        public string municipio { get; set; }
        [Display(Name = "Dirección")]
        public string direccion { get; set; }
    
        public virtual Usuarios Usuarios { get; set; }
    }
}
