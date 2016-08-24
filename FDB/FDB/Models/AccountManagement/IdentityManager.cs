using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FDB.Models
{
    public class IdentityManager
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private readonly RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(
            new RoleStore<ApplicationRole>(new ApplicationDbContext()));

        private readonly UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));

            return rm.RoleExists(name);
        }


        //public bool CreateRole(string name)
        //{
        //    var rm = new RoleManager<IdentityRole>(
        //        new RoleStore<IdentityRole>(new ApplicationDbContext()));
        //    var idResult = rm.Create(new IdentityRole(name));

        //    return idResult.Succeeded;
        //}
        public IdentityResult CreateRole(string name, string description = "", string Ma_TTP = "")
        {
            // Swap ApplicationRole for IdentityRole:
            return _roleManager.Create(new ApplicationRole(name, description, Ma_TTP));
        }

        //public bool CreateUser(ApplicationUser user, string password)
        //{
        //    var um = new UserManager<ApplicationUser>(
        //        new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    var idResult = um.Create(user, password);

        //    return idResult.Succeeded;
        //}

        public IdentityResult CreateUser(ApplicationUser user, string password)
        {
            return _userManager.Create(user, password);
        }


        //public bool AddUserToRole(string userId, string roleName)
        //{
        //    var um = new UserManager<ApplicationUser>(
        //        new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    var idResult = um.AddToRole(userId, roleName);

        //    return idResult.Succeeded;
        //}

        public IdentityResult AddUserToRole(string userId, string roleName)
        {
            return _userManager.AddToRole(userId, roleName);
        }

        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //var user = um.FindById(userId);

            //var currentRoles = new List<IdentityUserRole>();
            //currentRoles.AddRange(user.Roles);
            //foreach (var role in currentRoles)
            //{
            //    um.RemoveFromRole(userId, role.Role.Name);                
            //}

            var currentRoles = new List<string>();
            currentRoles.AddRange(um.GetRoles(userId));

            foreach (var role in currentRoles)
            {
                um.RemoveFromRoles(userId, role);
            }


        }

        #region "Group"
        public void CreateGroup(string groupName, string Ma_TTP)
        {
            if (this.GroupNameExists(groupName))
            {
                throw new System.Exception("A group by that name already exists in the database. Please choose another name.");
            }

            var newGroup = new Group(groupName, Ma_TTP);
            _db.Groups.Add(newGroup);
            _db.SaveChanges();
        }


        public bool GroupNameExists(string groupName)
        {
            var g = _db.Groups.Where(gr => gr.Name == groupName);
            if (g.Count() > 0)
            {
                return true;
            }
            return false;
        }

        // xoa toan bo user cua group (bao gom ca cac role cua user do)
        public void ClearUserGroups(string userId)
        {
            this.ClearUserRoles(userId);
            var user = _db.Users.Find(userId);
            user.Groups.Clear();
            _db.SaveChanges();
        }

        // xoa 1 user khoi group
        public void RemoveUserGroups(string userId, int groupId)
        {
            // truong hop role cua group nay cung thuoc group khac cua user thi ko xoa
            // lay cac groupId khac cua user
            var lstGroupsOfUser = GetNotCurrentGroupOfUser(groupId, userId);

            // xoa cac role va cac group cua user
            this.ClearUserGroups(userId);

            // add lai vao group ko bi xoa
            foreach (var gID in lstGroupsOfUser)
            {
                this.AddUserToGroup(userId, gID);
            }
        }

        public List<int> GetNotCurrentGroupOfUser(int groupId, string userId)
        {
            var lstGroupsOfUser = new List<int>();
            var user = _db.Users.Find(userId);

            // lay tat ca cac group khac cua user
            var groupOfUser = user.Groups.Where(g => g.GroupId != groupId);

            foreach (var g in groupOfUser)
            {
                lstGroupsOfUser.Add(g.GroupId);
            }

            return lstGroupsOfUser;
        }
        public void AddUserToGroup(string userId, int GroupId)
        {
            var group = _db.Groups.Find(GroupId);
            var user = _db.Users.Find(userId);

            var userGroup = new ApplicationUserGroup()
            {
                Group = group,
                GroupId = group.Id,
                User = user,
                UserId = user.Id
            };

            foreach (var role in group.Roles)
            {
                _userManager.AddToRole(userId, role.Role.Name);
            }
            user.Groups.Add(userGroup);
            _db.SaveChanges();
        }

        public void RemoveFromRole(string userId, string roleName)
        {
            _userManager.RemoveFromRole(userId, roleName);
        }

        public void DeleteRole(string roleId)
        {
            IQueryable<ApplicationUser> roleUsers = _db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
            ApplicationRole role = _db.Roles.Find(roleId);

            foreach (ApplicationUser user in roleUsers)
            {
                RemoveFromRole(user.Id, role.Name);
            }
            _db.Roles.Remove(role);
            _db.SaveChanges();
        }
        public void ClearGroupRoles(int groupId)
        {
            Group group = _db.Groups.Find(groupId);
            var groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id)).ToList();

            foreach (ApplicationRoleGroup role in group.Roles)
            {
                string currentRoleId = role.RoleId;
                foreach (ApplicationUser user in groupUsers)
                {
                    // Is the user a member of any other groups with this role?
                    int groupsWithRole = user.Groups.Count(g => g.Group.Roles.Any(r => r.RoleId == currentRoleId));

                    // This will be 1 if the current group is the only one:
                    if (groupsWithRole == 1)
                    {
                        RemoveFromRole(user.Id, role.Role.Name);
                    }
                }
            }
            group.Roles.Clear();
            _db.SaveChanges();

            //var group = _db.Groups.Find(groupId);
            //var groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));

            //foreach (var role in group.Roles)
            //{
            //    var currentRoleId = role.RoleId;
            //    foreach (var user in groupUsers)
            //    {
            //        // Is the user a member of any other groups with this role?
            //        var groupsWithRole = user.Groups.Where(g => g.Group.Roles.Any(r => r.RoleId == currentRoleId)).Count();

            //        if (groupsWithRole == 1)
            //        {
            //            this.RemoveFromRole(user.Id, role.Role.Name);
            //        }
            //    }
            //}
            //group.Roles.Clear();
            //_db.SaveChanges();
        }


        public void AddRoleToGroup(int groupId, string roleName)
        {
            var group = _db.Groups.Find(groupId);
            var role = _db.Roles.First(r => r.Name == roleName);
            var newgroupRole = new ApplicationRoleGroup()
            {
                GroupId = group.Id,
                Group = group,
                RoleId = role.Id,
                Role = (ApplicationRole)role
            };

            group.Roles.Add(newgroupRole);
            _db.SaveChanges();

            // Add all of the users in this group to the new role:
            var groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));
            foreach (var user in groupUsers)
            {
                if (!(_userManager.IsInRole(user.Id, roleName)))
                {
                    this.AddUserToRole(user.Id, role.Name);
                }
            }
        }


        public void DeleteGroup(int groupId)
        {
            var group = _db.Groups.Find(groupId);

            // Clear the roles from the group:
            this.ClearGroupRoles(groupId);
            _db.Groups.Remove(group);
            _db.SaveChanges();
        }


        #endregion
    }

    [Serializable]
    public class GroupExistsException : Exception
    {
        public GroupExistsException()
        {
        }

        public GroupExistsException(string message)
            : base(message)
        {
        }

        public GroupExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected GroupExistsException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}