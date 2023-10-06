using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace WebNuocHoa.Models
{
    public class project
    {
        public static List<SanPham> GetSanPhams()
        {
            List<SanPham> l = new List<SanPham>();
            dbQLNHDataContext cn = new dbQLNHDataContext();
            l = cn.GetTable<SanPham>().ToList<SanPham>();
            return l;
        }
        public static List<SanPham> GetSanPhamsByMaLoai(int MaLoai)
        {
            List<SanPham> l = new List<SanPham>();
            dbQLNHDataContext cn = new dbQLNHDataContext();
            l = cn.GetTable<SanPham>().Where(x => x.MaLoai == MaLoai).ToList<SanPham>();
            return l;           
        }

    }
}