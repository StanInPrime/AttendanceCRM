namespace AttendanceCRM.Helpers.Contracts
{
    public interface ICookieService
    {
        void AddCookie(string cookieName, string cookieValue);

        void RemoveCookie(string cookieName);

        string GetCookieValue(string cookieName);
    }
}
