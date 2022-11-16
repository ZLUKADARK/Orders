using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.ViewModels.Order
{
    public class DistinctValuesForSelect
    {
        public List<string> Name { get; set; }
        public List<string> Unit { get; set; }
        public List<string> Number { get; set; }
        public List<string> ProviderName { get; set; }
    }
}
