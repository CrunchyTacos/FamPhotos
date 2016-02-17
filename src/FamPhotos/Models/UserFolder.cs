using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamPhotos.Models
{

    public class UserFolder
    {
        public int UserFolderID { get; set; }
        public string Name { get; set; }
        
        public ICollection<Photo> Photos { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    
}
