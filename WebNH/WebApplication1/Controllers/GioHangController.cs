using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNuocHoa.Models;
namespace WebNuocHoa.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        dbQLNHDataContext data = new dbQLNHDataContext();
        // GET: GioHang
        public List<Giohang> LayGiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int iMaSP, string strURL)
        {
            List<Giohang> lstGiohang = LayGiohang();
            Giohang sanpham = lstGiohang.Find(n => n.iMaSP == iMaSP);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaSP);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        private double Tongtien()
        {
            double iTongtien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongtien = lstGiohang.Sum(n => n.dThanhtoan);
            }
            return iTongtien;
        }
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = LayGiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = Tongtien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = Tongtien();
            return PartialView();
        }
        //Xoa gio hang
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = LayGiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            if (lstGiohang != null)
            {
                lstGiohang.RemoveAll(n => n.iMaSP == iMaSP);
                return RedirectToAction("Giohang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = LayGiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            //Kiem tra viec dang nhap
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap","Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lay gio hang
            List<Giohang> lstGiohang = LayGiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = Tongtien();
            return View(lstGiohang);
        }

        public ActionResult Dathang(FormCollection collection)
        {
            //Them đơn hàng
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<Giohang> gh = LayGiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyy}", collection["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Thêm chi tiết đơn hàng
            foreach(var item in gh)
            {
                ChiTietDatHang ctdh = new ChiTietDatHang();
                ctdh.SoDH = ddh.SoDH;
                ctdh.MaSP = item.iMaSP;
                ctdh.SoLuong = item.iSoluong;
                ctdh.DonGia = (decimal)item.dDonggia;
                data.ChiTietDatHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }

}