using System.IO;
using MD.CMS.Template;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory()
});

builder.Services.Configure<AngularFusePathOptions>(builder.Configuration.GetSection("Template"));

var app = builder.Build();

app.UseMiddleware<AngularFusePathMiddleware>();

var rootProvider = new PhysicalFileProvider(app.Environment.ContentRootPath);
app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = rootProvider, RequestPath = PathString.Empty });
app.UseStaticFiles(new StaticFileOptions { FileProvider = rootProvider, RequestPath = PathString.Empty });

// Angular (Material) build output: npm run build in modern-app → wwwroot/modern, served as /modern/
var modernPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "modern");
if (Directory.Exists(modernPath))
{
    var modernProvider = new PhysicalFileProvider(modernPath);
    app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = modernProvider, RequestPath = "/modern" });
    app.UseStaticFiles(new StaticFileOptions { FileProvider = modernProvider, RequestPath = "/modern" });
    app.MapFallbackToFile(
        pattern: "/modern/{**clientRoute:nonfile}",
        filePath: "index.html",
        new StaticFileOptions { FileProvider = modernProvider });
}

app.Run();
