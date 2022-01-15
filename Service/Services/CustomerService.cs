using System.Linq;
using TestWebApi.Service.Models;

namespace TestWebApi.Service.Services
{
    public class CustomerService
    {
        public Customer GetCustomer(string login, string password)
        {
            var decodePassword = Customer.DecodePassword(password);
            return CustomerList.Items.SingleOrDefault(item =>
                item.UserName == login && item.Password == decodePassword);
        }

        // Suggestion:
        // Create a CustomerSession and CustomerSessionList classes where we track that specific customer uses different access tokens.
        // And also possibly I would track there for which operation (method of controller) appropriate access token has been used:
        //
        // public class CustomerSession
        // {
        //     public Guid Id { get; set; }
        //     public string CustomerId { get; set; }
        //     public string AccessToken { get; set; }
        //     public DateTime Time { get; set; }
        //     public string Operation { get; set; }
        // }
        // 
        // public static class CustomerSessionList
        // {
        //     ...
        //     public static readonly List<CustomerSession> Items;
        //     ...
        // }
        //
        // But obviously it is out of test task scope
    }
}