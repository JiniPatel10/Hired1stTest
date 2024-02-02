namespace backendApi.Infrastructure
{
    public interface IMailService
    {
        Task<bool> SendMailForForgotPassword(string reciepient, string message);

    }
}
