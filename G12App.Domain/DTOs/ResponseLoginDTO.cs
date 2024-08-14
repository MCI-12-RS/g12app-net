namespace G12App.Domain.DTOs
{
    public class ResponseLoginDTO
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
