using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace momoWear.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Index(string loginAccount,string loginPassword)
        {
            // 取得登入者姓名
            string[] accountAry = new string[] { "tingyang@gmail.com", "motomato914@gmail.com" };
            string[] pwdAry = new string[] { "0985555026ttt", "0958211914ttt" };

            int index = -1;
            for (int i= 0;  i< accountAry.Length; i++)
            {
                if (accountAry[i]== loginAccount && pwdAry[i]==loginPassword)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                ViewBag.Err = "帳號或密碼錯誤";
            }
            else {
                // 登入using System.Web.Security; 表單驗證服務 授權指定的帳號
                FormsAuthentication.RedirectFromLoginPage(loginAccount, false);
                return RedirectToAction("List", "Product");
            }

           
            return View();

        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }






        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}