using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNuocHoa.Models;
namespace WebNuocHoa.Controllers
{
    public class SingleProductController : Controller
    {
        // GET: SingleProduct
        public ActionResult Index(int id)
        {
            dbQLNHDataContext db = new dbQLNHDataContext();
            SanPham m = db.SanPhams.Where(sp => sp.MaSP.Equals(id)).First<SanPham>();
            //--Đưa sản phẩm vào index
            ViewData["Details"] = m;
            return View();
        }
    }
}