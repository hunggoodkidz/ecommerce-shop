using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTriWeb")]
    public class PhanQuyenController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: PhanQuyen
        public ActionResult Index()
        {
            return View(db.LoaiThanhViens.OrderBy(n => n.TenLoai));
        }

        [HttpGet]
        public ActionResult PhanQuyen(int? id)
        {
            //lấy mã loại tv dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null)
                return HttpNotFound();
            ViewBag.MaQuyen = db.Quyens; //lấy ds quyền
            ViewBag.LoaiTVQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == id);  //lấy ds loại của tv đó
            return View(ltv);
        }
        [HttpPost]
        public ActionResult PhanQuyen(int? MaLTV, IEnumerable<LoaiThanhVien_Quyen> lstPhanQuyen)
        {
            //trường hợp: nếu đã tiến hành phân quyền rồi nhưng muốn phân quyền lại
            //b1: xóa những quyền cũ thuộc loại tv đó
            var lstDaPhanQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == MaLTV);
            if (lstDaPhanQuyen.Count() != 0)
            {
                db.LoaiThanhVien_Quyen.RemoveRange(lstDaPhanQuyen);
                db.SaveChanges();
            }
            if(lstDaPhanQuyen != null)
            {
                foreach (var item in lstPhanQuyen)
                {
                    item.MaLoaiTV = int.Parse(MaLTV.ToString());
                    db.LoaiThanhVien_Quyen.Add(item);   //nếu đc check thì add data vào bảng phanquyen
                }
                db.SaveChanges();
            }
            //ktra list ds quyền dc check
            

            return RedirectToAction("Index");
        }
    }
}