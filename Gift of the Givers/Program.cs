using Gift_of_the_Givers.Data;
using Gift_of_the_Givers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// ==========================================
// RAZOR PAGES
// ==========================================

builder.Services.AddRazorPages();


// ==========================================
// DATABASE
// ==========================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// ==========================================
// PROTOTYPE IN-MEMORY STORAGE
// ==========================================

builder.Services.AddSingleton<PrototypeStore>();


// ==========================================
// ASP.NET IDENTITY
// ==========================================

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// ==========================================
// LOGIN SETTINGS
// ==========================================

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
});


// ==========================================
// BUILD APPLICATION
// ==========================================

var app = builder.Build();


// ==========================================
// CREATE DATABASE / ROLES / EMPLOYEE
// ==========================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext =
        services.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.EnsureCreated();


    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager =
        services.GetRequiredService<UserManager<IdentityUser>>();


    string[] roles =
    {
        "Employee",
        "Donor"
    };


    foreach (string role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(
                new IdentityRole(role)
            );
        }
    }


    string employeeEmail =
        "employee@giftofthegivers.co.za";

    string employeePassword =
        "Employee123!";


    var employee =
        await userManager.FindByEmailAsync(employeeEmail);


    if (employee == null)
    {
        employee = new IdentityUser
        {
            UserName = employeeEmail,
            Email = employeeEmail,
            EmailConfirmed = true
        };


        var result =
            await userManager.CreateAsync(
                employee,
                employeePassword
            );


        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(
                employee,
                "Employee"
            );
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(
                employee,
                "Employee"))
        {
            await userManager.AddToRoleAsync(
                employee,
                "Employee"
            );
        }
    }
}


// ==========================================
// REQUEST PIPELINE
// ==========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();