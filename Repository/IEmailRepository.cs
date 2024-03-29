using BookStore.Model;

namespace BookStore.Repository
{
    public interface IEmailRepository
    {
        Task SendEmailMessage(EmailMessage emailMessage);
        Task SendEmailConfirmationMessage(EmailMessage emailMessage);
    }
}