using System.Collections.Generic;

namespace PniaApi.Models.Output
{
    public class PrefixBusinessCounts
    {
        public string Prefix { get; set; }
        public List<BusinessCount> BusinessCountList { get; set; }
    }
}
