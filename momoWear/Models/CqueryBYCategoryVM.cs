using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace momoWear.Models
{
    public class CqueryBYCategoryVM
    {
        public int fid { get; set; }
        public Nullable<int> fcategoryID { get; set; }
        public string fcategoryName { get; set; }
        public string fserialNumber { get; set; }
        public string fname { get; set; }
        public string fcolor { get; set; }
        public string fsize { get; set; }
        public Nullable<int> fquentity { get; set; }
        public string fdescribe { get; set; }
        public Nullable<int> fsalesVolume { get; set; }
        public Nullable<decimal> fprice { get; set; }
        public Nullable<System.DateTime> fsalesdate { get; set; }
        public Nullable<short> fsafetyStockLevel { get; set; }
        public Nullable<System.DateTime> fmodifiedDate { get; set; }
        public string fbrand { get; set; }
        public string fphotoPath { get; set; }

       

        public HttpPostedFileBase photo { get; set; }
    }
}