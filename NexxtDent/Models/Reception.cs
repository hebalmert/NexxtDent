using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class Reception
    {
        [Key]
        public int ReceptionId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Index("Reception_Recepcion_Company_Index", 1, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Index("Reception_Recepcion_Company_Index", 2, IsUnique = true)]
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

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Patient")]
        public string Paciente { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_ToothColor")]
        public int ToothColorId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_GumColor")]
        public int GumColorId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_TestDate")]
        public DateTime DatePrueba { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_DeliverDate")]
        public DateTime DateEntrega { get; set; }

        [MaxLength(250, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_Detail")]
        [DataType(DataType.MultilineText)]
        public string Detalles { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_State")]
        public int StateId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal TotalValue { get { return ReceptionWorks == null ? 0 : ReceptionWorks.Sum(d => d.Value); } }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "CxC_Model_FechaPagado")]
        public DateTime DateAnulado { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Reception_Model_State")]
        public int HeadTextId { get; set; }

        public virtual Company Company { get; set; }

        public virtual Client Client { get; set; }

        public virtual ToothColor ToothColor { get; set; }

        public virtual GumColor GumColor { get; set; }

        public virtual State State { get; set; }

        public virtual HeadText HeadText { get; set; }

        public virtual ICollection<ReceptionLog> ReceptionLogs { get; set; }

        public virtual ICollection<ReceptionWork> ReceptionWorks { get; set; }

        public virtual ICollection<ReceptionAssign> ReceptionAssigns { get; set; }

        public virtual ICollection<TechnicalWork> TechnicalWorks { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

        public virtual ICollection<Sell> Sells { get; set; }

        public virtual ICollection<CxCBill> CxCBills { get; set; }

        public virtual ICollection<TechnicalPay> TechnicalPays { get; set; }

        public virtual ICollection<CxCCanceled> CxCCanceleds { get; set; }
    }
}