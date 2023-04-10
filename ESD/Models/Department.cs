using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Models;

namespace ESD.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
                
        public string Name { get; set; }

        public int IdeasS { get; set; }

        public Department()
        { }
    }
}
