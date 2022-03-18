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
    public class QuanLyNhaSXController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyNhaSX
        public ActionResult Index()
        {
            return View(db.NhaSanXuats.OrderBy(n=>n.MaNSX));
        }

        [HttpGet]
        public ActionResult TaoMoi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TaoMoi(NhaSanXuat nsx)
        {
            db.NhaSanXuats.Add(nsx);
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
            NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);
            if (nsx == null)
            {
                return HttpNotFound();
            }

            return View(nsx);
        }
        [HttpPost]
        public ActionResult ChinhSua(NhaSanXuat nsx)
        {
            //if(ModelState.IsValid)
            db.Entry(nsx).State = EntityState.Modified;
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
            NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);
            if (nsx == null)
            {
                return HttpNotFound();
            }

            return View(nsx);
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);
            if (nsx == null)
            {
                return HttpNotFound();
            }
            db.NhaSanXuats.Remove(nsx);
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