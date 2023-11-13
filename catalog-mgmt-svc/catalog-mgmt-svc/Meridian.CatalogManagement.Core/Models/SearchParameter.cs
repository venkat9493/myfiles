using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meridian.CatalogManagement.Core.Models
{
    public class SearchParameter
    {
        public string SearchText { get; set; }
        public string Sortby { get; set; }
        public string Category { get; set; }
        public SearchFilterCategory filterCatagory { get; set; }
    }
}
