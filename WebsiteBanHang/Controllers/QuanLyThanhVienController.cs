using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTriWeb")]
    public class QuanLyThanhVienController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyThanhVien
        public ActionResult Index()
        {
            return View(db.ThanhViens.OrderBy(n => n.MaThanhVien));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.MaLoaiTV), "MaLoaiTV", "TenLoai");

            return View();
        }
        [HttpPost]
        public ActionResult TaoMoi(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.MaLoaiTV), "MaLoaiTV", "TenLoai");

            db.ThanhViens.Add(tv);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            ViewBag.CauHoi = new SelectList(LoadCauHoi(), selectedValue: tv.CauHoi);
            ViewBag.MaLoaiTV = TaoDanhSachLoaiTV(tv.MaLoaiTV.Value);
            return View(tv);
        }

        [HttpPost]
        public ActionResult ChinhSua(ThanhVien tv)
        {
            if (ModelState.IsValid)
            {
                ThanhVien thanhVien = db.ThanhViens.Find(tv.MaThanhVien);
                TryUpdateModel(thanhVien, new string[] { "MaSP", "TaiKhoan", "MatKhau", "HoTen", "DiaChi", "Email",
                                                        "SoDienThoai", "CauHoi", "CauTraLoi", "MaLoaiTV" });

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CauHoi = new SelectList(tv.CauHoi);
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens, "MaLoaiTV", "TenLoai", tv.MaLoaiTV);
            return View(tv);
        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            ViewBag.CauHoi = new SelectList(LoadCauHoi(), selectedValue: tv.CauHoi);
            ViewBag.MaLoaiTV = TaoDanhSachLoaiTV(tv.MaLoaiTV.Value);
            return View(tv);
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            db.ThanhViens.Remove(tv);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        private SelectList TaoDanhSachLoaiTV(int IDChon = 0)
        {
            var items = db.LoaiThanhViens.Select(p => new { p.MaLoaiTV, ThongTin = p.TenLoai }).ToList();
            var result = new SelectList(items, "MaLoaiTV", "ThongTin", selectedValue: IDChon);
            return result;
        }

        //Load câu hỏi để đưa vào dropdownlist
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();    //tạo list câu hỏi chứa câu hỏi
            lstCauHoi.Add("Họ tên người cha bạn là gì?");
            lstCauHoi.Add("Ca sĩ mà bạn yêu thích là ai?");
            lstCauHoi.Add("Vật nuôi mà bạn yêu thích là gì?");
            lstCauHoi.Add("Sở thích của bạn là gì");
            lstCauHoi.Add("Hiện tại bạn đang làm công việc gì?");
            lstCauHoi.Add("Trường cấp ba bạn học là gì?");
            lstCauHoi.Add("Năm sinh của mẹ bạn là gì?");
            lstCauHoi.Add("Bộ phim mà bạn yêu thích là gì?");
            lstCauHoi.Add("Bài nhạc mà bạn yêu thích là gì?");

            return lstCauHoi;
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