using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace ESD.Models
{
    public class Idea
    {
        [Key]
        public int Id { get; set; }
                
        public string Text { get; set; }

        public string FilePath { get; set; }
               
        public DateTime DateTime { get; set; }
                
        public bool IsAnomynous { get; set; }

        public int LikeS { get; set; }

        public int DislikeS { get; set; }

        public int ViewS { get; set; }
                
        public string UserId { get; set; }

        //Foreign key for Category
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        //Foreign key for Topic
        [Display(Name = "Topic")]
        public int TopicId { get; set; }

        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }

        public Idea() 
        { }
    }
}
