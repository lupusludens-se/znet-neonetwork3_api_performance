using SE.Neo.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Neo.Common.Models
{
    public class InitiativeContentRecommendationActivityRequest
    {
        public int InitiativeId { get; set; }
        public InitiativeModules ContentType
        {
            get; set;
        }
    }
}
