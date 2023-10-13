using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NiceDashboard.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PagingTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public PagingTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }
        public class PagingInfo
        {
            public int TotalItems { get; set; }
            public int ItemPerPage { get; set; }
            public int CurrentPage { get; set; } 
            public int Type { get; set; }
            public int TotalPage => (int)Math.Ceiling((decimal)TotalItems / ItemPerPage);

        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");
            if (PageModel.TotalPage>1)
            {
                if (PageModel.CurrentPage != 1)
                {
                    result.InnerHtml.AppendHtml(Builder(1, "First"));
                }
                for (int i = 2; i >= 1; i--)
                {
                    if (PageModel.CurrentPage - i < 1)
                    {
                        continue;
                    }
                    result.InnerHtml.AppendHtml(Builder(PageModel.CurrentPage - i, (PageModel.CurrentPage - i).ToString()));
                }
                for (int i = PageModel.CurrentPage; i <= PageModel.CurrentPage + 3; i++)
                {
                    if (i > PageModel.TotalPage)
                    {
                        break;
                    }
                    result.InnerHtml.AppendHtml(Builder(i, i.ToString()));
                }
                if (PageModel.CurrentPage != PageModel.TotalPage)
                {
                    result.InnerHtml.AppendHtml(Builder(PageModel.TotalPage, "Last"));
                }

            }

            output.Content.AppendHtml(result.InnerHtml);
        }
        public TagBuilder Builder(int i, string text)
        {
            TagBuilder tag = new TagBuilder("a"); 
            string method = PageAction + "?pageId=" + i+"&type="+ PageModel.Type;
            tag.Attributes["href"] = method;
            tag.AddCssClass(PageClass);
            tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
            tag.InnerHtml.Append(text);
            return tag;
        }
    }
}
