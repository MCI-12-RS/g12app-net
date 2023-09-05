using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace G12App.Application.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [MinLength(2)]
        [MaxLength(40)]
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
