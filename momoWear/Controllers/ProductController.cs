using momoWear.Models;
using System;
using System.Collections.Generic;
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
            ViewBag.fcategoryNameResult = db.tcategory.Select(m=>m.fcategoryName);
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
            if (ModelState.IsValid)
            {
                db.tclothes.Add(c);
                db.SaveChanges();
                return RedirectToAction("List");
            }
           
            return View(c);
        }
    }
    
}