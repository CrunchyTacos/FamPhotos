using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamPhotos.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        public string UserName { get; set; }
    }
}
