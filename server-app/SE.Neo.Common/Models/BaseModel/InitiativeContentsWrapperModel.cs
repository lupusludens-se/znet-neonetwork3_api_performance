using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Neo.Common.Models.Shared
{
    public class InitiativeContentsWrapperModel<T>
    {
       
            public IEnumerable<T> DataList { get; set; }
            public int Count { get; set; }
            public int NewRecommendationsCount { get; set; }
        
    }
}
