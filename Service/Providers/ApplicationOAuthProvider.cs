using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using TestWebApi.Service.Services;

namespace TestWebApi.Service.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null) throw new ArgumentNullException(nameof(publicClientId));
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            await Task.Run(() =>
            {
                var service = new CustomerService();
                var customer = service.GetCustomer(context.UserName, context.Password);
                if (customer == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var oAuthIdentity = customer.GenerateUserIdentity(OAuthDefaults.AuthenticationType);
                var properties = CreateProperties(customer.Id, customer.UserName);
                var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
            });
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userId, string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userId", userId },
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}