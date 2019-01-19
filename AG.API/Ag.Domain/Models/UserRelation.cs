using System.ComponentModel.DataAnnotations;

namespace Ag.Domain.Models
{
    public class UserRelation
    {
        [Required]
        public int FromId { get; set; }

        [Required]
        public int ToId { get; set; }

        [Required]
        public virtual User UserFrom { get; set; }

        [Required]
        public virtual User UserTo { get; set; }
    }
}
