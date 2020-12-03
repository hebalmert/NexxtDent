using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class DeliveryDetail
    {
        [Key]
        public int DeliveryDetailId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Clinic")]
        public int DeliveryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        [Display(ResourceType = typeof(Resource), Name = "DeliveryDetail_Model_Delivered")]
        public string Entregado { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        [Display(ResourceType = typeof(Resource), Name = "DeliveryDetail_Model_Received")]
        public string Recibido { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resource), Name = "DeliveryDetail_Model_Detalle")]
        public string Detalle { get; set; }

        public virtual Delivery Delivery { get; set; }

    }
}