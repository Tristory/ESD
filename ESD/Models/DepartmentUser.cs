using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESD.Models
{
    public class DepartmentUser
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int DepartmentId { get; set; }               

        public DepartmentUser() 
        { }
    }
}
