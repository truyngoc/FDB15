using FDB.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FDB.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var rolesList = new List<RoleViewModel>();
            var _Roles = new List<ApplicationRole>();
            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                _Roles = _db.Roles.ToList();
            }
            else
            {
                string Ma_TTP = _db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                _Roles = _db.Roles.Where(m => m.Ma_TTP == Ma_TTP).ToList();
            }


            foreach (var role in _Roles)
            {
                var roleModel = new RoleViewModel(role);
                rolesList.Add(roleModel);
            }

            ViewBag.TotalRow = rolesList.Count;

            return View(rolesList);
        }


        //[Authorize(Roles = "Admin")]
        public ActionResult Create(string message = "")
        {
            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                List<DTINHTP> lstTTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.ToList();
                lstTTP.Insert(0, new DTINHTP() { MA_TINHTP = "", TEN_TINHTP = "" });
                ViewBag.DMTThanhPho = new SelectList(lstTTP, "MA_TINHTP", "TEN_TINHTP");
            }
            else
            {
                string Ma_TTP = _db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                ViewBag.DMTThanhPho = new SelectList((new FDB.DataAccessLayer.FDBContext()).DTINHTP.Where(d => d.MA_TINHTP == Ma_TTP), "MA_TINHTP", "TEN_TINHTP");
            }


            ViewBag.Message = message;
            return View();
        }


        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include =
            "RoleName,Description,Ma_TTP")]RoleViewModel model)
        {
            string message = "That role name has already been used";
            if (ModelState.IsValid)
            {
                // model.Ma_TTP = _db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                var role = new ApplicationRole(model.RoleName, model.Description, model.Ma_TTP);
                var idManager = new IdentityManager();

                if (idManager.RoleExists(model.RoleName))
                {
                    return View(message);
                }
                else
                {
                    idManager.CreateRole(model.RoleName, model.Description, model.Ma_TTP);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }


        //[Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                List<DTINHTP> lstTTP = (new FDB.DataAccessLayer.FDBContext()).DTINHTP.ToList();
                lstTTP.Insert(0, new DTINHTP() { MA_TINHTP = "", TEN_TINHTP = "" });
                ViewBag.DMTThanhPho = new SelectList(lstTTP, "MA_TINHTP", "TEN_TINHTP");
            }
            else
            {
                string Ma_TTP = _db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                ViewBag.DMTThanhPho = new SelectList((new FDB.DataAccessLayer.FDBContext()).DTINHTP.Where(d => d.MA_TINHTP == Ma_TTP), "MA_TINHTP", "TEN_TINHTP");
            }
            // It's actually the Role.Name tucked into the id param:
            var role = _db.Roles.First(r => r.Name == id);
            var roleModel = new EditRoleViewModel(role);
            return View(roleModel);
        }


        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include =
            "RoleName,OriginalRoleName,Description,Ma_TTP")] EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = _db.Roles.First(r => r.Name == model.OriginalRoleName);
                //  model.Ma_TTP = _db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).MA_TINHTP;
                role.Name = model.RoleName;
                role.Description = model.Description;
                role.Ma_TTP = model.Ma_TTP;
                _db.Entry(role).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        //[Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = _db.Roles.First(r => r.Name == id);
            var model = new RoleViewModel(role);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        //[Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var role = _db.Roles.First(r => r.Name == id);
            var idManager = new IdentityManager();
            idManager.DeleteRole(role.Id);
            return RedirectToAction("Index");
        }


        //Add menu to Role
        public ActionResult RoleAddMenu(string id)
        {
            ViewBag.RoleName = id;
            ApplicationDbContext db = new ApplicationDbContext();
         
            var _menu = new List<MenuItem>();

            if (User.Identity.Name.ToUpper() == "ADMIN")
            {
                _menu = db.Database.SqlQuery<MenuItem>(@"select * from MenuItems where  isMenu=1 ").ToList();
            }
            else
            {
                _menu = db.Database.SqlQuery<MenuItem>(@"select distinct e.* from AspNetRoles a inner join ApplicationRoleGroups b 
								                                    on a.Id=b.RoleId
							                                      inner join MenuItemRoles c
								                                    on a.Name=c.Role_ID
							                                      inner join ApplicationUserGroups d
								                                    on b.GroupId=d.GroupId
							                                      inner join MenuItems e
								                                    on c.MenuItem_ID=e.Item_ID
							                                      inner join AspNetUsers f
								                                    on d.UserId=f.Id
                                                                where  UserName=@UserName and e.isMenu=1"
                  , new SqlParameter("@UserName", User.Identity.Name)
                  ).ToList();
            }



            var _menuinRole = db.MenuItemRoles.Where(m => m.Role_ID == id).ToList();

            List<RoleAddMenuViewModel> _lstViewMenu = new List<RoleAddMenuViewModel>();
            foreach (var item1 in _menu)
            {
                RoleAddMenuViewModel _itemView = new RoleAddMenuViewModel(item1);
                foreach (var item2 in _menuinRole)
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

            Session["_menu"] = _lstViewMenu;
         
            RoleAddMenuViewModel root = new RoleAddMenuViewModel();
            root.Item_ID = "0";
            root.Item_name = "Đây là root node";
            _lstViewMenu.Add(root);

            var tree = _lstViewMenu.ToTree();

            return View(tree);
        }

        [HttpPost]
        public ActionResult RoleAddMenu(FormCollection _form)
        {

            string RoleName = _form["RoleName"];
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
            List<string> lstIDDistinct = lstID.Distinct().ToList();
            List<string> lstItemisNotMenu = _db.Database.SqlQuery<MenuItem>(@"select * from MenuItems where isMenu=0").Select(m => m.Item_ID).ToList<string>();
            lstIDDistinct.AddRange(lstItemisNotMenu);
            //add Menu to Role:
            ApplicationDbContext db = new ApplicationDbContext();
            //Xóa nMenu cũ:
            db.MenuItemRoles.Where(o => o.Role_ID == RoleName).ToList().ForEach(o => db.MenuItemRoles.Remove(o));
          
            string _MaTTP = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).MA_TINHTP;
            foreach (var item in lstIDDistinct)
            {
                db.MenuItemRoles.Add(new MenuItemRole() { MenuItem_ID = item, Role_ID = RoleName, Ma_TTP = _MaTTP });
            }
            db.SaveChanges();

            var _menu = db.Database.SqlQuery<MenuItem>(@"select distinct e.* from AspNetRoles a inner join ApplicationRoleGroups b 
								                                    on a.Id=b.RoleId
							                                      inner join MenuItemRoles c
								                                    on a.Name=c.Role_ID
							                                      inner join ApplicationUserGroups d
								                                    on b.GroupId=d.GroupId
							                                      inner join MenuItems e
								                                    on c.MenuItem_ID=e.Item_ID
							                                      inner join AspNetUsers f
								                                    on d.UserId=f.Id
                                                                where f.UserName=@UserName and e.isMenu=1"
                  , new SqlParameter("@UserName", User.Identity.Name));

            var _menuinRole = db.MenuItemRoles.Where(m => m.Role_ID == RoleName).ToList();

            List<RoleAddMenuViewModel> _lstViewMenu = new List<RoleAddMenuViewModel>();
            foreach (var item1 in _menu)
            {
                RoleAddMenuViewModel _itemView = new RoleAddMenuViewModel(item1);
                foreach (var item2 in _menuinRole)
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

            RoleAddMenuViewModel root = new RoleAddMenuViewModel();
            root.Item_ID = "0";
            root.Item_name = "Đây là root node";
            _lstViewMenu.Add(root);

            var tree = _lstViewMenu.ToTree();

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
    }
}
