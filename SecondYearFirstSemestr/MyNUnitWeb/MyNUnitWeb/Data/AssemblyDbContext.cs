using Microsoft.EntityFrameworkCore;

namespace MyNUnitWeb.Data
{
	public class AssemblyDbContext: DbContext
	{
		public DbSet<MethodData> MethodsData { get; set; }

		public DbSet<AssemblyData> AssembliesData { get; set; }


		public AssemblyDbContext(DbContextOptions<AssemblyDbContext> options)
			:base(options) { }
	}
}
