using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using System.Web.Security;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;

namespace WebsiteBanHang.Controllers
{

    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        
        // GET: Home/Index
        public ActionResult Index()
        {
            //Lần lượt tạo các viewbag để lấy list sp từ csdl
            //List laptop mới
            var lstLTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false).ToList();
            //Gán vào viewbag
            ViewBag.ListLTM = lstLTM;

            //List PC mới
            var lstPCM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false).ToList();
            //Gán vào viewbag
            ViewBag.ListPCM = lstPCM;

            //List dt mới
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 7 && n.Moi == 1 && n.DaXoa == false).ToList();
            //Gán vào viewbag
            ViewBag.ListDTM = lstDTM;

            return View();
        }

        public ActionResult MenuPartial()
        {
            //Lấy ra 1 lst sanpham và truyền trực tiếp vào partial
            var lstSP = db.SanPhams;

            return PartialView(lstSP);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            //đặt trùng tên viewbag giống trong bảng
            ViewBag.CauHoi = new SelectList(LoadCauHoi());  //gắn các câu hỏi vào viewbag để hiển thị lên view

            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)    //dùng post để truyền data lên csdl, dùng biến tv trong model thay formcollection
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());  //lưu câu hỏi đã chọn trong dropdownlist vào csdl
            //Kiểm tra captcha hợp lệ
            if (this.IsCaptchaValid("Captcha không hợp lệ!"))   //nếu captcha hợp lệ
            {
                if (ModelState.IsValid)
                {
                    tv.MaLoaiTV = 3;
                    ViewBag.ThongBao = "Thêm thành công";
                    //Thêm khách hàng vào csdl
                    db.ThanhViens.Add(tv);  //sau khi lấy được các thuộc tính trong biến tv qua các textbox thì truyền tv vào dbset ThanhViens
                    //Lưu thay đổi
                    db.SaveChanges();   //lấy data từ dbset chuyển vào csdl
                }
                return View();
            }
            ViewBag.ThongBao = "Sai mã Captcha";
            return View();
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

        //xd action dang nhap
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            ////ktra tên dn và pass
            //string sTaiKhoan = f["txtTenDangNhap"].ToString();  //lấy chuỗi trong txtTenDangNhap
            //string sMatKhau = f["txtMatKhau"].ToString();   //lấy chuỗi trong txtMatKhau

            //ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);    //so sánh với tk và mk trong csdl
            //if(tv != null)
            //{
            //    Session["TaiKhoan"] = tv;   //tạo session tên TaiKhoan với giá trị là biến tv
            //    return Content(@"<script>window.location.reload()</script>");   //đoạn script dùng để reload lại trang khi đăng nhập thành công
            //}
            //// khi đăng nhập sai xuất thông báo
            //return Content("Tài khoản hoặc mật khẩu không chính xác.");

            //ktra tên dn và pass
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
                    return Content(@"<script>window.location.reload()</script>");   //đoạn script dùng để reload lại trang khi đăng nhập thành công
                }
            }
            return Content("Tài khoản hoặc mật khẩu không chính xác.");
        }

        


        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null; //thiết lập session là null

            FormsAuthentication.SignOut();  //xóa bộ nhớ cookie

            return RedirectToAction("Index");
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