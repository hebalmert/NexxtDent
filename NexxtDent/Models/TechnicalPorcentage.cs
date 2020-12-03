using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class TechnicalPorcentage
    {
            [Key]
            public int TechnicalPorcentageId { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
            [Display(ResourceType = typeof(Resource), Name = "TecnichalPorcentage_Model_Company")]
            public int CompanyId { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "TecnichalPorcentage_Model_Porcentaje")]
            public bool CobraPorcentaje { get; set; }

            public virtual Company Company { get; set; }

    }
}