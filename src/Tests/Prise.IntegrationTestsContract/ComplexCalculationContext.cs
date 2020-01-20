using System;

namespace Prise.IntegrationTestsContract
{
    // Plugin Parameter types are not required to be serializable, Newtonsoft JSON takes care of this
    public class ComplexCalculationContext
    {
        public CalculationContext[] Calculations { get; set; }
    }
}