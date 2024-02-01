using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Domain.ClassTypes
{
    public class PageInput
    {
        public int First { get; set; }
        public int Rows { get; set; }
        public string SortField { get; set; }
        public int SortOrder { get; set; }
        public IDictionary<object, object> Filters { get; set; }
        public int page { get; set; }
    }
}
