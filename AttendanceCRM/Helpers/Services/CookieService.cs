using AttendanceCRM.Helpers.Contracts;

namespace AttendanceCRM.Helpers.Services
{
    public class CookieService : ICookieService
    {
        private IHttpContextAccessor _contextAccessor;

        public CookieService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void AddCookie(string cookieName, string cookieValue)
        {
            CookieOptions option = new CookieOptions()
            {
                Expires = DateTime.Now.AddMonths(1),
                SameSite = SameSiteMode.Strict
            };

            string encryptedCookieValue = EncryptionDecryption.GetEncrypt(cookieValue);
            _contextAccessor.HttpContext.Response.Cookies.Append(cookieName, encryptedCookieValue, option);
        }

        public void RemoveCookie(string cookieName)
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete(cookieName);
        }

        public string GetCookieValue(string cookieName)
        {
            var cookie = _contextAccessor.HttpContext.Request.Cookies[cookieName];
            return cookie != null ? EncryptionDecryption.GetDecrypt(cookie) : string.Empty;
        }
    }
}
