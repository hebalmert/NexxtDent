using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class StateTechnical
    {
        [Key]
        public int StateTechnicalId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("State_Estado_Index", IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "State_Model_Estado")]
        public string EstadoTecnico { get; set; }

    }
}