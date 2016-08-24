using FDB.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
//using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FDB.Controllers
{
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //[Authorize(Roles = "Admin, CanEditGroup, CanEditUser")]
        public ActionResult Index(string txtTenNhom)
        {
            string _MaTTP = "";
            var listGroup = new List<Group>();
            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                listGroup = db.Groups.Where(g => (txtTenNhom == null || txtTenNhom == "") || g.Name.ToUpper().Contains(txtTenNhom.ToUpper())).ToList();
            }
            else
            {
                 _MaTTP= db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                var s = db.Groups.Where(g => (txtTenNhom == null || txtTenNhom == "") || g.Name.ToUpper().Contains(txtTenNhom.ToUpper())).ToList();

                listGroup = s.Where(m => m.Ma_TTP == _MaTTP).ToList();
            }


            DTINHTP _TTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.FirstOrDefault(m => m.MA_TINHTP == _MaTTP);
            if (_TTP != null)
                ViewBag.TenTTP = _TTP.TEN_TINHTP;

            ViewBag.TotalRow = listGroup.Count();

            return View(listGroup);
        }


        //[Authorize(Roles = "Admin, CanEditGroup, CanEditUser")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }


        //[Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult Create()
        {
            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                List<DTINHTP> lstTTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.ToList();
                lstTTP.Insert(0, new DTINHTP() { MA_TINHTP = "", TEN_TINHTP = "" });
                ViewBag.DMTThanhPho = new SelectList(lstTTP, "MA_TINHTP", "TEN_TINHTP");
            }
            else
            {
                string Ma_TTP = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                ViewBag.DMTThanhPho = new SelectList((new FDB.DataAccessLayer.FDBContext()).DTINHTP.Where(d => d.MA_TINHTP == Ma_TTP), "MA_TINHTP", "TEN_TINHTP");
            }
            return View();
        }


        //[Authorize(Roles = "Admin, CanEditGroup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Ma_TTP")] Group group)
        {

            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }


        //[Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult Edit(int? id)
        {
            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                List<DTINHTP> lstTTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.ToList();
                lstTTP.Insert(0, new DTINHTP() { MA_TINHTP = "", TEN_TINHTP = "" });
                ViewBag.DMTThanhPho = new SelectList(lstTTP, "MA_TINHTP", "TEN_TINHTP");
            }
            else
            {
                string Ma_TTP = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                ViewBag.DMTThanhPho = new SelectList((new FDB.DataAccessLayer.FDBContext()).DTINHTP.Where(d => d.MA_TINHTP == Ma_TTP), "MA_TINHTP", "TEN_TINHTP");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }


        //[Authorize(Roles = "Admin, CanEditGroup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Ma_TTP")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }


        //[Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }


        //[Authorize(Roles = "Admin, CanEditGroup")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            var idManager = new IdentityManager();
            idManager.DeleteGroup(id);
            return RedirectToAction("Index");
        }


        //[Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult GroupRoles(int id)
        {
            var group = db.Groups.Find(id);
            var model = new SelectGroupRolesViewModel(group, User.Identity.Name);
            return View(model);
        }


        [HttpPost]
        //[Authorize(Roles = "Admin, CanEditGroup")]
        [ValidateAntiForgeryToken]
        public ActionResult GroupRoles(SelectGroupRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var Db = new ApplicationDbContext();
                var group = Db.Groups.Find(model.GroupId);
                idManager.ClearGroupRoles(model.GroupId);

                // Add each selected role to this group:
                foreach (var role in model.Roles)
                {
                    if (role.Selected)
                    {
                        idManager.AddRoleToGroup(group.Id, role.RoleName);
                    }
                }
                return RedirectToAction("index");
            }
            return View();
        }



        //[Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult GroupRemoveUsers(int id)
        {
            var group = db.Groups.Find(id);

            string _MaTTP = "";
            _MaTTP = User.Identity.Name.ToUpper() == "ADMIN" ? "" : db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).MA_TINHTP;

            var model = new SelectUsersInGroupViewModel(group);

            DTINHTP _TTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.FirstOrDefault(m => m.MA_TINHTP == _MaTTP);
            if (_TTP != null)
                ViewBag.TenTTP = _TTP.TEN_TINHTP;

            return View(model);
        }

        [HttpPost, ActionName("GroupRemoveUsers")]
        //[Authorize(Roles = "Admin, CanEditGroup")]
        [ValidateAntiForgeryToken]
        public ActionResult GroupRemoveUsers(SelectUsersInGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();


                var groupInSelectedIds = model.GroupInSelectedIds();
                foreach (var userId in groupInSelectedIds)
                {
                    idManager.RemoveUserGroups(userId, model.GroupId);
                }

                return RedirectToAction("index");
            }
            return View();
        }

        //[Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult GroupAddUsers(int id, string userName, string email)
        {
            var group = db.Groups.Find(id);

            string _MaTTP = "";
            _MaTTP = User.Identity.Name.ToUpper() == "ADMIN" ? "" : db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).MA_TINHTP;

            var model = new SelectUsersOutGroupViewModel(group, _MaTTP);

            if (!string.IsNullOrEmpty(userName))
            {
                model.UserOutGroup = model.UserOutGroup.Where(s => s.UserName != null && s.UserName.ToUpper().Contains(userName.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(email))
            {
                model.UserOutGroup = model.UserOutGroup.Where(s => s.Email != null && s.Email.ToUpper().Contains(email.ToUpper())).ToList();
            }

            DTINHTP _TTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.FirstOrDefault(m => m.MA_TINHTP == _MaTTP);
            if (_TTP != null)
                ViewBag.TenTTP = _TTP.TEN_TINHTP;

            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin, CanEditGroup")]
        [ValidateAntiForgeryToken]
        public ActionResult GroupAddUsers(SelectUsersOutGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();


                var groupOutSelectedIds = model.GroupOutSelectedIds();
                foreach (var userId in groupOutSelectedIds)
                {
                    idManager.AddUserToGroup(userId, model.GroupId);
                }

                return RedirectToAction("index");
            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #region"Phân quyền menu"
        //Add menu to Role
        public ActionResult GroupAddMenu(string id)
        {

            ApplicationDbContext db = new ApplicationDbContext();

            var _menu = new List<MenuItem>();

            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                
                _menu = db.Database.SqlQuery<MenuItem>(@"select * from MenuItems where  isMenu=1 and isDisplay=1 ").ToList();
            }
            else
            {
                //                _menu = db.Database.SqlQuery<MenuItem>(@"select distinct e.* from AspNetRoles a inner join ApplicationRoleGroups b 
                //								                                    on a.Id=b.RoleId
                //							                                      inner join MenuItemRoles c
                //								                                    on a.Name=c.Role_ID
                //							                                      inner join ApplicationUserGroups d
                //								                                    on b.GroupId=d.GroupId
                //							                                      inner join MenuItems e
                //								                                    on c.MenuItem_ID=e.Item_ID
                //							                                      inner join AspNetUsers f
                //								                                    on d.UserId=f.Id
                //                                                                where  UserName=@UserName and e.isMenu=1"
                //                  , new SqlParameter("@UserName", User.Identity.Name)
                //                  ).ToList();

                _menu = db.MenuItems.Where(m => m.isMenu == true &&m.isDisplay==true).ToList();
            }


            int _groupId = int.Parse(id);
            var _menuinGroup = db.MenuItemGroups.Where(m => m.Group_ID == _groupId).ToList();

            List<RoleAddMenuViewModel> _lstViewMenu = new List<RoleAddMenuViewModel>();
            foreach (var item1 in _menu)
            {
                RoleAddMenuViewModel _itemView = new RoleAddMenuViewModel(item1);
                foreach (var item2 in _menuinGroup)
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
            ViewBag.GroupName = db.Groups.FirstOrDefault(g => g.Id == _groupId).Name;
            ViewBag.GroupId = _groupId.ToString();
            return View(tree);
        }

        [HttpPost]
        public ActionResult GroupAddMenu(FormCollection _form)
        {
            ViewBag.SuccessGroup = "";
            string GroupName = _form["GroupName"];
            int _GroupId = int.Parse(_form["GroupId"].ToString());
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
            db.MenuItemGroups.Where(o => o.Group_ID == _GroupId).ToList().ForEach(o => db.MenuItemGroups.Remove(o));

            string _MaTTP = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).MA_TINHTP;
            foreach (var item in lstIDDistinct)
            {
                db.MenuItemGroups.Add(new MenuItemGroup() { MenuItem_ID = item, Group_ID = _GroupId, Ma_TTP = _MaTTP });
            }
            db.SaveChanges();

//            var _menu = db.Database.SqlQuery<MenuItem>(@"select distinct e.* from AspNetRoles a inner join ApplicationRoleGroups b 
//								                                    on a.Id=b.RoleId
//							                                      inner join MenuItemRoles c
//								                                    on a.Name=c.Role_ID
//							                                      inner join ApplicationUserGroups d
//								                                    on b.GroupId=d.GroupId
//							                                      inner join MenuItems e
//								                                    on c.MenuItem_ID=e.Item_ID
//							                                      inner join AspNetUsers f
//								                                    on d.UserId=f.Id
//                                                                where f.UserName=@UserName and e.isMenu=1"
//                  , new SqlParameter("@UserName", User.Identity.Name));

            var _menu = db.MenuItems.Where(m => m.isMenu == true).ToList();

            var _menuinGroup = db.MenuItemGroups.Where(m => m.Group_ID == _GroupId).ToList();

            List<RoleAddMenuViewModel> _lstViewMenu = new List<RoleAddMenuViewModel>();
            foreach (var item1 in _menu)
            {
                RoleAddMenuViewModel _itemView = new RoleAddMenuViewModel(item1);
                foreach (var item2 in _menuinGroup)
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
            ViewBag.GroupName = GroupName;
            ViewBag.GroupId = _GroupId.ToString();
            ViewBag.SuccessGroup = "Phân quyền nhóm " + GroupName + " thành công!";
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
    }
}
