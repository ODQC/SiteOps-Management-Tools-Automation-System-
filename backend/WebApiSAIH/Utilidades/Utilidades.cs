using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Utilidades
{
    public class Utilidades
    {
        public static String GenerarStringPassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var charsHastoBe = "$@!%*?";
            var charsHastoBe2 = "0123456789";
            var random = new Random(); 
            var result = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            var result3 = new string(Enumerable.Repeat(charsHastoBe2, 1).Select(s => s[random.Next(s.Length)]).ToArray());
            var random2 = new Random();
            var result2 = new string(Enumerable.Repeat(charsHastoBe, 1).Select(s => s[random2.Next(s.Length)]).ToArray());

            return result + result3 + result2;
        }
    }
}
