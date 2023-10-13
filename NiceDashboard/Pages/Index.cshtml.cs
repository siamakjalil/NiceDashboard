using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NiceDashboard.Models;
using NiceDashboard.TagHelpers;

namespace NiceDashboard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<FakeData.Data> FakeDataList { get; set; }
        public int Count { get; set; }
        public PagingTagHelper.PagingInfo PagingInfo { get; set; }

        public async Task<IActionResult> OnGet(int pageId=1)
        {
            var data = await FakeData.GetFakeList(0, 20);
            FakeDataList = data.Item1;
            Count = data.Item2;
            await PagingModel(pageId);
            return Page();
        }

        private async Task PagingModel(int pageId)
        {
            PagingInfo = new PagingTagHelper.PagingInfo()
            {
                CurrentPage = pageId,
                ItemPerPage = 20,
                TotalItems = Count, 
            };
        }
    }
}