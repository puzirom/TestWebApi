using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNet.Identity;
using TestWebApi.Service.Helper;

namespace TestWebApi.Service.Models
{
    [DataContract(Namespace = "http://serialize")]
    public class Customer: IUser
    {
        #region Properties

        [DataMember(EmitDefaultValue = false)] public string Id { get; set; }

        [DataMember(EmitDefaultValue = false)] public string FullName { get; set; }

        [DataMember(EmitDefaultValue = false)] public string UserName { get; set; }

        [DataMember(EmitDefaultValue = false)] public string Password { get; set; }

        #endregion

        /// <summary>
        /// Returns new Customer Instance
        /// </summary>
        /// <param name="fullName">User Full Name</param>
        /// <param name="userName">User Login</param>
        /// <param name="password">User Password</param>
        /// <returns>Customer</returns>
        public static Customer CreateCustomer(string fullName, string userName, string password)
        {
            return new Customer
            {
                Id = Guid.NewGuid().ToString(),
                FullName = fullName,
                UserName = userName,
                Password = password
            };
        }

        /// <summary>
        /// Generate a User Identity
        /// </summary>
        /// <param name="authenticationType">Authentication Type</param>
        /// <returns>ClaimsIdentity</returns>
        public ClaimsIdentity GenerateUserIdentity(string authenticationType)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, FullName),
                new Claim(ClaimTypes.NameIdentifier, UserName)
            };
            return new ClaimsIdentity(claims, authenticationType);
        }

        /// <summary>
        /// Decode a Password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DecodePassword(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }
    }

    public static class CustomerList
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Customers.xml");
        public static readonly List<Customer> Items;

        static CustomerList()
        {
            Items = DataStorageHelper.Deserialize<List<Customer>>(FilePath);
            GenerateTestList();
        }

        private static void GenerateTestList()
        {
            var initialPassword = Customer.DecodePassword("qwe123");
            for (var i = 0; i < 30; i++)
            {
                var index = i < 9 ? $"0{i + 1}" : $"{i + 1}";
                var name = $"{DataGenerationHelper.GenerateName(6)} {DataGenerationHelper.GenerateName(7)}";
                var userName = $"customer_{index}@mail.com";
                var customer = Customer.CreateCustomer(name, userName, initialPassword);
                Items.Add(customer);
            }

            Update();
        }

        public static void Update()
        {
            var xml = DataStorageHelper.Serialize(Items.GetType(), Items);
            xml.Save(FilePath);
        }
    }
}
