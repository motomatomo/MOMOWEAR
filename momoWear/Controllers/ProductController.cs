using momoWear.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace momoWear.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        MOMOWearEntities db = new MOMOWearEntities();
        /// <summary>
        /// List 所有列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var list = from p in db.tclothes
                       select p;
            //var list = db.tclothes.ToList();

            return View(list);
        }


        /// <summary>
        /// 新增產品 GET
        /// </summary>
        /// <returns></returns>
       
        public ActionResult Create()
        {
            //我想要把兩個欄位都放在TEXT裡失敗 改天再試
            //var bag = db.tcategory;
            //int categorycount = bag.Count();
            //var category = db.tcategory.FirstOrDefault(c => c.fid <= categorycount);

            //string fcategoryID = category.fcategoryID;
            //string fcategoryName = category.fcategoryName;

            //if (bag!= null)
            //{
            //    //下拉是選單 精簡 1.db.t表單,2.value 3.text
            //    //ViewBag.fcategoryName = new SelectList(db.tcategory, "fcategoryName", $"{fcategoryID} {fcategoryName}");
            //}
            var bag = db.tcategory;
            if (bag != null)
            {
                //原來新增物件到 Viewbag
                 ViewBag.fcategoryNameResult= bag;

                //下拉是選單 精簡 1.db.t表單,2.value 3.text
                //成功顯示 fcategoryName 但我想要下拉選單text有兩個 fcategoryID 和 fcategoryName
                //ViewBag.fcategoryName = new SelectList(db.tcategory, "fid", "fcategoryName");

            }

            return View();
        }

        /// <summary>
        /// 新增產品 post
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(tclothes c)
        {
            //HttpPostedFileBase photo= Request.Files["photo"];
            var bag = db.tcategory;
            if (bag != null)
            {
                // 新增物件到 Viewbag
                ViewBag.fcategoryNameResult = bag;
                //下拉是選單 精簡 1.db.t表單,2.value 3.text
                //ViewBag.fcategoryName = new SelectList(db.tcategory, "fid","fcategoryName");
            }

            if (!ModelState.IsValid) {
                string message = string.Empty;
                foreach (var err in ModelState.Values)
                {
                    if (err.Errors.Count>0)
                    {
                        foreach (var e in err.Errors)
                        {
                            message += e.ErrorMessage;
                        }
                    }
                }
                ViewBag.err = message;
                return View(c);

            } 
            
            if (c.photo != null)
            {
                string name = Guid.NewGuid().ToString() + ".jpg";

                string photoPath = Path.Combine(Server.MapPath("~/images"), name);
                c.photo.SaveAs(photoPath);
                c.fphotoPath = name;
                db.tclothes.Add(c);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                db.tclothes.Add(c);
                db.SaveChanges();

                return RedirectToAction("List");
            }







        }

        /// <summary>
        /// 新增類別Get
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateGategory() 
        {
            return View();
        }
        /// <summary>
        /// 新增類別Post 新增完畢後導向RedirectToAction("Create")
        /// </summary>
        /// <param name="newCategory"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateGategory(tcategory newCategory)
        {
            var bag = db.tcategory.Add(newCategory);
            db.SaveChanges();
            if (bag != null)
            {
                // 新增物件到 Viewbag 這樣新增類別後才會帶到Create.cshtml頁面顯示
                ViewBag.fcategoryNameResult = bag;
            }

            return RedirectToAction("Create");
        }
    }
    
}