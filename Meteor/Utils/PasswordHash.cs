using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Meteor.Utils
{
    public static class PasswordHash
    {
        private const int SaltByteSize = 23;
        private const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        private const int Pbkdf2Iterations = 1001;
        private const int IterationIndex = 0;
        private const int SaltIndex = 1;
        private const int Pbkdf2Index = 2;

        public static string Hash(string plainPwd)
        {
            if (plainPwd == null)
                throw Errors.InvalidInput(nameof(plainPwd));
            
            using var cryptoProvider = new RNGCryptoServiceProvider();
            var salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(plainPwd, salt, Pbkdf2Iterations, HashByteSize);
            return Pbkdf2Iterations + ":" +
                   Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        public static bool Validate(string plainPwd, string hash)
        {
            if (plainPwd == null || hash == null)
                throw Errors.InvalidInput(nameof(plainPwd));
            
            char[] delimiter = { ':' };
            var split = hash.Split(delimiter);
            var iterations = int.Parse(split[IterationIndex]);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var inputHash = Convert.FromBase64String(split[Pbkdf2Index]);

            var computedHash = GetPbkdf2Bytes(plainPwd, salt, iterations, inputHash.Length);
            return SlowEquals(inputHash, computedHash);
        }

        private static bool SlowEquals(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            var diff = (uint)a.Count ^ (uint)b.Count;
            for (var i = 0; i < a.Count && i < b.Count; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
        
        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}