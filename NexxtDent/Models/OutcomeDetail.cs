using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class OutcomeDetail
    {
        [Key]
        public int OutcomeDetailId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Outcome_Model_Egreso")]
        public int OutcomeId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OutcomeDetail_Model_Document")]
        [MaxLength(12, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        public string Documento { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Outcome_Model_Detalle")]
        public string Detalle { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "OutcomeDetail_Model_Valor")]
        public decimal Precio { get; set; }

        public virtual Outcome Outcome { get; set; }
    }
}