namespace Microsoft.AspNetCore.Builder;

public static class AngularFileServiceExtension {
    public static WebApplication UseAngularFileService(this WebApplication app) {
        var angularFileService = app.Services.GetRequiredService<AngularFileService>();
        angularFileService.Initialze();
        app.Use(angularFileService.Execute);
        app.UseStaticFiles();
        return app;
    }
}