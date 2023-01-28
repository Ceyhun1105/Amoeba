using System.ComponentModel.DataAnnotations;

namespace Amoeba.Models
{
    public class Position
    {
        public int Id { get; set; }
        [StringLength(maximumLength:50)]
        public string Name { get; set; }

        public List<Team>? Teams { get; set; }
    }
}
