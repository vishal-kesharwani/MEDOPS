namespace MedOps.Infrastructure.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MedOps.Domain.Entities;
using MedOps.Domain.Enums;
using MedOps.Domain.ValueObjects;

public static class SeedData
{
    public static async System.Threading.Tasks.Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Microsoft.AspNetCore.Identity.IdentityRole<Guid>>>();
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SeedData");
        var dbContext = serviceProvider.GetRequiredService<MedOpsDbContext>();

        string[] roles = ["Admin", "User"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole<Guid>(role));
                logger.LogInformation("Created role: {Role}", role);
            }
        }

        var adminEmail = "admin@medops.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                logger.LogInformation("Created admin user: {Email}", adminEmail);
            }
            else
            {
                foreach (var error in result.Errors)
                    logger.LogError("Failed to create admin user: {Error}", error.Description);
                return;
            }
        }

        if (await dbContext.Departments.AnyAsync()) return;

        var departments = new Department[]
        {
            new("Clinical Operations", "Manages clinical trial operations and site coordination"),
            new("Regulatory Affairs", "Handles regulatory submissions and compliance"),
            new("Data Management", "Manages clinical data collection and analysis"),
            new("Quality Assurance", "Ensures quality standards and GCP compliance"),
        };
        dbContext.Departments.AddRange(departments);
        await dbContext.SaveChangesAsync();

        var sites = new Site[]
        {
            new("Cincinnati Main Campus", "Headquarters clinical research center",
                new Address { Street = "5375 Medpace Way", City = "Cincinnati", State = "Ohio", Country = "USA", ZipCode = "45227" },
                new ContactInfo { Email = "cincinnati@medpace.com", Phone = "+1 513-579-9911" },
                adminUser.Id),
            new("Mumbai Research Center", "India-based clinical research facility",
                new Address { Street = "TTC Industrial Area", City = "Navi Mumbai", State = "Maharashtra", Country = "India", ZipCode = "400710" },
                new ContactInfo { Email = "mumbai@medpace.com", Phone = "+91 22 6280 5000" },
                adminUser.Id),
            new("London Satellite Office", "UK-based regulatory and data management office",
                new Address { Street = "1 London Bridge", City = "London", State = "England", Country = "UK", ZipCode = "SE1 9GF" },
                new ContactInfo { Email = "london@medpace.com", Phone = "+44 20 7946 0958" },
                adminUser.Id),
        };
        dbContext.Sites.AddRange(sites);
        await dbContext.SaveChangesAsync();

        var studies = new Study[]
        {
            new("ONCO-2024 Phase III", "Phase III oncology study for novel immunotherapy agent", adminUser.Id),
            new("CARDIO-2024 Phase II", "Phase II cardiovascular outcomes trial", adminUser.Id),
            new("NEURO-2023 Phase I", "Phase I neurology dose-finding study", adminUser.Id),
        };
        dbContext.Studies.AddRange(studies);
        await dbContext.SaveChangesAsync();

        var tasks = new MedOps.Domain.Entities.Task[]
        {
            new("Submit IRB amendments", "Prepare and submit protocol amendments to IRB", adminUser.Id, adminUser.Id),
            new("Database lock preparation", "Complete data cleaning and query resolution before database lock", adminUser.Id, adminUser.Id),
            new("Site monitoring visit", "Conduct on-site monitoring visit for source data verification", adminUser.Id, adminUser.Id),
            new("Update study budget", "Review and update study budget allocation", adminUser.Id, adminUser.Id),
        };
        dbContext.Tasks.AddRange(tasks);
        await dbContext.SaveChangesAsync();

        var requests = new Request[]
        {
            new("Budget increase for ONCO-2024", "Request additional funding for extended enrollment period", adminUser.Id, "High"),
            new("New site activation - Tokyo", "Activate new research site in Tokyo, Japan", adminUser.Id, "Medium"),
            new("Protocol deviation report", "Deviation report for missed visit window", adminUser.Id, "Low"),
        };
        dbContext.Requests.AddRange(requests);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seed data created: {Departments} departments, {Sites} sites, {Studies} studies, {Tasks} tasks, {Requests} requests",
            departments.Length, sites.Length, studies.Length, tasks.Length, requests.Length);
    }
}