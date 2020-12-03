using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class TechnicalPay
    {
        [Key]
        public int TechnicalPayId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        //[Index("TechnicalWork_Recepcion_Technical_Company_Index", 1, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        //[Index("TechnicalWork_Recepcion_Technical_Company_Index", 2, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "TechnicalWork_Model_Reception")]
        public int ReceptionId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        //[Index("TechnicalWork_Recepcion_Technical_Company_Index", 3, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "RepAssignDetail_Model_Tecnico")]
        public int TechnicalId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(50, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        [Display(ResourceType = typeof(Resource), Name = "RepAssignDetail_Model_Service")]
        public string Servicio { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "ReceptionWork_Model_Precio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "ReceptionWork_Model_Cantidad")]
        public decimal Cantidad { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "ReceptionWork_Model_Total")]
        public decimal Total { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "CxC_Model_Abono")]
        public decimal Abono { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "Cxc_Model_Saldo")]
        public decimal Saldo { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(0, 1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Porcentaje entre 0 y 1, 12%= 0.12
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "ReceptionWork_Model_Tasa")]
        public decimal Tasa { get; set; }

        [MaxLength(12, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "Sell_Model_Bill")]
        public string NFactura { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "TechnicalPay_Model_Facturado")]
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

        public virtual Reception Reception { get; set; }

        public virtual Technical Technical { get; set; }

        public virtual ICollection<TechnicalPayDetail> TechnicalPayDetails { get; set; }
    }
}