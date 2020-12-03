using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class CxCBill
    {
        [Key]
        public int CxCBillId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_BillingNote")]
        public int SellId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_BillingNote")]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        public string NotaCobro { get; set; }

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
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        //[Index("TechnicalWork_Recepcion_Technical_Company_Index", 2, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "TechnicalWork_Model_Reception")]
        public int ReceptionId { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "CxC_Model_Total")]
        public decimal Total { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "CxC_Model_Abono")]
        public decimal Abono { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "Cxc_Model_Saldo")]
        public decimal Saldo { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CxC_Model_Pagado")]
        public bool Pagado { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "CxC_Model_FechaPagado")]
        public DateTime DatePagado { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CxC_Model_Anulado")]
        public bool Anulado { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "CxC_Model_FechaPagado")]
        public DateTime DateAnulado { get; set; }

        public virtual Company Company { get; set; }

        public virtual Sell Sell { get; set; }

        public virtual Client Client { get; set; }

        public virtual Reception Reception { get; set; }

        public virtual ICollection<CxCBillDetail> CxCBillDetails { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal TotalValue { get { return CxCBillDetails == null ? 0 : CxCBillDetails.Sum(d => d.Abono); } }


        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public virtual decimal TotalDeuda { get; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public virtual decimal TotalAbono { get; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public virtual decimal TotalSaldo { get; }

        public virtual ICollection<CxCCanceled> CxCCanceleds { get; set; }
    }
}