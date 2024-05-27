
using EntityBasedAuth.Auth.Policies;
using EntityBasedAuth.Auth.Requirements;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/Accunt/Login");
        options.AccessDeniedPath = new PathString("/Account/Forbidden");
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(EmployeeReviewPolicies.ReadReviewPolicyName,
                      EmployeeReviewPolicies.GetReadReviewPolicy());
});

builder.Services.AddSingleton<IAuthorizationRequirement,
                              UserCanViewReviewRequirement>();
builder.Services.AddSingleton<IAuthorizationHandler, UserIsCreatorOfReviewHandler<UserCanViewReviewRequirement>>();
builder.Services.AddSingleton<IAuthorizationHandler, UserIsSubjectOfReviewHandler<UserCanViewReviewRequirement>>();
builder.Services.AddSingleton<IAuthorizationHandler, UserIsInHumanResourcesHandler>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
