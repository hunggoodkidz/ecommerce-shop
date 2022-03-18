using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = "QuanLy,QuanTriWeb")]
    public class QuanLyPhieuNhapController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: QuanLyPhieuNhap
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            ViewBag.NgayNhap = DateTime.Today;

            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> lstModel)
        {

            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            ViewBag.NgayNhap = DateTime.Today;

            model.NgayNhap = ViewBag.NgayNhap;
            model.DaXoa = false;
            //Sau khi đã ktra hết dl đầu vào

            db.PhieuNhaps.Add(model);
            db.SaveChanges();   //save để lấy MaPN gán cho lst chitietpn
            SanPham sp;
            foreach (var item in lstModel)
            {
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;  //update solg tồn

                item.MaPN = model.MaPN; //gán MaPN cho all chitietpn
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();

            return View();
        }

        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            //ds sp gần hết hàng với số lượng tồn bé hơn hoặc bằng 5
            var lstSP = db.SanPhams.Where(n => n.DaXoa == false && n.SoLuongTon <= 5);
            return View(lstSP);
        }

        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");

            if(id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
                return HttpNotFound();
            return View(sp);
        }

        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, ChiTietPhieuNhap ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);

            model.NgayNhap = DateTime.Now;
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();   //save để lấy MaPN gán cho lst chitietpn

            ctpn.MaPN = model.MaPN;
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            db.ChiTietPhieuNhaps.Add(ctpn);
            db.SaveChanges();

            return View(sp);
        }

        //Giải phóng dung lượng biến db, để ở cuối controller
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
