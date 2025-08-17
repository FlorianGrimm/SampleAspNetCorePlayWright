using Microsoft.Extensions.FileProviders;

namespace SampleWebApp;

public class Program {
    public static Func<string[], Task> MainRun = ((string[] args) => (new Program()).Run(args));
    public static async Task Main(string[] args) {
        await MainRun(args);
    }
   
    public async Task Run(
        string[] args
        ) {
        var builder = WebApplication.CreateBuilder(args);

        this.ConfigureWebApplicationServices(builder);
        
        var app = builder.Build();

        this.ConfigureHttpRequestPipeline(app);
        this.ConfigureMapEndpoints(app);

        var taskRun = app.RunAsync();
        this.OnPostRunAsync(builder, app);
        await taskRun;
    }

    protected virtual void ConfigureWebApplicationServices(WebApplicationBuilder builder) {
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSingleton<AngularFileService>();
        
        this.ConfigureAddAuthentication(builder);
    }

    protected virtual void ConfigureAddAuthentication(WebApplicationBuilder builder) {
        builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();
    }


    protected virtual void ConfigureHttpRequestPipeline(WebApplication app) {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error");
        }

        this.ConfigureUseAuthentication(app);
        app.UseRouting();
        app.UseAuthorization();
    }

    protected virtual void ConfigureUseAuthentication(WebApplication app) {
        app.UseAuthentication();
    }

    protected virtual void ConfigureMapEndpoints(WebApplication app) {
        app.MapGet("/ping", () => "pong");
        app.UseAngularFileService();

        app.MapStaticAssets();
        //app.UseEndpoints(e => { });
    }

    protected virtual void OnPostRunAsync(WebApplicationBuilder builder, WebApplication app) {
    }

}
