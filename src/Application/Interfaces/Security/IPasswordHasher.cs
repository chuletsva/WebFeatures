namespace Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        string ComputeHash(string passord);

        bool Verify(string hash, string expectedPassword);
    }
}
