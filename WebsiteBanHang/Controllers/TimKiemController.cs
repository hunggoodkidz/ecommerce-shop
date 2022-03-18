using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;

namespace WebsiteBanHang.Controllers
{
    public class TimKiemController : Controller
    {

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            //Phân trang
            if (Request.HttpMethod != "GET")
                page = 1;
            //tạo biến số sản phẩm trên trang
            int PageSize = 6;
            //tạo biến số trang hiện tại
            int PageNumber = (page ?? 1);
            //tìm kiếm theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;

            return View(lstSP.OrderBy(n=>n.TenSP).ToPagedList(PageNumber,PageSize));
        }
        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {
            //gọi về hàm get tìm kiếm

            return RedirectToAction("KQTimKiem", new {@sTuKhoa = sTuKhoa });
        }
    }
}