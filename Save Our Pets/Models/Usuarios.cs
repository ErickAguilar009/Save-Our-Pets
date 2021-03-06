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

    public partial class Usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuarios()
        {
            this.Adopciones = new HashSet<Adopciones>();
            this.direccion_usuario = new HashSet<direccion_usuario>();
            this.solicitud_adopcion = new HashSet<solicitud_adopcion>();
            this.solicitud_adopcion1 = new HashSet<solicitud_adopcion>();
        }
    
        public int id_usuario { get; set; }
        [Display(Name = "Tipo")]
        public Nullable<int> id_tipo { get; set; }
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Ingrese sus nombres, por favor")]
        public string nombres { get; set; }
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Ingrese sus apellidos, por favor")]
        public string apellidos { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Ingrese la dirección de su correo electrónico")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Ingrese un correo electrónico válido")]
        public string email { get; set; }
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "Ingrese el teléfono")]
        [RegularExpression("^(2|6|7)[0-9]{3}-[0-9]{4}$", ErrorMessage = "El número ingresado debe estar en el formato 2222-2222")]
        public string telefono { get; set; }
        [Display(Name = "DUI")]
        [Required(ErrorMessage = "Ingrese el DUI")]
        [RegularExpression("^[0-9]{8}-[0-9]{1}$", ErrorMessage = "El DUI ingresado debe estar en el formato 21455412-6")]
        public string DUI { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public System.DateTime fecha_nacimiento { get; set; }
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Ingrese una contraseña para la cuenta")]
        public string contra { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adopciones> Adopciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<direccion_usuario> direccion_usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<solicitud_adopcion> solicitud_adopcion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<solicitud_adopcion> solicitud_adopcion1 { get; set; }
        public virtual Tipo_usuario Tipo_usuario { get; set; }
    }
}
