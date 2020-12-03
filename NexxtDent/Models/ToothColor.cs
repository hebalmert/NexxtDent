﻿using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class ToothColor
    {
        [Key]
        public int ToothColorId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(1, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Msg_Range")]
        [Index("ToothColor_ColorDiente_Company_Index", 1, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Compania")]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Index("ToothColor_ColorDiente_Company_Index", 2, IsUnique = true)]
        [Display(ResourceType = typeof(Resource), Name = "ToothColor_Model_ToothColor")]
        public string ColorDiente { get; set; }

        [MaxLength(250, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
        [Display(ResourceType = typeof(Resource), Name = "ToothColor_Model_Detalle")]
        [DataType(DataType.MultilineText)]
        public string Detalles { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Company_Model_Active")]
        public bool Activo { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Reception> Receptions { get; set; }
    }
}