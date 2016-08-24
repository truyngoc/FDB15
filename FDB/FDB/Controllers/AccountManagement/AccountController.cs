using FDB.Models;
using FDB.DataAccessLayer;
using FDB.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Data.SqlClient;
using System;
using System.Data.Entity.Validation;
using System.Net;

using CaptchaMvc;
using CaptchaMvc.HtmlHelpers;

namespace FDB.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        #region "Manage User"

        //[Authorize(Roles = "Admin")]
        public ActionResult Index(int? page, string txtUserName, string txtEmail, string txtHoTen)
        {
            var Db = new ApplicationDbContext();
            var users = new List<ApplicationUser>();
            string _MaTTP = "";
            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                users = Db.Users.Where(u => ((txtUserName == null || txtUserName == "") || u.UserName.ToUpper().Contains(txtUserName.ToUpper()))
                                       && ((txtEmail == null || txtEmail == "") || u.Email.ToUpper().Contains(txtEmail.ToUpper()))
                                       && ((txtHoTen == null || txtHoTen == "") || (u.FirstName.ToUpper().Contains(txtHoTen.ToUpper()) || u.LastName.ToUpper().Contains(txtHoTen.ToUpper())))
                          ).OrderBy(o => o.UserName).ToList();
            }
            else
            {
                _MaTTP = Db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).MA_TINHTP;
                users = Db.Users.Where(u => ((txtUserName == null || txtUserName == "") || u.UserName.ToUpper().Contains(txtUserName.ToUpper()))
                                       && ((txtEmail == null || txtEmail == "") || u.Email.ToUpper().Contains(txtEmail.ToUpper()))
                                       && ((txtHoTen == null || txtHoTen == "") || (u.FirstName.ToUpper().Contains(txtHoTen.ToUpper()) || u.LastName.ToUpper().Contains(txtHoTen.ToUpper())))
                                       && (u.MA_TINHTP == _MaTTP)
                          ).OrderBy(o => o.UserName).ToList();
            }

            var model = new List<EditUserViewModel>();
            foreach (var user in users)
            {
                var u = new EditUserViewModel(user);
                model.Add(u);
            }

            DTINHTP _TTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.FirstOrDefault(m => m.MA_TINHTP == _MaTTP);
            if (_TTP != null)
                ViewBag.TenTTP = _TTP.TEN_TINHTP;

            ViewBag.TotalRow = model.Count;
            int pageSize = FDB.Common.Constants.PageSize; ;
            int pageNumber = page ?? 1;     // null-coalescing operator: return the value of page if it has a value, or return 1 if page is null.

            return View(model.ToPagedList(pageNumber, pageSize));
        }


        //[Authorize(Roles = "Admin")]
        public ActionResult Edit(string id, ManageMessageId? Message = null)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            ViewBag.MessageId = Message;

            initialCategoryEditAction(model);

            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Db = new ApplicationDbContext();
                var user = Db.Users.First(u => u.UserName == model.UserName);

                // Update the user data:
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                // them truong
                user.MA_TINHTP = model.MA_TINHTP;
                user.MA_QUANHUYEN = model.MA_QUANHUYEN;
                user.MA_PHUONGXA = model.MA_PHUONGXA;
                user.DON_VI_CONG_TAC = model.DON_VI_CONG_TAC;
                user.CHUC_VU = model.CHUC_VU;

                Db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            initialCategoryEditAction(model);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        public ActionResult Delete(string id = null)
        {
            TempData["tmpErrorDelete"] = "";
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            else if (user.UserName == User.Identity.Name)
            {
                TempData["tmpErrorDelete"] = "Bạn không được xóa chính mình";
                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            Db.Users.Remove(user);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }


        #endregion

        #region "Manage Groups"
        //[Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult UserGroups(string id)
        {
            var _db = new ApplicationDbContext();
            var user = _db.Users.First(u => u.UserName == id);
            var model = new SelectUserGroupsViewModel(user, User.Identity.Name);

            string _MaTTP = "";
            _MaTTP = User.Identity.Name.ToUpper() == "ADMIN" ? "" : _db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).MA_TINHTP;
            DTINHTP _TTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.FirstOrDefault(m => m.MA_TINHTP == _MaTTP);
            if (_TTP != null)
                ViewBag.TenTTP = _TTP.TEN_TINHTP;

            return View(model);
        }


        [HttpPost]
        //[Authorize(Roles = "Admin, CanEditUser")]
        [ValidateAntiForgeryToken]
        public ActionResult UserGroups(SelectUserGroupsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var _db = new ApplicationDbContext();
                var user = _db.Users.First(u => u.UserName == model.UserName);
                idManager.ClearUserGroups(user.Id);
                foreach (var group in model.Groups)
                {
                    if (group.Selected)
                    {
                        idManager.AddUserToGroup(user.Id, group.GroupId);
                    }
                }
                return RedirectToAction("index");
            }
            return View();
        }


        //[Authorize(Roles = "Admin, CanEditRole, CanEditGroup, User")]
        public ActionResult UserPermissions(string id)
        {
            var _db = new ApplicationDbContext();
            var user = _db.Users.First(u => u.UserName == id);
            var model = new UserPermissionsViewModel(user);
            return View(model);
        }
        #endregion

        #region "Manage Roles"

        //[Authorize(Roles = "Admin")]
        public ActionResult UserRoles(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new SelectUserRolesViewModel(user);
            return View(model);
        }


        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult UserRoles(SelectUserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var Db = new ApplicationDbContext();
                var user = Db.Users.First(u => u.UserName == model.UserName);
                idManager.ClearUserRoles(user.Id);
                foreach (var role in model.Roles)
                {
                    if (role.Selected)
                    {
                        idManager.AddUserToRole(user.Id, role.RoleName);
                    }
                }
                return RedirectToAction("index");
            }
            return View();
        }
        #endregion

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);

                if (this.IsCaptchaValid("Mã kiểm tra không đúng"))
                {
                    if (user != null)
                    {
                        await SignInAsync(user, model.RememberMe);

                        //Insert Logaccount
                        var db = new ApplicationDbContext();
                        var _TEN_TTP = "";
                         _TEN_TTP = (new FDBContext()).DTINHTP.FirstOrDefault(o => o.MA_TINHTP == user.MA_TINHTP).TEN_TINHTP;

                        //var _accountlog = new AccountLog { Username = model.UserName, Logtime = DateTime.Now, Fullname = user.FirstName + " " + user.LastName, MA_TTP = user.MA_TINHTP, TEN_TTP = _TEN_TTP };
                        var _accountlog = new AccountLog { Username = model.UserName, Logtime = DateTime.Now, Fullname = user.FirstName + " " + user.LastName, MA_TTP = user.MA_TINHTP, TEN_TTP = _TEN_TTP };
                        db.AccountLogs.Add(_accountlog);
                        db.SaveChanges();
                        Session["IdLog"] = _accountlog.Id;

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu!");
                    }
                }
                else
                {
                    ViewBag.ErrMessage = "Lỗi: Mã kiểm tra không đúng";
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/Register        
        [AllowAnonymous]
        public ActionResult Register()
        {
            initialCategoryCreateAction();

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = new ApplicationUser() { 
                //    UserName = model.UserName,
                //    FirstName = model.FirstName,
                //    LastName = model.LastName,
                //    Email = model.Email                    
                //};

                var user = model.GetUser();

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Account");
                }
                //else
                //{
                //    AddErrors(result);
                //}
            }

            initialCategoryCreateAction();

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public JsonResult check_User_Exist(string UserName, int isEdit)
        {
            if (isEdit == 0)
            {
                var _user = UserManager.Users.FirstOrDefault(u => u.UserName == UserName);

                return Json(_user == null);
            }

            return Json(true);
        }
        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session["MyMenu"] = null;
            Session["MyTreeMenu"] = null;
            Session["lstMenu"] = null;
            if (Session["IdLog"] != null)
            {
                var db = new ApplicationDbContext();
                int Id = int.Parse(Session["IdLog"].ToString());
                var _accountlog = db.AccountLogs.Find(Id);
                if (_accountlog != null)
                {
                    _accountlog.LogtimeEnd = DateTime.Now;
                    db.SaveChanges();
                }

                Session["IdLog"] = null;
            }



            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Login", "Account");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion


        #region "load danh muc"
        private FDBContext fdbContext = new FDBContext();
        public void initialCategoryCreateAction()
        {
            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                List<DTINHTP> lstTTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.ToList();
                lstTTP.Insert(0, new DTINHTP() { MA_TINHTP = "", TEN_TINHTP = "" });
                ViewBag.TINH_THANHPHOs = new SelectList(lstTTP, "MA_TINHTP", "TEN_TINHTP");
                ViewBag.QUAN_HUYENs = CategoryCommon.DEFAULT_VALUE_DDL;
            }
            else
            {
                var db = new ApplicationDbContext();
                string Ma_TTP = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                ViewBag.TINH_THANHPHOs = new SelectList((new FDB.DataAccessLayer.FDBContext()).DTINHTP.Where(d => d.MA_TINHTP == Ma_TTP), "MA_TINHTP", "TEN_TINHTP");

                List<DQUANHUYEN> lstQH = fdbContext.DQUANHUYEN.Where(q => q.MA_TINHTP == Ma_TTP).ToList();
                lstQH.Insert(0, new DQUANHUYEN() { MA_QUANHUYEN = "", TEN_QUANHUYEN = "" });
                ViewBag.QUAN_HUYENs = new SelectList(lstQH, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            }


            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }

        public void initialCategoryEditAction(EditUserViewModel user)
        {

            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                List<DTINHTP> lstTTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.ToList();
                lstTTP.Insert(0, new DTINHTP() { MA_TINHTP = "", TEN_TINHTP = "" });
                ViewBag.TINH_THANHPHOs = new SelectList(lstTTP, "MA_TINHTP", "TEN_TINHTP");

            }
            else
            {
                var db = new ApplicationDbContext();
                string Ma_TTP = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                ViewBag.TINH_THANHPHOs = new SelectList((new FDB.DataAccessLayer.FDBContext()).DTINHTP.Where(d => d.MA_TINHTP == Ma_TTP), "MA_TINHTP", "TEN_TINHTP");


            }

            var quanhuyen = fdbContext.DQUANHUYEN.Where(d => d.MA_TINHTP == user.MA_TINHTP).ToList();
            var phuongxa = fdbContext.DPHUONGXA.Where(d => d.MA_QUANHUYEN == user.MA_QUANHUYEN).ToList();

            ViewBag.QUAN_HUYENs = new SelectList(quanhuyen, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.PHUONG_XAs = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }

        [HttpPost]
        public ActionResult getDistrict(string ma_TinhTP)
        {
            var districts = fdbContext.DQUANHUYEN.Where(d => d.MA_TINHTP == ma_TinhTP).Select(a => "<option value='" + a.MA_QUANHUYEN + "'>" + a.TEN_QUANHUYEN + "</option>");

            return Content(string.Join("", districts));
        }

        [HttpPost]
        public ActionResult getWard(string ma_QuanHuyen)
        {
            var districts = fdbContext.DPHUONGXA.Where(d => d.MA_QUANHUYEN == ma_QuanHuyen).Select(a => "<option value='" + a.MA_PHUONGXA + "'>" + a.TEN_PHUONGXA + "</option>");

            return Content(string.Join("", districts));
        }
        #endregion


        #region"Phân quyền menu"
        //Add menu to Role
        public ActionResult UserAddMenu(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var _menu = new List<MenuItem>();

            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                _menu = db.Database.SqlQuery<MenuItem>(@"select * from MenuItems where  isMenu=1 and isDisplay=1").ToList();
            }
            else
            {
                _menu = db.MenuItems.Where(m => m.isMenu == true && m.isDisplay == true).ToList();
            }


            string _UserId = db.Users.FirstOrDefault(u => u.UserName == id).Id;
            var _menuinUser = db.MenuItemUsers.Where(m => m.User_ID == _UserId).ToList();

            List<RoleAddMenuViewModel> _lstViewMenu = new List<RoleAddMenuViewModel>();
            foreach (var item1 in _menu)
            {
                RoleAddMenuViewModel _itemView = new RoleAddMenuViewModel(item1);
                foreach (var item2 in _menuinUser)
                {
                    if (item2.MenuItem_ID == item1.Item_ID)
                    {
                        _itemView._checked = "checked";
                        _itemView.isCheck = true;
                    }
                    else
                        _itemView.isCheck = false;
                }
                _lstViewMenu.Add(_itemView);
            }

            //Session["_menu"] = _lstViewMenu;

            RoleAddMenuViewModel root = new RoleAddMenuViewModel();
            root.Item_ID = "0";
            root.Item_name = "Đây là root node";
            _lstViewMenu.Add(root);

            var tree = _lstViewMenu.ToTree();
            ViewBag.UserName = id;
            ViewBag.UserId = _UserId;
            return View(tree);
        }

        [HttpPost]
        public ActionResult UserAddMenu(FormCollection _form)
        {
            ViewBag.SuccessUser = "";
            string UserName = _form["UserName"];
            string _UserId = _form["UserId"].ToString();
            //lấy danh sách ID:
            List<string> lstID = new List<string>();

            foreach (var _key in _form.AllKeys)
            {
                if (_key.StartsWith("chk_"))
                {
                    string sID = _form[_key].Split('_')[0];
                    string spID = _form[_key].Split('_')[1];

                    lstID.Add(sID);
                    lstID.Add(spID);
                }

            }
            ApplicationDbContext db = new ApplicationDbContext();
            List<string> lstIDDistinct = lstID.Distinct().ToList();
            List<string> lstItemisNotMenu = db.Database.SqlQuery<MenuItem>(@"select * from MenuItems where isMenu=0").Select(m => m.Item_ID).ToList<string>();
            lstIDDistinct.AddRange(lstItemisNotMenu);
            //add Menu to Role:

            //Xóa nMenu cũ:
            try
            {
                db.MenuItemUsers.Where(o => o.User_ID == _UserId).ToList().ForEach(o => db.MenuItemUsers.Remove(o));

                string _MaTTP = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).MA_TINHTP;
                foreach (var item in lstIDDistinct)
                {
                    db.MenuItemUsers.Add(new MenuItemUser() { MenuItem_ID = item, User_ID = _UserId, Ma_TTP = _MaTTP });
                }
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                var str1 = "";
                var str = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    str = String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                         eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        str1 += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                              ve.PropertyName, ve.ErrorMessage);
                    }
                }
                ViewBag.Error = str + str1;

            }
            finally
            { }


            var _menu = db.MenuItems.Where(m => m.isMenu == true).ToList();

            var _menuinUser = db.MenuItemUsers.Where(m => m.User_ID == _UserId).ToList();

            List<RoleAddMenuViewModel> _lstViewMenu = new List<RoleAddMenuViewModel>();
            foreach (var item1 in _menu)
            {
                RoleAddMenuViewModel _itemView = new RoleAddMenuViewModel(item1);
                foreach (var item2 in _menuinUser)
                {
                    if (item2.MenuItem_ID == item1.Item_ID)
                    {
                        _itemView._checked = "checked";
                        _itemView.isCheck = true;
                    }
                    else
                        _itemView.isCheck = false;
                }
                _lstViewMenu.Add(_itemView);
            }

            //Session["_menu"] = _lstViewMenu;

            RoleAddMenuViewModel root = new RoleAddMenuViewModel();
            root.Item_ID = "0";
            root.Item_name = "Đây là root node";
            _lstViewMenu.Add(root);

            var tree = _lstViewMenu.ToTree();
            ViewBag.UserName = UserName;
            ViewBag.UserId = _UserId.ToString();
            ViewBag.SuccessUser = "Phân quyền người dùng " + UserName + " thành công!";
            return View(tree);
        }

        [ChildActionOnly]
        public PartialViewResult ChildrenCheckbox(string ParentID)
        {

            using (var db = new ApplicationDbContext())
            {
                //var _menu = from menu in db.MenuItems
                //            where menu.Parent_ID == ParentID
                //            select menu;

                List<RoleAddMenuViewModel> _lstmenu = Session["_menu"] as List<RoleAddMenuViewModel>;
                List<RoleAddMenuViewModel> _menu = _lstmenu.Where(m => m.Parent_ID == ParentID).ToList();
                return PartialView(_menu.ToList());
            }
        }
        #endregion

        #region "User Login"
        public ActionResult AccountLogs(ViewModelSearchAcc_Login SearchModel)
        {
            initialCategorySearchAction();
            var Db = new ApplicationDbContext();

            var account = Db.AccountLogs.Where(u =>( (SearchModel.MA_TINHTP == null || u.MA_TTP == SearchModel.MA_TINHTP)
                                 && (SearchModel.TU_NGAY == null || u.Logtime >= SearchModel.TU_NGAY)
                                 && (SearchModel.DEN_NGAY == null || u.Logtime <= SearchModel.DEN_NGAY))
                      );


            var result = account.GroupBy(a => new { a.MA_TTP, a.Username, a.TEN_TTP })
                
                                .Select(b => new ViewModelAccountLogs() { MA_TINHTP = b.Key.MA_TTP, TEN_TTP = b.Key.TEN_TTP, UserName = b.Key.Username, SO_LAN_DANG_NHAP = b.Count() });

          
            
       
            SearchModel.SearchResults = result.ToList();

            ViewBag.TotalRow = result.Count();

            return View(SearchModel);
        }

        public ActionResult DetailAccountLogs(int? page, string ma_ttp, string username)
        {

            //ViewModelSearchAcc_Login_Details SearchModel = new ViewModelSearchAcc_Login_Details();
            //SearchModel.MA_TINHTP = ma_ttp;
            //SearchModel.UserName = username;

            var db = new ApplicationDbContext();

            //var result = db.AccountLogs.Where(u => ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) || u.MA_TTP == SearchModel.MA_TINHTP)
            //                  && (string.IsNullOrEmpty(SearchModel.UserName) || u.Username.ToUpper().Contains(SearchModel.UserName.ToUpper())))).OrderByDescending(x => x.Logtime);

            var result = db.AccountLogs.Where(u => ((string.IsNullOrEmpty(ma_ttp) || u.MA_TTP == ma_ttp)
                           && (string.IsNullOrEmpty(username) || u.Username.ToUpper().Contains(username.ToUpper())))).OrderByDescending(x => x.Logtime).ToArray();
 
         

          // int pageSize = FDB.Common.Constants.PageSize;
           int pageSize = 10;
           int pageNumber = page ?? 1;

         

            //SearchModel.SearchResults =  result.ToPagedList(pageNumber, pageSize);

           return PartialView("DetailAccountLogs", result.ToPagedList(pageNumber, pageSize));
        }

        public void initialCategorySearchAction()
        {

            var db = new FDBContext();


            ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
        }

      
            
        #endregion
    }
}