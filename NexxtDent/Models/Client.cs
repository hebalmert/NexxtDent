using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; } 

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Index("Client_Cliente_Company_Index", 1, IsUnique = true)]
        [Index("Client_IdentificationNumber_Company_Index", 1, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("Client_Cliente_Company_Index", 2, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_Client")]
        public string Cliente { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_DocumentType")]
        public int IdentificationId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("Client_IdentificationNumber_Company_Index", 2, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_DocumentNumber")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("Client_UserName_Index", IsUnique = true)]
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
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_Cell")]
        public string Movil { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resource), Name = "Client_Model_Address")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "LevelPrice_Model_PriceLevel")]
        public int LevelPriceId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "City_Model_City")]
        public int CityId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Zone_Model_Zone")]
        public int ZoneId { get; set; }

        public virtual Company Company { get; set; }

        public virtual Identification Identification { get; set; }

        public virtual LevelPrice LevelPrice { get; set; }

        public virtual City City { get; set; }

        public virtual Zone Zone { get; set; }

        public virtual ICollection<Reception> Receptions { get; set; }

        public virtual ICollection<ReceptionAssign> ReceptionAssigns { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

        public virtual ICollection<Sell> Sells { get; set; }

        public virtual ICollection<CxCBill> CxCBills { get; set; }

        public virtual ICollection<CxCCanceled> CxCCanceleds { get; set; }
    }
}