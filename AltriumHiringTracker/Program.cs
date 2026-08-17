using AltriumHiringTracker.Components;
using AltriumHiringTracker.Components.Account;
using AltriumHiringTracker.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider,
    IdentityRevalidatingAuthenticationStateProvider>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>,
    IdentityNoOpEmailSender>();

var app = builder.Build();

Console.WriteLine("DEBUG: Starting role/user seeding...");

// ===== Seed roles =====
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "HR", "Interviewer", "Management" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// ===== Seed users + assign roles =====
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();

    // 1) Existing HR test user, if it already exists.
    var existingHrUser = await userManager.FindByEmailAsync("test2@test.com");

    if (existingHrUser != null &&
        !await userManager.IsInRoleAsync(existingHrUser, "HR"))
    {
        await userManager.AddToRoleAsync(existingHrUser, "HR");
    }

    // 2) HR / Recruiter account.
    const string hrEmail = "hr@altrium.com";
    const string hrPassword = "Hr@2020";

    var hrUser = await userManager.FindByEmailAsync(hrEmail);

    if (hrUser == null)
    {
        hrUser = new ApplicationUser
        {
            UserName = hrEmail,
            Email = hrEmail,
            EmailConfirmed = true
        };

        var hrCreateResult = await userManager.CreateAsync(hrUser, hrPassword);

        if (hrCreateResult.Succeeded)
        {
            await userManager.AddToRoleAsync(hrUser, "HR");
        }
        else
        {
            Console.WriteLine("DEBUG: Failed to create HR user: " +
                string.Join(", ", hrCreateResult.Errors.Select(
                    error => error.Description)));
        }
    }
    else
    {
        var hrToken = await userManager.GeneratePasswordResetTokenAsync(hrUser);

        var hrResetResult = await userManager.ResetPasswordAsync(
            hrUser,
            hrToken,
            hrPassword);

        if (!hrResetResult.Succeeded)
        {
            Console.WriteLine("DEBUG: Failed to reset HR password: " +
                string.Join(", ", hrResetResult.Errors.Select(
                    error => error.Description)));
        }

        if (!await userManager.IsInRoleAsync(hrUser, "HR"))
        {
            await userManager.AddToRoleAsync(hrUser, "HR");
        }
    }

    // 3) Original Interviewer test account.
    const string interviewerEmail = "interviewer@altrium.com";
    const string interviewerPassword = "Interviewer@2020";

    var interviewerUser = await userManager.FindByEmailAsync(interviewerEmail);

    if (interviewerUser == null)
    {
        interviewerUser = new ApplicationUser
        {
            UserName = interviewerEmail,
            Email = interviewerEmail,
            EmailConfirmed = true
        };

        var interviewerCreateResult = await userManager.CreateAsync(
            interviewerUser,
            interviewerPassword);

        if (interviewerCreateResult.Succeeded)
        {
            await userManager.AddToRoleAsync(interviewerUser, "Interviewer");
        }
        else
        {
            Console.WriteLine("DEBUG: Failed to create Interviewer user: " +
                string.Join(", ", interviewerCreateResult.Errors.Select(
                    error => error.Description)));
        }
    }
    else
    {
        var interviewerToken =
            await userManager.GeneratePasswordResetTokenAsync(interviewerUser);

        var interviewerResetResult = await userManager.ResetPasswordAsync(
            interviewerUser,
            interviewerToken,
            interviewerPassword);

        if (!interviewerResetResult.Succeeded)
        {
            Console.WriteLine("DEBUG: Failed to reset Interviewer password: " +
                string.Join(", ", interviewerResetResult.Errors.Select(
                    error => error.Description)));
        }

        if (!await userManager.IsInRoleAsync(interviewerUser, "Interviewer"))
        {
            await userManager.AddToRoleAsync(interviewerUser, "Interviewer");
        }
    }

    // 4) Interviewer accounts used in the Interview Scheduling dropdown.
    var interviewerAccounts = new[]
    {
        new
        {
            Name = "Interviewer 1",
            Email = "interviewer1@altrium.com"
        },
        new
        {
            Name = "Interviewer 2",
            Email = "interviewer2@altrium.com"
        }
    };

    foreach (var account in interviewerAccounts)
    {
        var user = await userManager.FindByEmailAsync(account.Email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = account.Email,
                Email = account.Email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(
                user,
                interviewerPassword);

            if (!createResult.Succeeded)
            {
                Console.WriteLine(
                    $"DEBUG: Failed to create {account.Name}: " +
                    string.Join(", ", createResult.Errors.Select(
                        error => error.Description)));

                continue;
            }
        }
        else
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetResult = await userManager.ResetPasswordAsync(
                user,
                token,
                interviewerPassword);

            if (!resetResult.Succeeded)
            {
                Console.WriteLine(
                    $"DEBUG: Failed to reset {account.Name} password: " +
                    string.Join(", ", resetResult.Errors.Select(
                        error => error.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, "Interviewer"))
        {
            await userManager.AddToRoleAsync(user, "Interviewer");
        }
    }

    // 5) Management / Leadership account.
    const string managementEmail = "management@altrium.com";
    const string managementPassword = "Management@2020";

    var managementUser = await userManager.FindByEmailAsync(managementEmail);

    if (managementUser == null)
    {
        managementUser = new ApplicationUser
        {
            UserName = managementEmail,
            Email = managementEmail,
            EmailConfirmed = true
        };

        var managementCreateResult = await userManager.CreateAsync(
            managementUser,
            managementPassword);

        if (managementCreateResult.Succeeded)
        {
            await userManager.AddToRoleAsync(managementUser, "Management");
        }
        else
        {
            Console.WriteLine("DEBUG: Failed to create Management user: " +
                string.Join(", ", managementCreateResult.Errors.Select(
                    error => error.Description)));
        }
    }
    else
    {
        var managementToken =
            await userManager.GeneratePasswordResetTokenAsync(managementUser);

        var managementResetResult = await userManager.ResetPasswordAsync(
            managementUser,
            managementToken,
            managementPassword);

        if (!managementResetResult.Succeeded)
        {
            Console.WriteLine("DEBUG: Failed to reset Management password: " +
                string.Join(", ", managementResetResult.Errors.Select(
                    error => error.Description)));
        }

        if (!await userManager.IsInRoleAsync(managementUser, "Management"))
        {
            await userManager.AddToRoleAsync(managementUser, "Management");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();