using Ecco.Entities;
using Ecco.Entities.Attributes;
using Syncfusion.XForms.DataForm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ecco.Mobile.Models
{
    public class CreateCardModel
    {
        [Display(Name = "Full Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a name")]
        public string FullName { get; set; }
        
        [Display(Name = "Job Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a job title")]
        public string JobTitle { get; set; }
        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a description")]
        public string Description { get; set; }
        
        [Display(Name = "Card Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a card title")]
        public string CardTitle { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide an email")]
        public string Email { get; set; }
        
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must select a service category")]
        public string ServiceCategory { get; set; }
    }
}