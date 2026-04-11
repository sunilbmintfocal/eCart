using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MintCart.Common
{
    public class PasswordHelper
    {
        public static string AutoGeneratePassword()
        {
            string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            string numberChars = "0123456789";
            string specialChars = "!@#$%^&*()_+";

            var random = new Random();

            // Ensure at least one character from each category
            char uppercaseChar = uppercaseChars[random.Next(uppercaseChars.Length)];
            char lowercaseChar = lowercaseChars[random.Next(lowercaseChars.Length)];
            char numberChar = numberChars[random.Next(numberChars.Length)];
            char specialChar = specialChars[random.Next(specialChars.Length)];

            // Combine all characters
            string allChars = uppercaseChars + lowercaseChars + numberChars + specialChars;

            // Generate the remaining characters
            string remainingChars = new string(Enumerable.Repeat(allChars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            // Shuffle all characters
            string shuffledChars = new string(remainingChars
                .Concat(new[] { uppercaseChar, lowercaseChar, numberChar, specialChar })
                .OrderBy(c => random.Next()).ToArray());

            return shuffledChars;
        }
    }
}
