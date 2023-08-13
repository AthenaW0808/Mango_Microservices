using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service;

public class TokenProvider : ITokenProvider
{

    /*
     * IHttpContextAccessor是ASP.NET Core中的一个接口，
     * 它允许您在整个应用程序中访问当前HTTP请求的相关信息，
     * 例如HTTP上下文、请求和响应对象等。通常情况下，
     * ASP.NET Core将每个HTTP请求视为一个独立的操作单元，
     * HttpContextAccessor使您能够在应用程序的
     * 各个部分轻松地访问和共享这些请求相关的信息
     */
    private readonly IHttpContextAccessor _contextAccessor;

    public TokenProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void SetToken(string token)
    {
        _contextAccessor.HttpContext?
            .Response.Cookies.Append(SD.TokenCookie, token);
    }

    public string? GetToken()
    {
        string? token = null;
        bool? hasToken = _contextAccessor.HttpContext?
            .Request.Cookies.TryGetValue(SD.TokenCookie, out token);

        return hasToken is true ? token : null;
    }

    public void ClearToken()
    {
        _contextAccessor.HttpContext?
            .Response.Cookies
            .Delete(SD.TokenCookie);
    }
}