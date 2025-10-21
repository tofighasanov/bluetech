using System.Globalization;
using System.Security.Cryptography;

namespace Bluetech.Services
{
    public record struct ArithmeticCaptchaChallenge(string Question, string Answer);

    public sealed class ArithmeticCaptchaService : IArithmeticCaptchaService
    {
        public ArithmeticCaptchaChallenge GenerateChallenge()
        {
            var left = RandomNumberGenerator.GetInt32(2, 10);
            var right = RandomNumberGenerator.GetInt32(1, 10);
            var answer = (left + right).ToString(CultureInfo.InvariantCulture);
            var question = $"Сколько будет {left} + {right}?";

            return new ArithmeticCaptchaChallenge(question, answer);
        }
    }
}