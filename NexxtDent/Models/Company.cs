using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("Company_Name_Index", IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public string Compania { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("Company_Rif_Index", IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Rif")]
        public string Rif { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.PhoneNumber)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(250, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Logo")]
        public string Logo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Country")]
        public string Country { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Logo")]
        public HttpPostedFileBase LogoFile { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Desde")]
        public DateTime DateDesde { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Hasta")]
        public DateTime DateHasta { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Active")]
        public bool Activo { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Register> Registers { get; set; }

        public virtual ICollection<City> Cities { get; set; }

        public virtual ICollection<Identification> Identifications { get; set; }

        public virtual ICollection<HeadText> HeadTexts { get; set; }

        public virtual ICollection<SupportText> SupportTexts { get; set; }

        public virtual ICollection<Tax> Taxes { get; set; }

        public virtual ICollection<Client> Clients { get; set; }

        public virtual ICollection<WorkStation> WorkStations { get; set; }

        public virtual ICollection<Technical> Technicals { get; set; }

        public virtual ICollection<WorkCategory> WorkCategories { get; set; }

        public virtual ICollection<ServiceCategory> ServiceCategories { get; set; }

        public virtual ICollection<Work> Works { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual ICollection<ToothColor> ToothColors { get; set; }

        public virtual ICollection<GumColor> GumColors { get; set; }

        public virtual ICollection<Reception> Receptions { get; set; }

        public virtual ICollection<ReceptionLog> ReceptionLogs { get; set; }

        public virtual ICollection<ReceptionAssign> ReceptionAssigns { get; set; }

        public virtual ICollection<TechnicalWork> TechnicalWorks { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

        public virtual ICollection<Sell> Sells { get; set; }

        public virtual ICollection<CxCBill> CxCBills { get; set; }

        public virtual ICollection<CxCBillDetail> CxCBillDetails { get; set; }

        public virtual ICollection<Income> Incomes { get; set; }

        public virtual ICollection<Outcome> Outcomes { get; set; }

        public virtual ICollection<TechnicalPay> TechnicalPays { get; set; }

        public virtual ICollection<TechnicalPayDetail> TechnicalPayDetails { get; set; }

        public virtual ICollection<CxCCanceled> CxCCanceleds { get; set; }

        public virtual ICollection<TechnicalPorcentage> TechnicalPorcentages { get; set; }
    }
}