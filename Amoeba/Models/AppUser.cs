using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Amoeba.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }
    }
}
