using FiltersSample.Interfaces;

namespace FiltersSample.Services
{
    public class KnownAuthorService :IKnownAuthorService
    {
        public bool IsKnownAuthor(string name)
        {
            return name == "Steve";
        }
    }
}
