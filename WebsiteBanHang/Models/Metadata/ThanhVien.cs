using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebsiteBanHang.Models         //phải cùng namespace với class trong Model
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    //liên kết class tv này với class tv trong Model (vì nếu update Model.edmx thì validation trong class tv của model sẽ mất hết)
    public partial class ThanhVien      
    {
        internal sealed class ThanhVienMetadata
        {
            public int MaThanhVien { get; set; }

            [DisplayName("Tài khoản")]
            [StringLength(100, ErrorMessage = "Tên tài khoản không quá 100 kí tự.")]
            [Required(ErrorMessage = "Hãy nhập tên {0}.")]
            public string TaiKhoan { get; set; }

            [DisplayName("Họ tên")]
            [StringLength(50, ErrorMessage = "Tên tài khoản không quá 50 kí tự.")]
            [Required(ErrorMessage = "Hãy nhập {0}.")]
            public string HoTen { get; set; }

            [DisplayName("Địa chỉ")]
            [Required(ErrorMessage = "Hãy nhập {0}.")]
            public string DiaChi { get; set; }

            [DisplayName("Email")]
            [Required(ErrorMessage = " Hãy nhập địa chỉ {0}.")]
            [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "{0} không hợp lệ.")]
            public string Email { get; set; }

            [DisplayName("Số điện thoại")]
            [Required(ErrorMessage = "Hãy nhập {0}.")]
            [StringLength(10, ErrorMessage = "{0} không hợp lệ.")]
            public string SoDienThoai { get; set; }

            [DisplayName("Câu hỏi")]
            public string CauHoi { get; set; }

            [DisplayName("Câu trả lời")]
            [Required(ErrorMessage = "Hãy nhập {0}.")]
            public string CauTraLoi { get; set; }
            [DisplayName("Mã loại thành viên")]
            public Nullable<int> MaLoaiTV { get; set; }

            [DisplayName("Mật khẩu")]
            [Required(ErrorMessage = "Hãy nhập mật khẩu.")]
            public string MatKhau { get; set; }

        }
    }
}