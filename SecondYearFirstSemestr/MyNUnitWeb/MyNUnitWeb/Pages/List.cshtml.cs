using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNUnitWeb.Data;
namespace MyNUnitWeb.Pages
{
    public class ListModel : PageModel
    {
		private readonly AssemblyDbContext context;
		public ListModel(AssemblyDbContext context)
		=> this.context = context;
		public IList<Data.AssemblyData> AssembliesData { get; private set; } = new List<Data.AssemblyData>();
		public void OnGet()
		{
			AssembliesData = context.AssembliesData.OrderBy(p => p.AssemblyDataId).ToList();
		}
	}
}
