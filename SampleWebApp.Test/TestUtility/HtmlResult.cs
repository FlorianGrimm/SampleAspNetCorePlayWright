using Microsoft.AspNetCore.Http;

namespace Sample.WebApp.Test.TestUtility;
public static class ResultsExtensions {
    public static IResult Html(this IResultExtensions resultExtensions, string html, string? testAuthCookie) {
        ArgumentNullException.ThrowIfNull(resultExtensions);
        return new HtmlResult(html, testAuthCookie);
    }
}

public class HtmlResult : IResult {
    private readonly string _Html;
    private readonly string? _TestAuthCookie;

    public HtmlResult(string html, string? testAuthCookie) {
        this._Html = html;
        this._TestAuthCookie = testAuthCookie;
    }

    public Task ExecuteAsync(HttpContext httpContext) {
        if (this._TestAuthCookie is { Length: > 0 } testAuthCookie)
            httpContext.Response.Cookies.Append("x-TestAuth", testAuthCookie, new CookieOptions() {
                Secure = false, /* for testing using http */
                HttpOnly = true
            });
        httpContext.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_Html);
        return httpContext.Response.WriteAsync(_Html);
    }
}