using JobManagementSystem.Entities;

namespace JobManagementSystem.DataAccess;

public static class SeedDataExtensionMethod
{
    public static void SeedData(this JobManagementContext context)
    {
        var tr1 = new TaxRegime() { Id = 1, CountryCode = "GBR", Description = "Under British tax", MinimumThreshold = 10000M, TaxRate = 0.2M };
        var tr2 = new TaxRegime() { Id = 2, CountryCode = "USA", Description = "Overseas - United States", MinimumThreshold = 1000M, TaxRate = 0.14M };
        var tr3 = new TaxRegime() { Id = 3, CountryCode = "FRA", Description = "Overseas - France", MinimumThreshold = 5000M, TaxRate = 0.24M };
        var tr4 = new TaxRegime() { Id = 4, CountryCode = "JPN", Description = "Overseas - Japan", MinimumThreshold = 7000M, TaxRate = 0.18M };

        context.TaxRegimes.AddRange(tr1, tr2, tr3, tr4);

        var jc1 = new JobCategory() { Id = 1, Description = "Management" };
        var jc2 = new JobCategory() { Id = 2, Description = "Account Management" };
        var jc3 = new JobCategory() { Id = 3, Description = "Printing" };

        context.JobCategories.AddRange(jc1, jc2, jc3);

        var jr1 = new JobRole() { Id = 1, Description = "CEO" };
        var jr2 = new JobRole() { Id = 2, Description = "CFO" };
        var jr3 = new JobRole() { Id = 3, Description = "Senior Account Manager" };
        var jr4 = new JobRole() { Id = 4, Description = "Regional Account Manager" };
        var jr5 = new JobRole() { Id = 5, Description = "Account Manager" };
        var jr6 = new JobRole() { Id = 6, Description = "Digital Printer" };
        var jr7 = new JobRole() { Id = 7, Description = "Lithographic Printer" };
        var jr8 = new JobRole() { Id = 8, Description = "Ink Technician" };
        var jr9 = new JobRole() { Id = 9, Description = "Maintenance" };

        context.JobRoles.AddRange(jr1, jr2, jr3, jr4, jr5, jr6, jr7, jr8, jr9);

        var e1 = new Employee() { Id = 1, Name = "Adam Appleby", Email = "aa@company.com", JobRoleId = jr1.Id };
        var e2 = new Employee() { Id = 2, Name = "Betty Black", Email = "bb@company.com", JobRoleId = jr2.Id };
        var e3 = new Employee() { Id = 3, Name = "Catherine Chatsworth", Email = "cc@company.com", JobRoleId = jr3.Id };
        var e4 = new Employee() { Id = 4, Name = "Dennis Drupt", Email = "dd@company.com", JobRoleId = jr4.Id };
        var e5 = new Employee() { Id = 5, Name = "Eric Enest", Email = "ee@company.com", JobRoleId = jr5.Id };
        var e6 = new Employee() { Id = 6, Name = "Fiona Flough", Email = "ff@company.com", JobRoleId = jr5.Id };
        var e7 = new Employee() { Id = 7, Name = "Gina Gerry", Email = "gg@company.com", JobRoleId = jr6.Id };
        var e8 = new Employee() { Id = 8, Name = "Henry Hatter", Email = "hh@company.com", JobRoleId = jr6.Id };
        var e9 = new Employee() { Id = 9, Name = "Ingrid Irvine", Email = "ii@company.com", JobRoleId = jr7.Id };
        var e10 = new Employee() { Id = 10, Name = "John Jacks", Email = "jj@company.com", JobRoleId = jr7.Id };
        var e11 = new Employee() { Id = 11, Name = "Kiara Klive", Email = "kk@company.com", JobRoleId = jr7.Id };
        var e12 = new Employee() { Id = 12, Name = "Liam Lovegood", Email = "ll@company.com", JobRoleId = jr8.Id };
        var e13 = new Employee() { Id = 13, Name = "Marge Mavis", Email = "mm@company.com", JobRoleId = jr9.Id };

        context.Employees.AddRange(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13);

        var j1 = new Job() { Id = 1, Description = "Organise company culture meeting", JobCategoryId = jc1.Id };
        var j2 = new Job() { Id = 2, Description = "Year end accounts", JobCategoryId = jc1.Id };
        context.Jobs.AddRange(j1, j2);

        e1.AssignedJobs.Add(j1);
        e2.AssignedJobs.Add(j2);

        var ti1 = new TaxInformation() { Id = 1, Description = "Internal", TaxRegimeId = tr1.Id, JobId = j1.Id };
        var ti2 = new TaxInformation() { Id = 2, Description = "UK", TaxRegimeId = tr1.Id, JobId = j2.Id };
        var ti3 = new TaxInformation() { Id = 3, Description = "US", TaxRegimeId = tr2.Id, JobId = j2.Id };
        var ti4 = new TaxInformation() { Id = 4, Description = "EU", TaxRegimeId = tr3.Id, JobId = j2.Id };
        var ti5 = new TaxInformation() { Id = 5, Description = "JP", TaxRegimeId = tr4.Id, JobId = j2.Id };

        context.TaxInformations.AddRange(ti1, ti2, ti3, ti4, ti5);

        context.SaveChanges();
    }
}
