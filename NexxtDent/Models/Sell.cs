using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class Sell
    {
        [Key]
        public int SellId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public int CompanyId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_BillingNote")]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        public string NotaCobro { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Fecha")]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Reception")]
        public int ReceptionId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Reception")]
        public int Recepcion { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Client")]
        public int ClientId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Client")]
        public string Cliente { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_IDType")]
        public string TipoDocumento { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_ID")]
        public string IdentificationNumber { get; set; }

        [MaxLength(20, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.PhoneNumber)]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Address")]
        public string Address { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.PhoneNumber)]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Zona")]
        public string Zona { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_ExpirationDate")]
        public DateTime DateVencimiento { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.PhoneNumber)]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Condition")]
        public string Condicion { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_SubTotal")]
        public decimal SubTotal { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_IVA")]
        public decimal Iva { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Total")]
        public decimal Total { get; set; }

        [Display(Name = "Impresa")]
        public bool Impresa { get; set; }

        [MaxLength(12, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Control")]
        public string Control { get; set; }

        [MaxLength(12, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Bill")]
        public string NFactura { get; set; }

        [Display(Name = "Anulada Factura")]
        public bool AnuladaFactura { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resource), Name = "CxC_Model_FechaPagado")]
        public DateTime DateAnulada { get; set; }


        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_HeadText")]
        public int HeadTextId { get; set; }

        public virtual Company Company { get; set; }

        public virtual Reception Reception { get; set; }

        public virtual Client Client { get; set; }

        public virtual HeadText HeadText { get; set; }

        public virtual ICollection<SellDetail> SellDetails { get; set; }

        public virtual ICollection<CxCBill> CxCBills { get; set; }

        public virtual ICollection<CxCCanceled> CxCCanceleds { get; set; }
    }
}