using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Neo.Common.Models.UserProfile
{
    public class SkillsByCategoryResponse
    {

        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategorySkillId { get; set; }
    }
}
