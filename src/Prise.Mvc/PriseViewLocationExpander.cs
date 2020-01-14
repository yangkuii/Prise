using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Prise.Mvc
{
    public class PriseViewLocationExpander : IViewLocationExpander
    {

        /// <summary>
        /// Used to specify the locations that the view engine should search to 
        /// locate views.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewLocations"></param>
        /// <returns></returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            //{2} is area, {1} is controller,{0} is the action
            //D:\src\sources\Prise.Examples\APIWithControllersAsPlugins\MyHost\bin\Debug\netcoreapp3.1\Plugins\DashboardControllerPlugin\Views
            string[] locations = new string[] { "/bin/Debug/netcoreapp3.1/Plugins/DashboardControllerPlugin/Views/{2}/{1}/{0}.cshtml" };
            return locations.Union(viewLocations);          //Add mvc default locations after ours
        }


        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["customviewlocation"] = nameof(PriseViewLocationExpander);
        }
    }
}
