using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName ="MaxLength")]
        [Index("State_Estado_Index", IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "State_Model_Estado")]
        public string Estado { get; set; }

        public virtual ICollection<Reception> Receptions { get; set; }

        public virtual ICollection<ReceptionAssign> ReceptionAssigns { get; set; }

        public virtual ICollection<ReceptionAssignDetail> ReceptionAssignDetails { get; set; }

        public virtual ICollection<TechnicalWork> TechnicalWorks { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}