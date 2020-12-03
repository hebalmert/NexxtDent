using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class LevelPrice
    {
        [Key]
        public int LevelPriceId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "LevelPrice_Model_PriceLevel")]
        public string NivelPrecio { get; set; }

        public virtual ICollection<Client> Clients { get; set; }

        public virtual ICollection<ReceptionAssignDetail> ReceptionAssignDetails { get; set; }
    }
}