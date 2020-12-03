using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class TechnicalWorkDetail
    {
        [Key]
        public int TTechnicalWorkDetailId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        //[Index("TechnicalWork_Recepcion_Technical_Company_Index", 2, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "TechnicalWork_Model_Reception")]
        public int TechnicalWorkId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resource), Name = "WorkStation_Model_Detail")]
        public string Detalle { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_State")]
        public int StateTechnicalId { get; set; }

        public virtual TechnicalWork TechnicalWork { get; set; }

        public virtual StateTechnical StateTechnical { get; set; }
    }
}