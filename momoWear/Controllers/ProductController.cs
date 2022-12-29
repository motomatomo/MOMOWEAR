using momoWear.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;


namespace momoWear.Controllers
{   
    [Authorize(Users = "tingyang@gmail.com,motomato914@gmail.com")]
    public class ProductController : Controller
    {
        // GET: Product
        MOMOWearEntities db = new MOMOWearEntities();

        [HttpGet]
        public ActionResult List(int page = 1) 
        {

            getCategoryName();
            getName();
            IEnumerable<tclothes> list = db.tclothes.OrderByDescending(m=>m.fquentity);
            int pageSize = 10;

            int currentPage = page < 1 ? 1 : page;
            return View(list.ToPagedList(currentPage, pageSize));
        }

        /// <summary>
        /// List 所有列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string keyword,int? page)
        {

            IEnumerable<tclothes> list = null;
            //IEnumerable<tclothes> list;
            //input
            keyword = Request.Form["txtKeyword"];
            //下拉選單關鍵字
            string str = Request.Form["mpick"];
            //分類名稱
            string fcategoryName = Request.Form["fcategoryName"];
            int pageSize = 10;
            

            try
            {
                getCategoryName();
                getName();


                if (!string.IsNullOrEmpty(keyword))
                {
                    switch (str)
                    {
                        case "商品名稱":
                            list = db.tclothes.Where
                                   (p => p.fname.Contains(keyword) || p.fdescribe.Contains(keyword));
                            list = list.OrderByDescending(p => p.fquentity);

                            break;

                        case "顏色":
                            list = db.tclothes.Where(p => p.fcolor.Contains(keyword)).OrderByDescending(p => p.fquentity);

                            break;



                        case "選擇搜關鍵字分類":
                        default:
                            list = from p
                                   in db.tclothes
                                   orderby p.fquentity descending
                                   select p;
                            break;




                    }
                    //雖然list就算沒找到東西 也不會進到NULL的因為他不是NULL是Empty
                    //所以要用Count判斷到底有沒有找到 如果為0就是沒找到

                    if (!list.Any())
                    {
                        TempData["noFound"] = "未搜尋到任何產品";
                        //TempData["noFound"] = "未搜尋到任何產品";

                    }

                    return View(list.ToPagedList(page ?? 1, pageSize));

                }


                //如果搜尋input沒輸入
                else
                {
                    switch (str)
                    {

                        case "庫存不足商品":
                            list = db.tclothes.Where(p => p.fquentity >= 0 && p.fquentity < 5).OrderByDescending(p => p.fquentity);
                            break;

                        case "商品類別":
                            list = db.tclothes.Where(p => p.tcategory.fcategoryName.Contains(fcategoryName));
                            list = list.OrderByDescending(p => p.fquentity);
                            break;

                        case "選擇搜關鍵字分類":
                        default:
                            list = from p
                                   in db.tclothes
                                   orderby p.fquentity descending
                                   select p;
                            break;
                    }
                }

                //if (list == null)
                //    return View("Error", new { message = "發生未知錯誤" });

            }
            catch (Exception e)
            {
                TempData["err"]= e.Message;
            }

           
            //雖然list就算沒找到東西 也不會進到NULL的因為他不是NULL是Empty 
            //所以要用Count判斷到底有沒有找到 如果為0就是沒找到 Count效能比較差 改用Any
            if (!list.Any())
            {
                //TempData["noFound"] = "$('#dialog').modal('show');";
                TempData["noFound"] = "未搜尋到任何產品";

            }
            return View(list.ToPagedList(page ?? 1, pageSize));
        }

        private void getCategoryName()
        {
            var tempData = db.tcategory.ToList();
            if (tempData != null)
            {
                TempData["fcategoryNameResult"] = tempData;
            }
        }

        private void getName()
        {
            //若驗證通過 夾帶使用者名稱
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.userName = User.Identity.Name;
            }
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
            var tempData = db.tcategory.ToList();
            if (tempData != null)
            {
                //新增類別到 TempData["fcategoryNameResult"]
                //新增失敗要轉向回Create get方法時 只在get ViewBag下拉是選單會報錯 因為VIEWBAG不見了
                //[HttpPost]方法也要 變成getPost兩個地方都要寫ViewBag 帶分類下拉回去 我覺得比較不聰明
                //tempData型別是List<tcategory>
                //ViewBag.fcategoryNameResult= tempData;
                TempData["fcategoryNameResult"] = tempData;

                //下拉是選單 精簡 1.db.t表單,2.value 3.text
                //成功顯示 fcategoryName 但我想要下拉選單text有兩個 fcategoryID 和 fcategoryName
                //ViewBag.fcategoryName = new SelectList(db.tcategory, "fid", "fcategoryName");

            }
            getName();
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
            
            //依有沒有上傳照片分類
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
                    //string name = Guid.NewGuid().ToString() + ".jpg";
                    string name = Path.GetFileName(c.photo.FileName);

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
            getName();
            return View();
        }
        /// <summary>
        /// 新增類別Post 新增完畢後導向RedirectToAction("Create")
        /// </summary>
        /// <param name="newCategory">tcategory newCategory</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateGategory(tcategory newCategory)
        {
            if (!ModelState.IsValid)
            {
                var bag = db.tcategory.Add(newCategory);
                db.SaveChanges();
                if (bag != null)
                {
                    // 新增物件到 Viewbag 這樣新增類別後才會帶到Create.cshtml頁面顯示
                    ViewBag.fcategoryNameResult = bag;
                }
                getName();
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
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return RedirectToAction("List");
        
        }

        [HttpPost]
        public ActionResult Edit(tclothes editedProduct)
        {
            if (ModelState.IsValid)
            {
                tclothes product = db.tclothes.FirstOrDefault(c => c.fid == (int)editedProduct.fid);
                //依有沒有上傳照片分類
                if (product != null)
                {

                    if (editedProduct.photo != null)
                    {
                        bool fileValid = true;
                        // Limit File Szie Below : 10MB
                        if (editedProduct.photo.ContentLength <= 0 || editedProduct.photo.ContentLength > 10475760)
                        {
                            fileValid = false;
                            ViewBag.ContentLength = "檔案上傳請低於: 10MB";
                        }
                        if (fileValid == true)
                        {
                            string photoName = Path.GetFileName(editedProduct.photo.FileName);
                            string photoSavePath = Path.Combine(Server.MapPath("~/images"), photoName);
                            //存照片
                            editedProduct.photo.SaveAs(photoSavePath);
                            //選出來的產品的圖片路徑改成新的圖檔的檔名
                            product.fphotoPath = photoName;
                        }  
                    }
                    
                    else
                    {
                        //要等於原來SElect的照片  不能等於 NULL editedProduct.fphotoPath
                        product.fphotoPath = product.fphotoPath;
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

        [HttpPost]
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">錯誤訊息</param>
        
        public ActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }



    }

}