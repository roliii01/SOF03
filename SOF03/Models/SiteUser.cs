using Microsoft.AspNetCore.Identity;

namespace SOF03.Models
{
    public class SiteUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MinimumSalasy { get; set; }
        public virtual ICollection<JobSiteUser> Jobs { get; set; }
    }
}
