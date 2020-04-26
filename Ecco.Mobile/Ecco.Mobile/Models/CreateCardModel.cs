using Ecco.Entities;
using Ecco.Entities.Attributes;
using Syncfusion.XForms.DataForm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xamarin.Forms;

namespace Ecco.Mobile.Models
{
    public class CreateCardModel
    {
        [Display(Name = "Card Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a card title")]
        public string CardTitle { get; set; }

        [Display(Name = "Your Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide an email")]
        public string Email { get; set; }
        
        [Display(Name = "Your Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must select a service category")]
        public string ServiceCategory { get; set; }

        [Display(AutoGenerateField = false)]
        public ImageSource TemplateImage { get; set; }

        [Display(AutoGenerateField = false)]
        public string ExportedImageData { get; set; }

        [Display(AutoGenerateField = false)]
        public int TemplateId { get; set; }

        [Display(AutoGenerateField = false)]
        public bool IsCompanyTemplate { get; set; }

        public int id;
    }
}