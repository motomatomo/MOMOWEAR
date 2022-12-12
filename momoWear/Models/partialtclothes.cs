﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace momoWear.Models
{
    [MetadataType(typeof(tclothesMetaData))]
    public partial class tclothes
    {
        public class tclothesMetaData 
        {
            public int fid { get; set; }
            [DisplayName("對外代號")]
            public string fserialNumber { get; set; }

            [DisplayName("類別代號")]
            public Nullable<int> fcategoryID { get; set; }

            [Required(ErrorMessage = "必填")]
            [DisplayName("品名") ] 
            public string fname { get; set; }

            [DisplayName("顏色")]
            public string fcolor { get; set; }

            [DisplayName("尺寸")]
            public string fsize { get; set; }

            [DisplayName("數量")]
            [Range(0,150,ErrorMessage ="數量在0-150之間") ]
            public Nullable<int> fquentity { get; set; }

            [DisplayName("描述")]
            public string fdescribe { get; set; }

            [DisplayName("已銷售量")]
            public Nullable<int> fsalesVolume { get; set; }
            [DisplayName("價格")]
            public Nullable<decimal> fprice { get; set; }

            [DisplayName("販售日")]
            public Nullable<System.DateTime> fsalesdate { get; set; }

            [DisplayName("可銷售庫存")]
            public Nullable<short> fsafetyStockLevel { get; set; }

            [DisplayName("修改日")]
            public Nullable<System.DateTime> fmodifiedDate { get; set; }

            [DisplayName("品牌")]
            public string fbrand { get; set; }

            [DisplayName("照片")]
            public string fphotoPath { get; set; }

            [DisplayName("內類別代號")]
            public virtual tcategory tcategory { get; set; }

            //public HttpPostedFileBase photo { get; set; }


        }
    }
}