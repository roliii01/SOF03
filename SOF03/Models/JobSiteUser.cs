using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOF03.Models
{
    public class JobSiteUser
    {
        [MaxLength(100)]
        public string JobUid { get; set; }
        public virtual Job Job { get; set; }
        [MaxLength(450)]
        [ForeignKey("SiteUser")]
        public string SiteUserId { get; set; }
        public virtual SiteUser SiteUser { get; set; }
    }
}
