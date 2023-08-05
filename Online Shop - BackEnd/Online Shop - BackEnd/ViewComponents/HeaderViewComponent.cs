using Microsoft.AspNetCore.Mvc;
using Online_Shop___BackEnd.Services;
using Online_Shop___BackEnd.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Shop___BackEnd.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly LayoutService _layoutService;
        public HeaderViewComponent(LayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, string> settings = await _layoutService.GetDatasFromSetting();

            HeaderVM model = new HeaderVM
            {
                Settings = settings,
            };

            return await Task.FromResult(View(model));

        }
    }
}
