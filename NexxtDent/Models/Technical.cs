using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class Technical
    {
        [Key]
        public int TechnicalId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Index("Technical_Identificacion_Company_Index", 1, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "User_Model_FisrtName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "User_Model_LastName")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Techincal_Model_FullName")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_DocumentType")]
        public int IdentificationId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("Technical_Identificacion_Company_Index", 2, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_DocumentNumber")]
        public string Identificacion { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("Technical_UserName_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(Resource), Name = "User_Model_Email")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.PhoneNumber)]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.PhoneNumber)]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_Phone")]
        public string Phone2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_Address")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "WorkStation_Model_WorkStation")]
        public int WorkStationId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "City_Model_City")]
        public int CityId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Zone_Model_Zone")]
        public int ZoneId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Active")]
        public bool Activo { get; set; }

        //[Display(ResourceType = typeof(Resource), Name = "Techincal_Model_FullName")]
        //public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public virtual Company Company { get; set; }

        public virtual Identification Identification { get; set; }

        public virtual WorkStation WorkStation { get; set; }

        public virtual City City { get; set; }

        public virtual Zone Zone { get; set; }

        public virtual ICollection<ReceptionAssignDetail> ReceptionAssignDetails { get; set; }

        public virtual ICollection<TechnicalWork> TechnicalWorks { get; set; }

        public virtual ICollection<TechnicalPay> TechnicalPays { get; set; }
    }
}