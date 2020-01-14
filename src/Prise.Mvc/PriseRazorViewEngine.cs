using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Prise.Mvc
{
    //public class PriseRazorViewEngine : RazorViewEngine
    //{
    //    private readonly string[] _customAreaFormats = new string[]
    //    {
    //    "/Views/{2}/{1}/{0}.cshtml"
    //    };

    //    public PriseRazorViewEngine(
    //        IRazorPageFactoryProvider pageFactory,
    //        IRazorPageActivator viewFactory,
    //        HtmlEncoder htmlEncoder,
    //        IOptions<RazorViewEngineOptions> optionsAccessor,
    //        RazorProject razorProject,
    //        ILoggerFactory loggerFactory,
    //        DiagnosticSource diagnosticSource)
    //        : base(pageFactory, viewFactory, optionsAccessor, viewLocationCache)
    //    {
    //    }

    //    public override IEnumerable<string> AreaViewLocationFormats =>
    //        _customAreaFormats.Concat(base.AreaViewLocationFormats);
    //}
}
