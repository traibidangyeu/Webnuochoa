using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNuocHoa.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace WebNuocHoa.Controllers
{
    public class TrangQTController : Controller
    {
        dbQLNHDataContext db = new dbQLNHDataContext();
        //GET:Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SanPham(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10 ;
            return View(db.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber,pageSize));
        }
        public ActionResult login() 
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải Nhập Tên Đăng Nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải Nhập Mật Khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoan"] = ad;
                    return RedirectToAction("SanPham", "TrangQT");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng ";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSP()
        {
            ViewBag.MaLoai = new SelectList(db.LoaiSPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSP(SanPham sp , HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(db.LoaiSPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if (fileupload==null)
            {
                ViewBag.Thongbao = "Chọn hình ảnh vào";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu tên file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    // lưu đường dẫn
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.AnhBia = fileName;
                    // Lưu file
                    db.SanPhams.InsertOnSubmit(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("SanPham");

            }
        }
        [HttpGet]
        public ActionResult Delete (int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xacnhanxoa(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SanPhams.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoai = new SelectList(db.LoaiSPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sp.MaLoai);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SanPham sp, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(db.LoaiSPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Chọn hình ảnh vào";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu tên file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    // lưu đường dẫn
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.AnhBia = fileName;
                    // Lưu file
                    UpdateModel(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("SanPham");

            }
        }
    }
}