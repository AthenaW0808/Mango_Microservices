using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
 * builder.Services.AddControllersWithViews():
 * 这行代码将MVC（Model-View-Controller）
 * 控制器和视图服务添加到应用程序的依赖注入容器中。
 * 这样，您可以在应用程序中使用控制器和视图来处理
 * Web请求和生成HTML视图。
 */
builder.Services.AddControllersWithViews();

/*
 * builder.Services.AddHttpClient():
 * 这行代码将HttpClient服务添加到依赖注入容器中。
 * HttpClient是用于发送HTTP请求和处理响应的类，
 * 它可以在应用程序中被注入和使用。
 * 在您的应用程序中，通过IHttpClientFactory接口来创建和
 * 管理HttpClient实例。
 */
builder.Services.AddHttpClient();

/*
 * builder.Services.AddHttpContextAccessor():
 * 这行代码将HttpContextAccessor服务添加到依赖注入容器中。
 * HttpContextAccessor是一个帮助类，
 * 用于在应用程序中访问当前HTTP请求的上下文信息。
 * 通过将它添加到容器中，
 * 您可以在需要的地方注入IHttpContextAccessor接口，
 * 并通过它来访问请求的相关信息
 */
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();

SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();