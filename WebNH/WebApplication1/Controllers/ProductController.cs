using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNuocHoa.Models;
using PagedList;
using PagedList.Mvc;

namespace WebNuochoa.Controllers
{
    public class ProductController : Controller
    {
        dbQLNHDataContext data = new dbQLNHDataContext();
        private List<SanPham> sanphammoi(int count)
        {
            return data.SanPhams.OrderByDescending(a => a.MaLoai).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 9;
            int pageNum = (page ?? 1);
            var nuochoamoi = sanphammoi(40);
            return View(nuochoamoi.ToPagedList(pageNum,pageSize));
        }

    }
}