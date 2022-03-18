using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [AllowAnonymous]
    public class ThongKeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: ThongKe
        public ActionResult Index()
        {
            DateTime hienTai = DateTime.Today;
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();   //lấy slg ng truy cập từ application đã tạo
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();   //lấy slg ng online từ application đã tạo
            ViewBag.TongDoanhThu = ThongKeDoanhThu();
            ViewBag.SoDonHang = ThongKeDonHang();
            ViewBag.SoThanhVien = ThongKeThanhVien();
            ViewBag.DoanhThuThang = ThongKeDoanhThuThang(hienTai.Month, hienTai.Year);

            return View();
        }

        [HttpGet]
        public ActionResult DangkyAdmin()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());  //gắn các câu hỏi vào viewbag để hiển thị lên view

            return View();
        }

        [HttpPost]
        public ActionResult DangkyAdmin(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());

            if (ModelState.IsValid)
            {
                tv.MaLoaiTV = 2;
                //Thêm khách hàng vào csdl
                db.ThanhViens.Add(tv);  //sau khi lấy được các thuộc tính trong biến tv qua các textbox thì truyền tv vào dbset ThanhViens
                                        //Lưu thay đổi
                db.SaveChanges();   //lấy data từ dbset chuyển vào csdl
            }
            return View();
        }

        [HttpPost]
        public ActionResult DangNhapAdmin(FormCollection f)
        {
            string taikhoan = f["txtTenDangNhap"].ToString();   //lấy chuỗi trong txtTenDangNhap
            string matKhau = f["txtMatKhau"].ToString();    //lấy chuỗi trong txtMatKhau

            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matKhau);      //so sánh với tk và mk trong csdl
            if (tv != null)
            {
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);   //lấy ra list quyền tương ứng loaitv
                string Quyen = "";
                if (lstQuyen.Count() != 0)
                {
                    foreach (var item in lstQuyen)   //duyệt list quyền
                    {
                        Quyen += item.Quyen.MaQuyen + ",";
                    }
                    Quyen = Quyen.Substring(0, Quyen.Length - 1); //Cắt dấu ,
                    PhanQuyen(tv.TaiKhoan.ToString(), Quyen);

                    Session["TaiKhoan"] = tv;
                    return RedirectToAction("Index");   //đoạn script dùng để reload lại trang khi đăng nhập thành công
                }
            }
            return Content("Tài khoản hoặc mật khẩu không chính xác.");
        }

        public ActionResult DangXuatAdmin()
        {
            Session["TaiKhoan"] = null; //thiết lập session là null

            FormsAuthentication.SignOut();  //xóa bộ nhớ cookie

            return RedirectToAction("Index","ThongKe");
        }

        public ActionResult LoiPhanquyen()
        {
            return View();
        }

        public void PhanQuyen(string Taikhoan, string Quyen)
        {
            FormsAuthentication.Initialize();

            var ticket = new FormsAuthenticationTicket(1,
                                            Taikhoan,   //đặt tên ticket theo tên tk 
                                            DateTime.Now,   //lấy tgian bắt đầu
                                            DateTime.Now.AddHours(3),   //thời gian 3 tiếng out ra
                                            false,  //ko lưu
                                            Quyen,  //Lấy chuỗi phân quyền
                                            FormsAuthentication.FormsCookiePath);   //Lấy đg dẫn cookie thay vì name
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));  //tạo cookie(tự tạo name, mã hóa thông tin ticket add vào cookie)
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;    //ktra cookie có chưa
            Response.Cookies.Add(cookie);     //
        }


        //thống kê doanh thu từ khi web thành lập
        public decimal ThongKeDoanhThu()
        {
            decimal TongDoanhThu = db.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.Dongia).Value;
            return TongDoanhThu;
        }
        public decimal ThongKeDoanhThuThang(int Thang,int Nam)
        {
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang && n.NgayDat.Value.Year == Nam);  //lấy ds đơn hàng có date tương ứng
            decimal TongDoanhThu = 0;
            foreach(var item in lstDDH) //duyệt chi tiết từng đơn và tính tổng tiền
            {
                TongDoanhThu += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.Dongia).Value.ToString());
            }
            return TongDoanhThu;
        }
        
        //Thống kê tổng đơn hàng
        public double ThongKeDonHang()
        {
            double slddh = db.DonDatHangs.Count();    //đếm số đơn hàng
            return slddh;
        }
        public double ThongKeThanhVien()
        {
            double sltv = db.ThanhViens.Count();    //đếm số thành viên
            return sltv;
        }

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
            if(disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}