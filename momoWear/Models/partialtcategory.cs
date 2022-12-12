using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace momoWear.Models
{
    [MetadataType(typeof(tcategoryMetaData))]
    public partial class tcategory
    {
        public class tcategoryMetaData
        {
            public int fid { get; set; }
            [Required(ErrorMessage = "必填")]
            [DisplayName("類別代號")]
            public string fcategoryID { get; set; }
            [Required(ErrorMessage = "必填")]
            [DisplayName("類別名稱")]
            public string fcategoryName { get; set; }
            [DisplayName("修改日期")]
            public Nullable<System.DateTime> fmodifiedDate { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<tclothes> tclothes { get; set; }

        }
    }
}