namespace InvoiceManagement1.Auth
{
    public interface IUserService
    {
        bool IsValidUser(string username, string password);
    }
}
