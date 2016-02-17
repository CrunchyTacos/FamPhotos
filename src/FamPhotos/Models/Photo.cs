using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamPhotos.Models
{
    public class Photo
    {
        public int PhotoID { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string Url { get; set; }

        public int UserFolderId { get; set; }
        public UserFolder UserFolder { get; set; }
    }
}
