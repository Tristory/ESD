using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ESD.Models;

namespace ESD.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
                
        public string Text { get; set; }

        public int IdeaId { get; set; }

        public string UserId { get; set; }

        public Comment() { }
    }
}
