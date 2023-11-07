using G12App.Domain.Model.Base;

namespace G12App.Domain.Model
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
    }
}
