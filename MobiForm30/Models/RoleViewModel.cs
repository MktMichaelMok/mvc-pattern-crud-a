using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobiForm30.Models {
    public class RoleViewModel {

        [Display(Name = "序號")]
        public long RoleSeq { set; get; }

        [Display(Name = "權限ID")]
        [StringLength(32), Required(ErrorMessage = "請輸入權限ID")]        
        public string RoleID { set; get; }

        [Display(Name = "權限名稱")]
        [StringLength(32), Required(ErrorMessage = "請輸入權限名稱")]
        public string RoleName { set; get; }

        [Display(Name = "使用者")]        
        public string UserNameList { set; get; }
    }
}