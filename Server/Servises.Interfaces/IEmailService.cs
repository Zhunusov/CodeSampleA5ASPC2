using System.Threading.Tasks;

namespace Servises.Interfaces
{
    /// <summary>
    /// Service for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends a message to the specified address.
        /// </summary>
        /// <param name="email">Email address of recipient.</param>
        /// <param name="subject">Subject of email.</param>
        /// <param name="message">Email message.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation.</returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}