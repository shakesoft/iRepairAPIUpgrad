using System.Linq;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using BEZNgCore.Editions;
using BEZNgCore.EntityFrameworkCore;

namespace BEZNgCore.Migrations.Seed.Tenants;

public class DefaultTenantBuilder
{
    private readonly BEZNgCoreDbContext _context;

    public DefaultTenantBuilder(BEZNgCoreDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateDefaultTenant();
    }

    private void CreateDefaultTenant()
    {
        //Default tenant

        //var defaultTenant = _context.Tenants.IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == MultiTenancy.Tenant.DefaultTenantName);
        //if (defaultTenant == null)
        //{
        //    defaultTenant = new MultiTenancy.Tenant(AbpTenantBase.DefaultTenantName, AbpTenantBase.DefaultTenantName);

        //    var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
        //    if (defaultEdition != null)
        //    {
        //        defaultTenant.EditionId = defaultEdition.Id;
        //    }

        //    _context.Tenants.Add(defaultTenant);
        //    _context.SaveChanges();
        //}
        const string DefaultTenantName = "TEST"; //"TCOC";// "HGCB";// "TCOH";//"HGCM";//"HGCAW";// "JCHGC";// "HGCH";//"HGCL";// HCO";//"Chancellor";
        var defaultTenant = _context.Tenants.IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == MultiTenancy.Tenant.DefaultTenantName);
        if (defaultTenant == null)
        {
            defaultTenant = new MultiTenancy.Tenant(DefaultTenantName, DefaultTenantName);

            var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                defaultTenant.EditionId = defaultEdition.Id;
            }

            _context.Tenants.Add(defaultTenant);
            _context.SaveChanges();
        }
    }
}

