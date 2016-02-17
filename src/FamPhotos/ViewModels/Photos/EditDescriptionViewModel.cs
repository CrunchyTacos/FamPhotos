using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FamPhotos.ViewModels.Photos
{
    public class EditDescriptionViewModel
    {
        [Required]
        public string Description { get; set; }
    }
}
