using System;
using System.Collections.Generic;
using System.Text;

namespace Prise.Mvc.Infrastructure
{
    public interface IPriseActionDescriptorChangeProvider
    {
        void TriggerPluginChanged();
    }
}
