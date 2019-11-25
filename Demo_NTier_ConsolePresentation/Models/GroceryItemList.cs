using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Demo_NTier_XmlJsonData.Models
{
    public class GroceryItemsList
    {
        //[JsonProperty("Groceries")]
        public List<GroceryItem> GroceryList { get; set; }
    }
}
