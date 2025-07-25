using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CardPayment.Helper
{
    public class PayUHashHelper
    {
        public static string GenerateHash(string key, string txnId, string amount, string productInfo,
       string firstName, string email, string salt)
        {
            string hashString = $"{key}|{txnId}|{amount}|{productInfo}|{firstName}|{email}|||||||||||{salt}";
            using (var sha512 = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(hashString);
                byte[] hash = sha512.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
