using Microsoft.EntityFrameworkCore;

namespace SalaRemota.Infrastructure.Persistence;

public sealed class SalaRemotaDbContext(DbContextOptions<SalaRemotaDbContext> options)
    : DbContext(options);
