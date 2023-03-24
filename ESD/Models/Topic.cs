using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESD.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CloseDate_1 { get; set; }

        public DateTime CloseDate_2 { get; set;}

        public List<Idea> Ideas { get; set; }

        public Topic()
        { }
    }
}
