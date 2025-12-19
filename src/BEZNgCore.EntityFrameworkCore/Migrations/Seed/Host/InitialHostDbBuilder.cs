using BEZNgCore.EntityFrameworkCore;

namespace BEZNgCore.Migrations.Seed.Host;

public class InitialHostDbBuilder
{
    private readonly BEZNgCoreDbContext _context;

    public InitialHostDbBuilder(BEZNgCoreDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        new DefaultEditionCreator(_context).Create();
        new DefaultLanguagesCreator(_context).Create();
        new HostRoleAndUserCreator(_context).Create();
        new DefaultSettingsCreator(_context).Create();

        _context.SaveChanges();
    }
}

