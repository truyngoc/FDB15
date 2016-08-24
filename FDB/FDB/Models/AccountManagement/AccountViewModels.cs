using System.ComponentModel.DataAnnotations;

// New namespace imports:
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using System;

namespace FDB.Models
{
    //role:

    public class RoleAddMenuViewModel
    {
        public RoleAddMenuViewModel(MenuItem _menu)
        {
            this.Childens = new List<RoleAddMenuViewModel>();
            this.Item_ID = _menu.Item_ID;
            this.Item_name = _menu.Item_name;
            this.Item_Action = _menu.Item_Action;
            this.Item_Controller = _menu.Item_Controller;
            this.Parent_ID = _menu.Parent_ID;
            this.isMenu = _menu.isMenu;
            this.Order_No = _menu.Order_No;
            this.ExistChild = _menu.ExistChild;
        }

        public RoleAddMenuViewModel()
        {
            this.Childens = new List<RoleAddMenuViewModel>();
        }
        public string Item_ID { get; set; }
        public string Item_name { get; set; }
        public string Item_Action { get; set; }
        public string Item_Controller { get; set; }
        public string Parent_ID { get; set; }
        public bool isMenu { get; set; }
        public int? Order_No { get; set; }

        public bool ExistChild { get; set; }

        public bool isCheck { get; set; }

        public string _checked
        {
            get;
            set;
        }

        public List<RoleAddMenuViewModel> Childens { get; set; }
    }

    public static class RoleAddMenuViewModelExtensions
    {
        public static RoleAddMenuViewModel ToTree(this List<RoleAddMenuViewModel> list)
        {
            if (list == null) throw new ArgumentNullException("list");

            var root = list.FirstOrDefault(x => x.Parent_ID == null);
            if (root == null) throw new InvalidOperationException("root == null");

            PopulateChildren2(root, list);
            return root;
        }

        //recursive method
        private static void PopulateChildren2(RoleAddMenuViewModel node, List<RoleAddMenuViewModel> all)
        {
            var childs = all.Where(x => x.Parent_ID == node.Item_ID).ToList();
            foreach (var item in childs)
            {
                node.ExistChild = true;
                //node.leaf = false;
                node.Childens.Add(item);
            }

            foreach (var item in childs)
                all.Remove(item);

            foreach (var item in childs)
                PopulateChildren2(item, all);
        }
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessage = "Mật khẩu cũ là bắt buộc nhập")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc nhập")]
        [StringLength(100, ErrorMessage = "{0} tối thiểu là {2} ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "{0} và {1} không trùng khớp")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc nhập")]
        [Display(Name = "Tên đăng nhập")]
        [System.Web.Mvc.Remote("check_User_Exist", "Account", HttpMethod = "POST", ErrorMessage = "Tên đăng nhập đã tồn tại", AdditionalFields = "IsEdit")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc nhập")]
        [StringLength(100, ErrorMessage = "{0} tối thiểu là {2} ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "mật khẩu và nhập lại mật khẩu không trùng khớp")]
        public string ConfirmPassword { get; set; }

        // New Fields added to extend Application User class:
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Địa chỉ Email không hợp lệ")]
        public string Email { get; set; }


        // them truong
        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        public string MA_TINHTP { get; set; }
        public string MA_QUANHUYEN { get; set; }
        public string MA_PHUONGXA { get; set; }
        public string DON_VI_CONG_TAC { get; set; }
        public string CHUC_VU { get; set; }


        // Return a pre-poulated instance of AppliationUser:
        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,

                // them truong
                MA_TINHTP = this.MA_TINHTP,
                MA_QUANHUYEN = this.MA_QUANHUYEN,
                MA_PHUONGXA = this.MA_PHUONGXA,
                DON_VI_CONG_TAC = this.DON_VI_CONG_TAC,
                CHUC_VU = this.CHUC_VU
            };
            return user;
        }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditUserViewModel(ApplicationUser user)
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;

            // them truong
            this.MA_TINHTP = user.MA_TINHTP;
            this.MA_QUANHUYEN = user.MA_QUANHUYEN;
            this.MA_PHUONGXA = user.MA_PHUONGXA;
            this.DON_VI_CONG_TAC = user.DON_VI_CONG_TAC;
            this.CHUC_VU = user.CHUC_VU;
        }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Họ đệm")]
        public string FirstName { get; set; }

        [Display(Name = "Tên")]
        public string LastName { get; set; }

        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Địa chỉ Email không hợp lệ")]
        public string Email { get; set; }

        // them truong
        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        public string MA_TINHTP { get; set; }
        public string MA_QUANHUYEN { get; set; }
        public string MA_PHUONGXA { get; set; }
        public string DON_VI_CONG_TAC { get; set; }
        public string CHUC_VU { get; set; }
    }

    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }


        // Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;

            var Db = new ApplicationDbContext();
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Add all available roles to the list of EditorViewModels:
            var allRoles = Db.Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            var currentRoles = new List<string>();
            currentRoles.AddRange(um.GetRoles(user.Id));

            foreach (var userRole in currentRoles)
            {
                var checkUserRole =
                    this.Roles.Find(r => r.RoleName == userRole);
                checkUserRole.Selected = true;
            }
        }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }

    // Used to display a single role with a checkbox, within a list structure:
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }
        public SelectRoleEditorViewModel(ApplicationRole role)
        {
            this.RoleName = role.Name;

            // Assign the new Descrption property:
            this.Description = role.Description;
            this.Ma_TTP = role.Ma_TTP;
        }

        public bool Selected { get; set; }

        [Required]

        [Display(Name = "Tên")]
        public string RoleName { get; set; }

        // Add the new Description property:
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public string Ma_TTP { get; set; }
    }

    public class RoleViewModel
    {
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Tên quyền là bắt buộc nhập")]
        public string RoleName { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public string Ma_TTP { get; set; }

        public RoleViewModel() { }
        public RoleViewModel(ApplicationRole role)
        {
            this.RoleName = role.Name;
            this.Description = role.Description;
            this.Ma_TTP = role.Ma_TTP;
        }
    }

    public class EditRoleViewModel
    {
        public string OriginalRoleName { get; set; }

        [Display(Name = "Tên")]
        public string RoleName { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public string Ma_TTP { get; set; }
        public EditRoleViewModel() { }
        public EditRoleViewModel(ApplicationRole role)
        {
            this.OriginalRoleName = role.Name;
            this.RoleName = role.Name;
            this.Description = role.Description;
            this.Ma_TTP = role.Ma_TTP;
        }
    }
    #region "Group"
    // Wrapper for SelectGroupEditorViewModel to select user group membership:
    public class SelectUserGroupsViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SelectGroupEditorViewModel> Groups { get; set; }

        public SelectUserGroupsViewModel()
        {
            this.Groups = new List<SelectGroupEditorViewModel>();
        }

        public SelectUserGroupsViewModel(ApplicationUser user, string UserName)
            : this()
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;


            var Db = new ApplicationDbContext();
            var allGroups = new List<Group>();
            // Add all available groups to the public list:
            if (UserName.ToUpper() == "ADMIN")
            {
                allGroups = Db.Groups.ToList();
            }
            else
            {
                allGroups = Db.Groups.Where(m => m.Ma_TTP == user.MA_TINHTP).ToList();
            }


            foreach (var role in allGroups)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectGroupEditorViewModel(role);
                this.Groups.Add(rvm);
            }

            // Set the Selected property to true where user is already a member:
            foreach (var group in user.Groups)
            {
                var checkUserRole =
                    this.Groups.Find(r => r.GroupName == group.Group.Name);
                checkUserRole.Selected = true;
            }
        }
    }


    // Used to display a single role group with a checkbox, within a list structure:
    public class SelectGroupEditorViewModel
    {
        public SelectGroupEditorViewModel() { }
        public SelectGroupEditorViewModel(Group group)
        {
            this.GroupName = group.Name;
            this.GroupId = group.Id;
        }

        public bool Selected { get; set; }

        [Required]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }


    public class SelectGroupRolesViewModel
    {
        public SelectGroupRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }


        // Enable initialization with an instance of ApplicationUser:
        public SelectGroupRolesViewModel(Group group, string UserName)
            : this()
        {
            this.GroupId = group.Id;
            this.GroupName = group.Name;
            this.Ma_TTP = group.Ma_TTP;

            var Db = new ApplicationDbContext();

            // Add all available roles to the list of EditorViewModels:
            var allRoles = new List<ApplicationRole>();
            if (UserName.ToUpper() == "ADMIN")
                allRoles = Db.Roles.ToList();
            else
                allRoles = Db.Roles.Where(r => r.Ma_TTP == this.Ma_TTP).ToList();

            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (var groupRole in group.Roles)
            {
                var checkGroupRole =
                    this.Roles.Find(r => r.RoleName == groupRole.Role.Name);
                checkGroupRole.Selected = true;
            }
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public string Ma_TTP { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }


    public class UserPermissionsViewModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(
            new RoleStore<ApplicationRole>(new ApplicationDbContext()));
        private readonly UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
           new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public UserPermissionsViewModel()
        {
            this.Roles = new List<RoleViewModel>();
        }


        // Enable initialization with an instance of ApplicationUser:
        public UserPermissionsViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;

            var roles = _userManager.GetRoles(user.Id);
            foreach (var role in roles)
            {
                var appRole = (ApplicationRole)_roleManager.FindByName(role);
                var pvm = new RoleViewModel(appRole);
                this.Roles.Add(pvm);
            }
        }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }

    // test remove cac user thuoc group
    public class SelectUserEditorViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Selected { get; set; }
    }
    public class SelectUsersInGroupViewModel
    {
        public SelectUsersInGroupViewModel()
        {
            this.UserInGroup = new List<SelectUserEditorViewModel>();
        }

        public SelectUsersInGroupViewModel(Group group)
            : this()
        {
            var Db = new ApplicationDbContext();

            this.GroupId = group.Id;
            this.GroupName = group.Name;

            // user in group
            var userInGroup = Db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));
            foreach (var u in userInGroup)
            {
                var editorViewModel = new SelectUserEditorViewModel()
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                };
                UserInGroup.Add(editorViewModel);
            }
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<SelectUserEditorViewModel> UserInGroup { get; set; }
        public IEnumerable<string> GroupInSelectedIds()
        {
            // Return an Enumerable containing the Id's of the selected user:
            return (from p in this.UserInGroup where p.Selected select p.UserId).ToList();
        }
    }

    public class SelectUsersOutGroupViewModel
    {
        public SelectUsersOutGroupViewModel()
        {
            this.UserOutGroup = new List<SelectUserEditorViewModel>();
        }

        public SelectUsersOutGroupViewModel(Group group, string MaTTP)
            : this()
        {
            var Db = new ApplicationDbContext();

            this.GroupId = group.Id;
            this.GroupName = group.Name;

            // user out group
            var userOutGroup=new List<ApplicationUser>();
            if(!String.IsNullOrEmpty(MaTTP))
                userOutGroup = Db.Users.Where(u => u.Groups.All(g => g.GroupId != group.Id) &&  u.MA_TINHTP == MaTTP).ToList();
            else
                userOutGroup = Db.Users.Where(u => u.Groups.All(g => g.GroupId != group.Id)).ToList();
            //UserOutGroup.AddRange(userOutGroup);
            foreach (var u in userOutGroup)
            {
                var editorViewModel = new SelectUserEditorViewModel()
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Selected = false
                };
                UserOutGroup.Add(editorViewModel);
            }
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<SelectUserEditorViewModel> UserOutGroup { get; set; }
        public IEnumerable<string> GroupOutSelectedIds()
        {
            // Return an Enumerable containing the Id's of the selected user:
            return (from p in this.UserOutGroup where p.Selected select p.UserId).ToList();
        }
    }
    #endregion
}
