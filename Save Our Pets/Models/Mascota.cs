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

    public partial class Mascota
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mascota()
        {
            this.Adopciones = new HashSet<Adopciones>();
            this.Estado_salud = new HashSet<Estado_salud>();
            this.img_mascota = new HashSet<img_mascota>();
            this.reporte_rescate = new HashSet<reporte_rescate>();
            this.Reporte_vacunas = new HashSet<Reporte_vacunas>();
        }

        public int id_mascota { get; set; }
        [Display(Name = "Nombre")]
        public string nombre_mascota { get; set; }
        [Display(Name = "Especie")]
        public int id_especie { get; set; }
        [Display(Name = "Raza")]
        public int id_raza { get; set; }
        [Display(Name = "Color de pelo")]
        public string color_pelo { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string fecha_nacimiento { get; set; }
        [Display(Name = "Peso")]
        public decimal peso { get; set; }
        [Display(Name = "Esterilizado")]
        public string esterilizado { get; set; }
        [Display(Name = "Estado")]
        public int id_estado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adopciones> Adopciones { get; set; }
        public virtual Especie Especie { get; set; }
        public virtual Estado_mascota Estado_mascota { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Estado_salud> Estado_salud { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<img_mascota> img_mascota { get; set; }
        public virtual Raza Raza { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reporte_rescate> reporte_rescate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reporte_vacunas> Reporte_vacunas { get; set; }
    }
}
