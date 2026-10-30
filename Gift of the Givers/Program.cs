
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
    // DATABASE - AZURE SQL
    // ==========================================

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        // Integration tests can replace this
        // configuration with an in-memory database.

        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions =>
            {
                // Retry temporary Azure SQL failures.
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            }
        );
    });

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
    // DATABASE INITIALISATION
    // ==========================================

    // Integration tests manage their own database.
    // Never seed the test employee in production.

    if (!app.Environment.IsEnvironment("Testing"))
    {
        try
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var dbContext =
                services.GetRequiredService<ApplicationDbContext>();

            // Initialise database if accessible.
            // A temporary failure must not stop
            // the entire website from starting.
            dbContext.Database.EnsureCreated();

            var roleManager =
                services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles =
            {
                "Employee",
                "Donor"
            };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult =
                        await roleManager.CreateAsync(
                            new IdentityRole(role)
                        );

                    if (!roleResult.Succeeded)
                    {
                        throw new InvalidOperationException(
                            "Could not create role: " + role
                        );
                    }
                }
            }

            // ==========================================
            // DEVELOPMENT TEST EMPLOYEE
            // ==========================================

            if (app.Environment.IsDevelopment())
            {
                var userManager =
                    services.GetRequiredService<UserManager<IdentityUser>>();

                string? employeeEmail =
                    app.Configuration["TestEmployee:Email"];

                string? employeePassword =
                    app.Configuration["TestEmployee:Password"];

                if (!string.IsNullOrWhiteSpace(employeeEmail) &&
                    !string.IsNullOrWhiteSpace(employeePassword))
                {
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

                        var createResult =
                            await userManager.CreateAsync(
                                employee,
                                employeePassword
                            );

                        if (!createResult.Succeeded)
                        {
                            throw new InvalidOperationException(
                                "Could not create test employee: " +
                                string.Join(
                                    "; ",
                                    createResult.Errors.Select(
                                        e => e.Description
                                    )
                                )
                            );
                        }
                    }

                    if (!await userManager.IsInRoleAsync(
                            employee,
                            "Employee"))
                    {
                        var roleResult =
                            await userManager.AddToRoleAsync(
                                employee,
                                "Employee"
                            );

                        if (!roleResult.Succeeded)
                        {
                            throw new InvalidOperationException(
                                "Could not assign Employee role."
                            );
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log startup database errors without
            // terminating the entire web application.
            app.Logger.LogError(
                ex,
                "Database initialisation failed. " +
                "The application will continue starting."
            );
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

    // ==========================================
    // INTEGRATION TEST SUPPORT
    // ==========================================

    public partial class Program
    {
    }
