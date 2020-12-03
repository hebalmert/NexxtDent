using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class ReceptionAssign
    {
        [Key]
        public int ReceptionAssignId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Index("ReceptionAssign_Recepcion_Company_Index", 1, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Index("ReceptionAssign_Recepcion_Company_Index", 2, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Clinic")]
        public int ReceptionId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Reception")]
        public int Recepcion { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Clinic")]
        public int ClientId { get; set; }
      
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_TotalValue")]
        public decimal Total { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_State")]
        public int StateId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal TotalValue { get { return ReceptionAssignDetails == null ? 0 : ReceptionAssignDetails.Sum(d => d.Value); } }

        public virtual Company Company { get; set; }

        public virtual Reception Reception { get; set; }

        public virtual Client Client { get; set; }

        public virtual State State { get; set; }

        public virtual ICollection<ReceptionAssignDetail> ReceptionAssignDetails { get; set; }

        public virtual ICollection<TechnicalWork> TechnicalWorks { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}