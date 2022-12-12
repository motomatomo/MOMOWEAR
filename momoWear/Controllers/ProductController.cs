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
        public ActionResult List()
        {
           
            var list = db.tclothes.ToList();

            return View(list);
        }


        /// <summary>
        /// 新增空介面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var bag = db.tcategory;
            if (bag!= null)
            {
                // 新增物件到 Viewbag
                ViewBag.fcategoryNameResult= bag;
            }
            
            return View();
        }

        /// <summary>
        /// 新增 post
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
            }

            //if (!ModelState.IsValid)return View(c);
       
           
            if (c.photo!=null)
            {
                string name = Guid.NewGuid().ToString() + ".jpg";

                string photoPath = Path.Combine(Server.MapPath("~/images"), name);
                c.photo.SaveAs(photoPath);
                c.fphotoPath = name;
                db.tclothes.Add(c);
                db.SaveChanges();   
            }

            return RedirectToAction("List");




        }

        /// <summary>
        /// 新增類別
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateGategory() 
        {
            return View();
        }

       [HttpPost]
        public ActionResult CreateGategory(tcategory newCategory)
        {
            var bag = db.tcategory.Add(newCategory);
            db.SaveChanges();
            if (bag != null)
            {
                // 新增物件到 Viewbag
                ViewBag.fcategoryNameResult = bag;
            }

            return RedirectToAction("Create");
        }
    }
    
}