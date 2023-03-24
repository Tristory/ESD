using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ESD.Models
{
    public class React
    {
        [Key]
        public int Id { get; set; }

        public bool IsLiked { get; set; }

        public int IdeaId { get; set; }

        public string UserId { get; set; }

        public React()
        { }
    }
}
