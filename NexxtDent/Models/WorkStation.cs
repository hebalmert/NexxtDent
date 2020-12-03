using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class WorkStation
    {
        [Key]
        public int WorkStationId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Index("WorkStation_Company_Estacion_Index", 1, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("WorkStation_Company_Estacion_Index", 2, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "WorkStation_Model_WorkStation")]
        public string Estacion { get; set; }

        [MaxLength(250, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resource), Name = "WorkStation_Model_Detail")]
        public string Detalle { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Technical> Technicals { get; set; }
    }
}