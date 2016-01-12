using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels.Reference
{
    public class RequestAccessViewModel
    {
        [Required]
        public string Comments { get; set; }
    }
}
