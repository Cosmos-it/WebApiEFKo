using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace StandardWebApi.Security
{
    
    public class TokenGenerator
    {
        private const string _salt = "jsGLWw89nv+nruXIAJvGgbGwz36u"; // Randomly generated for hashing password
        private const string _algo = "HmacSHA256";
        private const int _tokenExpiration = 10; // mins

        public static string GenerateToken(string username, string password, string ip, long ticks)
        {
            string hash = string.Join(":", new string[] { username, ip, ticks.ToString() });
            string hashLeft = "";
            string hashRight = "";
            using (HMAC hmac = HMACSHA256.Create(_algo))
            {
                hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(password));
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", new string[] { username, ticks.ToString() });
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }

        private static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });
            using (HMAC hmac = HMACSHA256.Create(_algo))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hmac.Hash);
            }
        }

        public static bool IsTokenValid(string token, string ip)
        {
            bool result = false;

            try
            {
                // Base64 decode the string, obtaining the token:username:timeStamp.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    string username = parts[1];
                    long ticks = long.Parse(parts[2]);
                    DateTime timeStamp = new DateTime(ticks);
                    // Ensure the timestamp is valid.
                    bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _tokenExpiration;
                    if (!expired)
                    {
                        //
                        // Lookup the user's account from the db. Validate the users account before computing the 
                        // access token
                        //
                        if (username == "taban")
                        {
                            string password = "1234";
                            // Hash the message with the key to generate a token.
                            string computedToken = GenerateToken(username, password, ip, ticks);
                            // Compare the computed token with the one supplied and ensure they match.
                            result = (token == computedToken);
                        }
                    }
                }
            }
            catch
            {
            }
            return result;
        }
    }
}