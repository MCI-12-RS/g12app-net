namespace G12App.Domain.DTOs
{
    public class ResponseLoginDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        
    }
}
