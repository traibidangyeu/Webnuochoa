using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNuocHoa.Models
{
    public class Giohang
    {
        dbQLNHDataContext data = new dbQLNHDataContext();
        public int iMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sAnhBia { set; get; }
        public double dDonggia { set; get; }
        public int iSoluong { set; get; }
        public double dThanhtoan { get { return iSoluong * dDonggia; } }
        public Giohang(int MaSP)
        {
            iMaSP = MaSP;
            SanPham san = data.SanPhams.Single(n => n.MaSP == iMaSP);
            sTenSP = san.TenSP;
            sAnhBia = san.AnhBia;
            dDonggia = double.Parse(san.GiaBan.ToString());
            iSoluong = 1;
        }
    }
}