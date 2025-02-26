using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyBookWebRazor_Temp.Models
{
    public class Category
    {
        [Key] //primary key data annotation
        public int Id { get; set; }
        [Required] // mandatory(can not be null) data annotation
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(0, 100, ErrorMessage = "Dispaly Order must be between 0-100")]
        public int DisplayOrder { get; set; }
    }
}
