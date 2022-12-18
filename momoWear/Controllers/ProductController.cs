﻿using momoWear.Models;
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
        public ActionResult Create([Bind(Exclude = "fid")] tclothes c)
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

                var fileValid = true;
                // Limit File Szie Below : 10MB
                if (c.photo.ContentLength <= 0 || c.photo.ContentLength > 10475760)
                {
                    fileValid = false;
                    ViewBag.ContentLength = "Limit File Szie Below : 10MB";
                }
                if (fileValid == true)
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
            else
            {
               c.fphotoPath = "no-picture.jpg";
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


        public ActionResult Edit(int? id) 
        {
            if (id!=null)
            {
                //tclothes product = db.tclothes.FirstOrDefault(c => c.fid == (int)id);
                tclothes product = db.tclothes.Where(c => c.fid == (int)id).FirstOrDefault();
                var bag = db.tcategory;
                if (bag != null)
                {
                    //原來新增物件到 Viewbag
                    ViewBag.fcategoryNameResult = bag;
                    return View(product);
                }
            }
            return RedirectToAction("List");
        
        }

        [HttpPost]
        public ActionResult Edit(tclothes editedProduct)
        {
            if (ModelState.IsValid)
            {
                tclothes product = db.tclothes.FirstOrDefault(c => c.fid == (int)editedProduct.fid);

                if (product != null)
                {

                    if (editedProduct.photo != null)
                    {
                        var fileValid = true;
                        // Limit File Szie Below : 10MB
                        if (editedProduct.photo.ContentLength <= 0 || editedProduct.photo.ContentLength > 10475760)
                        {
                            fileValid = false;
                            ViewBag.ContentLength = "檔案上傳請低於: 10MB";
                        }
                        if (fileValid == true)
                        {
                            string photoName = Guid.NewGuid().ToString()+".jpg";
                            string photoSavePath = Path.Combine(Server.MapPath("~/images"), photoName);
                            //存照片
                            editedProduct.photo.SaveAs(photoSavePath);
                            //選出來的產品的圖片路徑改成新的圖檔的檔名
                            product.fphotoPath = photoName;
                        }  
                    }
                    else
                    {
                        product.fphotoPath = editedProduct.fphotoPath;
                    }

                    product.fserialNumber = editedProduct.fserialNumber;
                    product.fcategoryID = editedProduct.fcategoryID;
                    product.fname = editedProduct.fname;
                    product.fsize = editedProduct.fsize;
                    product.fquentity = editedProduct.fquentity;
                    product.fdescribe = editedProduct.fdescribe;
                    product.fsalesVolume = editedProduct.fsalesVolume;
                    product.fprice = editedProduct.fprice;
                    product.fsalesdate =editedProduct.fsalesdate;
                    product.fsafetyStockLevel = editedProduct.fsafetyStockLevel;
                    product.fmodifiedDate = editedProduct.fmodifiedDate;

                    db.SaveChanges();

                }
                return RedirectToAction("List");
            }
            return View(editedProduct);

        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                tclothes prod = db.tclothes.FirstOrDefault(p => p.fid == (int)id);
                if (prod != null)
                {
                    db.tclothes.Remove(prod);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("List");
        }




    }
    
}