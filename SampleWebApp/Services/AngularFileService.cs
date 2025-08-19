using Microsoft.Extensions.FileProviders;

namespace Sample.WebApp.Services;
public class AngularFileService {
    private readonly IWebHostEnvironment _WebHostEnvironment;

    public AngularFileService(IWebHostEnvironment webHostEnvironment) {
        this._WebHostEnvironment = webHostEnvironment;
    }

    public void Initialze() {
        var webRootPath = this._WebHostEnvironment.WebRootPath;
        //var webRootProvider = new PhysicalFileProvider(webRootPath);
        var webRootProvider = this._WebHostEnvironment.WebRootFileProvider;
        var additionalPathProvider = new PhysicalFileProvider(
            System.IO.Path.Combine(webRootPath, "static/browser")
            );

        var compositeProvider = new CompositeFileProvider(
            webRootProvider,
            additionalPathProvider);

        // Update the default provider.
        this._WebHostEnvironment.WebRootFileProvider = compositeProvider;
    }

    public async Task Execute(HttpContext context, Func<Task> next) {
        var requestPath = context.Request.Path;
        if ((requestPath == "") || (requestPath == "/")) {
            var fileInfo = this._WebHostEnvironment.WebRootFileProvider.GetFileInfo("index.html");
            if (fileInfo.Exists) {
                context.Response.ContentType = "text/html";
                context.Response.ContentLength = fileInfo.Length;
                using (var fileStream = fileInfo.CreateReadStream()) {
                    await fileStream.CopyToAsync(context.Response.Body);
                    return;
                }
            }
        }
        await next.Invoke();
    }

}
