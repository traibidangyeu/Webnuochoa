using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNuocHoa.Models;
namespace WebNuocHoa.Controllers
{
    public class NguoidungController : Controller
    {
        dbQLNHDataContext db = new dbQLNHDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KhachHang kh)
        {
            //gan gia tri vao form
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var nhaplaimatkhau = collection["Matkhaunhaplai"];
            var email = collection["Email"];
            var diachi = collection["DiachiKH"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = " Họ Tên Khách Hàng Không Được Để Trống";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = " Tài Khoản Không Được Để Trống";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = " Mật Khẩu Không Được Để Trống";
            }
            if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi4"] = " Mật Khẩu Nhập Lại Không Được Để Trống";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = " Email không được để trống";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = " Địa chỉ không được để trống";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = " Điện thoại Không Được Để Trống";
            }
            else
            {
                //Gan gia vao Data
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienThoaiKH = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");

            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải Nhập Mật Khẩu";
            }
            else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Baoloi = "Chúc mừng đăng nhập thành công ";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Baoloi = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}