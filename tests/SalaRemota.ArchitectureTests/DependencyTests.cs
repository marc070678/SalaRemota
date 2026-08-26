using System.Reflection;
using SalaRemota.Domain;
using Xunit;

namespace SalaRemota.ArchitectureTests;

public sealed class DependencyTests
{
    [Fact]
    public void Domain_has_no_forbidden_architecture_dependencies()
    {
        var forbiddenPrefixes = new[]
        {
            "Microsoft.AspNetCore",
            "Microsoft.EntityFrameworkCore",
            "Npgsql",
            "SalaRemota.Infrastructure",
            "SalaRemota.Api"
        };

        var references = typeof(AssemblyMarker).Assembly.GetReferencedAssemblies();

        Assert.DoesNotContain(references, reference =>
            forbiddenPrefixes.Any(prefix =>
                reference.Name?.StartsWith(prefix, StringComparison.Ordinal) is true));
    }
}
