namespace InvoiceManagement1.Auth
{
    public class UserService : IUserService
    {
        // For simplicity: one hardcoded user
        // Dummy User Validation
        public bool IsValidUser(string username, string password)
        {
            return username == "admin" && password == "admin123";
        }
    }
}
