using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;    

namespace WebsiteBanHang.Controllers
{

    
    public class SanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        //tao 2 partial view san pham 1 va 2 de hien thi san pham theo 2 style khac nhau
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            

            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }

        //xây dựng trang xem chi tiết
        // GET: SanPham/XemChiTiet/1
        public ActionResult XemChiTiet(int? id,string tensp)
        {
            //check tham số truyền vào có null ko
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //nếu ko thì truy xuất csdl lấy ra sp với id tương ứng
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id&& n.DaXoa==false);   //trả về null nếu ko có id nào tương ứng 
            if(sp == null)
            {
                //thông báo nếu ko thấy sp này
                return HttpNotFound();
            }

            return View(sp);
        }
  
        //xây dựng action load sp theo mã loại sp và mã nsx
        public ActionResult SanPham(int? MaLoaiSP,int? MaNSX,int? page)
        {
            
            //check tham số truyền vào có null ko
            if (MaLoaiSP == null|| MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //load sp theo 2 tiêu chí là mã loại sp và mã nsx
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if(lstSP.Count() == 0)
            {
                //thông báo nếu ko thấy sp này
                return HttpNotFound();
            }
            //Phân trang
            if (Request.HttpMethod != "GET")
                page = 1;
            //tạo biến số sản phẩm trên trang
            int PageSize = 6;
            //tạo biến số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;


            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber,PageSize));
        }

        //xây dựng action load sp theo mã loại sp
        public ActionResult LoaiSanPham(int? MaLoaiSP, int? page)
        {
            //check tham số truyền vào có null ko
            if (MaLoaiSP == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //load sp theo 2 tiêu chí là mã loại sp và mã nsx
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP);
            if (lstSP.Count() == 0)
            {
                //thông báo nếu ko thấy sp này
                return HttpNotFound();
            }
            //Phân trang
            if (Request.HttpMethod != "GET")
                page = 1;
            //tạo biến số sản phẩm trên trang
            int PageSize = 6;
            //tạo biến số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;

            return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
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