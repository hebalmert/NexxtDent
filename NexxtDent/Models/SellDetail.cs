using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class SellDetail
    {
        [Key]
        public int SellDetailId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Display(ResourceType = typeof(Resource), Name = "SellDetail_Model_Codigo")]
        public int SellId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellDetail_Model_Codigo")]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        public string Codigo { get; set; }

        //[Display(Name = "Origen")]
        //[MaxLength(25, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        //public string Origen { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellDetail_Model_Concepto")]
        [MaxLength(25, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        public string Concepto { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [Display(ResourceType = typeof(Resource), Name = "SellDetail_Model_Cantidad")]
        public int Cantidad { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "SellDetail_Model_Unitario")]
        public decimal Unitario { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Currency es formato de Moneda del pais IP
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "SellDetail_Model_Total")]
        public decimal Total { get; set; }

        [Range(0, 1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]  //Porcentaje entre 0 y 1, 12%= 0.12
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)] //Formato Porcentaje con 2 decimales
        [Display(ResourceType = typeof(Resource), Name = "SellDetail_Model_Tasa")]
        public decimal Tasa { get; set; }

        public virtual Sell Sell { get; set; }
    }
}