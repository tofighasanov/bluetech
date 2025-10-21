namespace Bluetech.Services
{
    public interface IArithmeticCaptchaService
    {
        ArithmeticCaptchaChallenge GenerateChallenge();
    }
}