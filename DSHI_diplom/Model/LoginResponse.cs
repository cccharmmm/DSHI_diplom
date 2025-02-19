namespace DSHI_diplom.Model
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public int UserId { get; set; }
        public string? Login { get; set; }
        public string? Role { get; set; }
    }
}
