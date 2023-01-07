using momoWear.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.Security;

namespace momoWear.Controllers
{   
    [Authorize(Users = "tingyang@gmail.com,motomato914@gmail.com")]
    public class ProductController : Controller
    {
        // GET: Product 
        MOMOWearEntities db = new MOMOWearEntities();
        /// <summary>
        /// List 所有列表
        /// </summary>
        /// <returns></returns>
        public ActionResult  List(string mpick, string txtKeyword,int page=1)
        {
            getCategoryName();
            getName();
            IEnumerable<tclothes> list = null;
            int currentPage;
            int pageSize = 20;
            //一定要設定這兩個值TempData["txtKeyword"] TempData["mpick"]
            //讓他帶回VIEW才能在下關鍵字時搜尋的到
            TempData["txtKeyword"] = txtKeyword;
            TempData["mpick"] = mpick;
            //GET分類名稱VALUE
            int fcategoryFid = Convert.ToInt32(Request.Form["fcategoryName"]);

            //如果input沒有輸入且衣服類別沒有下拉預設0
            // fcategoryFid有下拉 就會變成資料庫的fid 我設定在select的VALUE裡
            //透過Request.Form["fcategoryName"]取值 並轉INT32

            if (string.IsNullOrEmpty(txtKeyword) && fcategoryFid==0)
            {
                list = db.tclothes.OrderByDescending(m => m.fsalesVolume);
                currentPage = page < 1 ? 1 : page;
                return View(list.ToPagedList(currentPage, pageSize));
            }
            else
            {
                try
                {

                    if (!string.IsNullOrEmpty(txtKeyword))
                    {
                        switch (mpick)
                        {
                            case "商品名稱":
                                list = db.tclothes.Where
                                       (p => p.fname.Contains(txtKeyword) || p.fdescribe.Contains(txtKeyword));
                                list = list.OrderByDescending(p => p.fsalesVolume);
                                //pageSize = list.Count();
                                break;

                            case "顏色":
                                list = db.tclothes.Where(p => p.fcolor.Contains(txtKeyword)).OrderByDescending(p => p.fsalesVolume);
                                //pageSize = list.Count();
                                break;

                            case "選擇搜關鍵字分類":
                            default:
                                //list = db.tclothes.OrderByDescending(m => m.fsalesVolume);
                                TempData["reSearh"] = "請下拉選擇搜尋方式";
                                throw new Exception();
                        }
                        //雖然list就算沒找到東西 也不會進到NULL的因為他不是NULL是Empty
                        //所以要用Count判斷到底有沒有找到 如果為0就是沒找到
                        if (!list.Any())
                        {
                            TempData["noFound"] = "未搜尋到任何產品";
                            //TempData["noFound"] = "未搜尋到任何產品";
                        }

                        //currentPage = page < 1 ? 1 : page;
                        //return View(list.ToPagedList(currentPage, pageSize));
                        return View(list.ToPagedList(page, pageSize));
                    }


                    //如果搜尋input沒輸入
                    else
                    {
                        switch (mpick)
                        {

                            case "庫存不足商品":
                                list = db.tclothes.Where(p => p.fquentity >= 0 && p.fquentity < 5).OrderByDescending(p => p.fquentity);
                                pageSize = list.Count();
                                break;

                            case "商品類別":
                                list = db.tclothes.Where(p => p.tcategory.fid == fcategoryFid);
                                list = list.OrderByDescending(p => p.fsalesVolume);
                                pageSize = list.Count();
                                break;

                            case "顏色":
                            case "商品名稱":
                                TempData["reSearh"] = "請輸入商品名稱或衣服顏色";

                                throw new Exception();

                            case "選擇搜關鍵字分類":
                            default:
                                //list = db.tclothes.OrderByDescending(m => m.fsalesVolume);
                                TempData["reSearh"] = "請下拉選擇搜尋方式";
                                throw new Exception();
                        }

                        //雖然list就算沒找到東西 也不會進到NULL的因為他不是NULL是Empty 
                        //所以要用Count判斷到底有沒有找到 如果為0就是沒找到 Count效能比較差 改用Any
                        if (!list.Any())
                        {
                            //TempData["noFound"] = "$('#dialog').modal('show');";
                            TempData["noFound"] = "未搜尋到任何產品";

                        }
                        //currentPage = (int)page < 1 ? 1 : (int)page;
                        //return View(list.ToPagedList(currentPage, pageSize));
                        return View(list.ToPagedList(page, pageSize));
                    }

                }
                catch (Exception)
                {
                    //TempData["err"]= e.Message;
                    return RedirectToAction("List");
                }
  
            }

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
        
        private void getCategoryNameByID(int id=1)
        {
            var tempData = db.tcategory.Where(m=>m.fid==id).FirstOrDefault().fcategoryName;
            if (tempData != null)
            {
                TempData["Defaultfcategory"] = tempData;
            }
        }

        /// <summary>
        /// 新增產品 GET
        /// </summary>
        /// <returns></returns>

        public ActionResult Create()
        {
           
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
                //檔案驗證
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
                    string tempfileName = "";
                    //偵測是否有同樣檔名的檔案
                    if (System.IO.File.Exists(photoPath)) {
                        int myCounter = 2;
                        
                        //如果有就把相同檔名的檔案改掉,因為可能不只一個檔名相同,所以用while相同都要改,然後數字會遞增去改
                        while (System.IO.File.Exists(photoPath))
                        {
                            tempfileName = myCounter.ToString() + "-" + name;
                            photoPath = Path.Combine(Server.MapPath("~/images"),tempfileName);
                            myCounter += 1;
                        }
                        //把修改好的檔名丟給name
                        name = tempfileName;
                    }
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
            if (ModelState.IsValid)
            {
                newCategory.fcategoryName.Trim();
                var bag = db.tcategory.Add(newCategory);
                db.SaveChanges();
                if (bag != null)
                {
                    // 新增物件到 Viewbag 這樣新增類別後才會帶到Create.cshtml頁面顯示
                    TempData["fcategoryNameResult"] = bag;
                }
                getName();
            }
            return RedirectToAction("Create");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        ///  [HttpPost] 修改
        /// </summary>
        /// <param name="editedProduct"></param>
        /// <returns></returns>
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
                            string name = Path.GetFileName(editedProduct.photo.FileName);
                            string photoPath = Path.Combine(Server.MapPath("~/images"), name);

                            string tempfileName = "";
                            //偵測是否有同樣檔名的檔案
                            
                            {
                                int myCounter = 2;

                                //如果有就把相同檔名的檔案改掉,因為可能不只一個檔名相同,所以用while相同都要改,然後數字會遞增去改
                                while (System.IO.File.Exists(photoPath))
                                {
                                    tempfileName = myCounter.ToString() + "-" + name;
                                    photoPath = Path.Combine(Server.MapPath("~/images"), tempfileName);
                                    myCounter += 1;
                                }
                                //把修改好的檔名丟給name
                                name = tempfileName;
                            }



                            //存照片
                            editedProduct.photo.SaveAs(photoPath);
                            //選出來的產品的圖片路徑改成新的圖檔的檔名
                            product.fphotoPath = name;
                        }  
                    }
                    //沒更新照片的話使用原照片
                    else
                    {
                        //要等於原來SElect的照片  不能等於 NULL editedProduct.fphotoPath
                        product.fphotoPath = product.fphotoPath;
                    }

                    product.fserialNumber = editedProduct.fserialNumber;
                    product.fcategoryID = editedProduct.fcategoryID;
                    product.fname = editedProduct.fname;
                    product.fsize = editedProduct.fsize;
                    product.fcolor = editedProduct.fcolor;
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

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 列出所有依類別排序品項
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult queryBYCategory()
        {
            //getCategoryNameByID(id);
            getCategoryName();
            getName();
            var c = db.tcategory.ToList();
            var query = (from o in db.tcategory
                         join p in db.tclothes
                         on o.fid equals p.fcategoryID
                         orderby p.fcategoryID ascending
                         select new CqueryBYCategoryVM
                         {
                             
                             fid = p.fid,
                             fcategoryID = o.fid,
                             fcategoryName = o.fcategoryName,
                             fname = p.fname,
                             fquentity = p.fquentity,
                             fsalesVolume = p.fsalesVolume,
                             fdescribe = p.fdescribe,
                             fphotoPath=p.fphotoPath
                         }).ToList();
            
            return View(query);
        }

    }

}