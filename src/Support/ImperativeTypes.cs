
namespace ImperativeTypes
{
    public class Request
    {
        public Request(int id, string n, string e) => (UserId, Name, Email) = (id, n, e);
        public int UserId;
        public string Name;
        public string Email;
    }

    public interface IDb
    {
        bool UpdateDatabase(Request r);        
    }

    public interface ILog
    {
        void Error<T>(T e);
    }

    public interface ISmtpClient
    {
        bool SendEmail(string email);
    }

    public static class Assert
    {
        public static void AreEqual<T>(T a, T b) {}
    }
}