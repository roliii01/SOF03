using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOF03.Models
{
    public class Job
    {
        [Key]
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Salary { get; set; }
        public string OwnerId { get; set; }
        [NotMapped]
        public virtual SiteUser Owner { get; set; }
        public virtual ICollection<JobSiteUser> JobSiteUsers  { get; set; }
        public Job()
        {
            Uid = Guid.NewGuid().ToString(); 
        }
    }
}
