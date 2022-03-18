using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanHang.Models
{
    //ViewModel hiển thị các thuộc tính cần được hiển thị lên view lấy từ model
    public class ItemGioHang
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }
        public ItemGioHang() { }
        //constructor theo id (dùng cho trường hợp chỉ có sl=1)
        public ItemGioHang(int iMaSP) 
        {
            using (QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;  //kiểu decimal dùng value để lấy gtri
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public ItemGioHang(int iMaSP,int sl)
        {
            using(QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = sl;
                this.ThanhTien = DonGia * SoLuong;
            }
        }


    }
}