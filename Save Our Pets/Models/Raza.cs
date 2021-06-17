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

    public partial class Raza
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Raza()
        {
            this.Mascota = new HashSet<Mascota>();
        }
    
        public int id_raza { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Ingrese el nombre de la raza")]
        public string nombre { get; set; }
        [Display(Name = "Especie")]
        public int id_especie { get; set; }
    
        public virtual Especie Especie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mascota> Mascota { get; set; }
    }
}