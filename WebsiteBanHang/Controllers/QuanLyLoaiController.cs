using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = "QuanLy,QuanTriWeb")]
    public class QuanLyLoaiController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyLoai
        public ActionResult Index()
        {
            return View(db.LoaiSanPhams.OrderBy(n=>n.MaLoaiSP));
        }

        [HttpGet]
        public ActionResult TaoMoi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TaoMoi(LoaiSanPham loai)
        {
            db.LoaiSanPhams.Add(loai);
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
            LoaiSanPham loaisp = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (loaisp == null)
            {
                return HttpNotFound();
            }
          
            return View(loaisp);
        }
        [HttpPost]
        public ActionResult ChinhSua(LoaiSanPham loai)
        {
            //if(ModelState.IsValid)
            db.Entry(loai).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
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
            LoaiSanPham loaisp = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (loaisp == null)
            {
                return HttpNotFound();
            }
            
            return View(loaisp);
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaisp = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (loaisp == null)
            {
                return HttpNotFound();
            }
            db.LoaiSanPhams.Remove(loaisp);
            db.SaveChanges();

            return RedirectToAction("Index");
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