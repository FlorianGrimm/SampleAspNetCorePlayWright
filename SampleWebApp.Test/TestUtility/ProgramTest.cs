using EnumerableAsyncProcessor.RunnableProcessors.ResultProcessors;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Security.Claims;
namespace SampleWebApp.Test.TestUtility;

public class ProgramTest : Program {
    public static void Enable() {
        MainRun = (args) => {
            var result = new ProgramTest();
            return result.Run(args);
        };
    }
    public ProgramTest() : base() {
    }
    protected override void ConfigureWebApplicationServices(WebApplicationBuilder builder) {
        builder.Services.AddDataProtection()
            .UseEphemeralDataProtectionProvider();
        base.ConfigureWebApplicationServices(builder);
    }
    protected override void ConfigureAddAuthentication(WebApplicationBuilder builder) {
        // base.ConfigureAddAuthentication(builder);
        var authenticationBuilder
            = builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
        authenticationBuilder
                .AddCookie(options => {
                    options.ExpireTimeSpan = TimeSpan.FromHours(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                    options.ForwardAuthenticate = TestAuthHandler.AuthenticationScheme;
                });
        authenticationBuilder
                .AddScheme<TestAuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.AuthenticationScheme, options => { });
        
        builder.Services.AddSingleton<TestAuthenticationUsers>();
    }
    protected override void ConfigureUseAuthentication(WebApplication app) {
        app.UseCookiePolicy(new CookiePolicyOptions() {
            HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.SameAsRequest,
            CheckConsentNeeded = (_) => false
        });
        base.ConfigureUseAuthentication(app);
    }
    protected override void ConfigureHttpRequestPipeline(WebApplication app) {
        app.MapGet("/test/ping", () => "/test/pong").AllowAnonymous();
        app.MapGet("/test/sign-in", async (HttpContext httpContext) => {
            if (httpContext.User.Identity is { IsAuthenticated:true } identity) {
                //OK
            } else {
                var user = httpContext.Request.Query["user"].FirstOrDefault();
                var password = httpContext.Request.Query["password"].FirstOrDefault();
                if (httpContext.RequestServices.GetRequiredService<TestAuthenticationUsers>()
                        .GetUserClaims(user, password) is { } claims) {

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await httpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    identity = claimsIdentity;
                } else {
                    identity = null;
                }
            }
            if (identity is { IsAuthenticated: true }
                && identity.Name is { Length:>0} name) {
                return Results.Content($"<html><body><a href='/' data-testid='home'>{HtmlEncoder.Default.Encode(name)}</a></body></html>", "text/html", Encoding.UTF8);
            } else {
                return Results.Content($"<html><body>Anonymous</body></html>", "text/html", Encoding.UTF8);
            }
        }).AllowAnonymous();
        base.ConfigureHttpRequestPipeline(app);
    }
    protected override void ConfigureMapEndpoints(WebApplication app) {
        base.ConfigureMapEndpoints(app);
    }
}
