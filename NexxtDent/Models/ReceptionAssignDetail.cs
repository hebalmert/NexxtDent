using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class ReceptionAssignDetail
    {
        [Key]
        public int ReceptionAssignDetailId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Clinic")]
        public int ReceptionAssignId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "RepAssignDetail_Model_Tecnico")]
        public int TechnicalId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "RepAssignDetail_Model_CategoryService")]
        public int ServiceCategoryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "RepAssignDetail_Model_Service")]
        public int ServiceId { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Service_Model_Service")]
        public string Servicio { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "RepAssignDetail_Model_NivelPrecio")]
        public int LevelPriceId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(0, 1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Tax_Model_RangeMsg")]  //Porcentaje entre 0 y 1, 12%= 0.12
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "ReceptionWork_Model_Tasa")]
        public decimal Tasa { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Numero simbolo del Pais con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "RepAssignDetail_Model_Precio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [Display(ResourceType = typeof(Resource), Name = "RepAssignDetail_Model_Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Numero simbolo del Pais con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "RepAssignDetail_Model_Total")]
        public decimal Total { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_State")]
        public int StateId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Value { get { return Precio * (decimal)Cantidad; } }

        public virtual ReceptionAssign ReceptionAssign { get; set; }

        public virtual Technical Technical { get; set; }

        public virtual ServiceCategory ServiceCategory { get; set; }

        public virtual Service Service { get; set; }

        public virtual LevelPrice LevelPrice { get; set; }

        public virtual State State { get; set; }

    }
}