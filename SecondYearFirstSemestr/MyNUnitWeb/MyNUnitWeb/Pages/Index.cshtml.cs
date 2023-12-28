using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNUnit;
using System.Reflection;
using MyNUnitWeb.Data;

namespace MyNUnitWeb.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {

        private readonly AssemblyDbContext _context;

        public IndexModel(AssemblyDbContext _context) => this._context = _context;

        public AssemblyData LastAssembly { get; set; }

		public List<IFormFile> Files { get; set; }

		public IList<Data.AssemblyData> AssembliesData { get; private set; } = new List<Data.AssemblyData>();
		public void OnGet()
		{
			AssembliesData = _context.AssembliesData.OrderBy(p => p.AssemblyDataId).ToList();
		}

		public async Task<IActionResult> OnPostAsync()
        {
            await Console.Out.WriteLineAsync("Wow");
            if (Files != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    foreach (var file in Files)
                    {
                        file.CopyTo(memoryStream);
                        var data = memoryStream.ToArray();
                        var assembly = Assembly.Load(data);
                        var result = TestRunner.RunTests(assembly).Result;

                        AssemblyData assemblyData = new();
                        assemblyData.Name = result.Name;
                        assemblyData.AmountOfTest = result.GetAmountOfTest();
                        assemblyData.AmountOfSucceedTests = result.GetAmountOfSucceedTest();
                        assemblyData.AmountOfFailTests = result.GetAmountOfFailedTest();
                        assemblyData.AmountOfIgnoredTests = result.GetAmountOfIgnoreTest();
						_context.AssembliesData.Add(assemblyData);
                        LastAssembly = assemblyData;
						await _context.SaveChangesAsync();
						foreach (var method in result.GetAllMethods())
                        {
                            var assemblyDataDb = _context.AssembliesData.Find(assemblyData.AssemblyDataId);
                            MethodData methodData = new();
                            methodData.Name = method.Name;
                            methodData.IsSucceed = method.Succeed;
                            methodData.IgnoreReason = method.Ignore;
                            methodData.Time = method.Time;
                            methodData.AssemblyDataId = assemblyData.AssemblyDataId;
                            _context.Add(methodData);
                            _context.SaveChanges();
                        }
					}
				}
            }
			return RedirectToPage("./Index");
		}
    }
}
