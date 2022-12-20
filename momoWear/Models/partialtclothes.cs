using System;
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

            [DisplayName("內分類代號")]
            [Required(ErrorMessage = "分類代號為必選")]
            public Nullable<int> fcategoryID { get; set; }

            [Required(ErrorMessage = "品名為必填")]
            [DisplayName("品名") ] 
            public string fname { get; set; }

            [Required(ErrorMessage = "顏色為必填")]
            [DisplayName("顏色")]
            public string fcolor { get; set; }

            [Required(ErrorMessage = "尺寸為必填")]
            [DisplayName("尺寸")]
            public string fsize { get; set; }

            [DisplayName("數量")]
            [Required(ErrorMessage = "數量為必填")]
            [Range(0,200,ErrorMessage = "{0} 必須在 {1}和{2}之間.") ]
            public Nullable<int> fquentity { get; set; }

            [DisplayName("描述")]
            public string fdescribe { get; set; }

            [DisplayName("已銷售量")]
            [Range(0, 2000,ErrorMessage = "{0} 必須在 {1}和{2}之間.")]
           
            public Nullable<int> fsalesVolume { get; set; }

            [Required(ErrorMessage = "價格為必填")]
            [DisplayName("價格")]
            public Nullable<decimal> fprice { get; set; }

            [DisplayName("販售日")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public Nullable<System.DateTime> fsalesdate { get; set; }

            [DisplayName("可銷售庫存")]
            [Range(0, 200, ErrorMessage = "{0} 必須在 {1}和{2}之間.")]
            public Nullable<short> fsafetyStockLevel { get; set; }

            [DisplayName("修改日")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public Nullable<System.DateTime> fmodifiedDate { get; set; }

            [DisplayName("品牌")]
            public string fbrand { get; set; }

            [DisplayName("照片")]
            public string fphotoPath { get; set; }

            [DisplayName("內類別代號")]
            public virtual tcategory tcategory { get; set; }

            


        }
    }
}