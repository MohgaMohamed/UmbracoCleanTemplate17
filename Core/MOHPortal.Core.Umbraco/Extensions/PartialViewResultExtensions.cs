using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;

namespace MOHPortal.Core.Umbraco.Extensions
{
    public static class PartialViewResultExtensions
    {
        public async static Task<string> ConvertToString(
            this PartialViewResult partialView,
            ControllerContext controllerContext,
            ViewDataDictionary viewData,
            IRazorViewEngine ViewEngine,
            ITempDataProvider TempDataProvider)
        {
            if (string.IsNullOrWhiteSpace(partialView.ViewName))
            {
                return string.Empty;
            }

            IView? view = default;
            ViewEngineResult getViewResult = ViewEngine.GetView(executingFilePath: null, viewPath: partialView.ViewName, isMainPage: true);
            if (getViewResult.Success)
            {
                view = getViewResult.View;
            }

            ViewEngineResult findViewResult = ViewEngine.FindView(controllerContext, partialView.ViewName, isMainPage: true);
            if (findViewResult.Success)
            {
                view = findViewResult.View;
            }

            if (view is null)
            {
                return string.Empty;
            }

            await using StringWriter output = new();
            ViewContext viewContext = new(
                controllerContext,
                view,
                viewData,
                new TempDataDictionary(
                    controllerContext.HttpContext,
                    TempDataProvider),
                output,
                new HtmlHelperOptions()
            );

            await view.RenderAsync(viewContext);
            return output.ToString();
        }
    }
}
