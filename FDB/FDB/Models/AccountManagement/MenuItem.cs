using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FDB.Models
{
    public class MenuItemViewModel
    {
        public MenuItemViewModel(MenuItem _menu)
        {

            this.Item_ID = _menu.Item_ID;
            this.Item_name = _menu.Item_name;
            this.Item_Action = _menu.Item_Action;
            this.Item_Controller = _menu.Item_Controller;
            this.Parent_ID = _menu.Parent_ID;
            this.isMenu = _menu.isMenu;
            this.Order_No = _menu.Order_No;
            this.ExistChild = _menu.ExistChild;
            this.isDisplay = _menu.isDisplay;
            this.Childens = new List<MenuItemViewModel>();
        }

        public MenuItemViewModel()
        {
            this.Childens = new List<MenuItemViewModel>();
           
        }
        public string Item_ID { get; set; }
        public string Item_name { get; set; }
        public string Item_Action { get; set; }
        public string Item_Controller { get; set; }
        public string Parent_ID { get; set; }
        public bool isMenu { get; set; }
        public int? Order_No { get; set; }

        public bool ExistChild { get; set; }

        public bool isDisplay { get; set; }

        public bool isCheck { get; set; }

        public List<MenuItemViewModel> Childens { get; set; }
    }
    public class MenuItem
    {
        public MenuItem()
        {
            //this.Childens = new List<MenuItem>();
        }

        [Key]
        [Required]
        public string Item_ID { get; set; }
        public string Item_name { get; set; }
        public string Item_Action { get; set; }
        public string Item_Controller { get; set; }
        public string Parent_ID { get; set; }
        public bool isMenu { get; set; }
        public int? Order_No { get; set; }

        public bool ExistChild { get; set; }

        public bool isDisplay { get; set; }

        public virtual ICollection<MenuItemRole> MenuItemRoles { get; set; }
        public virtual ICollection<MenuItemGroup> MenuItemGroups { get; set; }
        public virtual ICollection<MenuItemUser> MenuItemUsers { get; set; }

        //public List<MenuItem> Childens { get; set; }
    }

    public static class MenuItemExtensions
    {
        public static MenuItemViewModel ToTree(this List<MenuItemViewModel> list)
        {
            if (list == null) throw new ArgumentNullException("list");

            var root = list.FirstOrDefault(x => x.Parent_ID == null);
            if (root == null) throw new InvalidOperationException("root == null");

            PopulateChildren(root, list);
            return root;
        }

        //recursive method
        private static void PopulateChildren(MenuItemViewModel node, List<MenuItemViewModel> all)
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
                PopulateChildren(item, all);
        }
    }
    public class MenuItemRole
    {
        [Key, Column(Order = 0)]
        [Required]
        public string MenuItem_ID { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public string Role_ID { get; set; }

        public string Ma_TTP { get; set; }

        [ForeignKey("Role_ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual ApplicationRole Role { get; set; }

        [ForeignKey("MenuItem_ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual MenuItem MenuItem { get; set; }

    }

    public class MenuItemGroup
    {
        [Key, Column(Order = 0)]
        [Required]
        public virtual string MenuItem_ID { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public virtual int Group_ID { get; set; }

        public string Ma_TTP { get; set; }

        [ForeignKey("Group_ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual Group Group { get; set; }

        [ForeignKey("MenuItem_ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual MenuItem MenuItem { get; set; }
    }

    public class MenuItemUser
    {
        [Key, Column(Order = 0)]
        [Required]
        public virtual string MenuItem_ID { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public virtual string User_ID { get; set; }

        public string Ma_TTP { get; set; }

        [ForeignKey("User_ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("MenuItem_ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual MenuItem MenuItem { get; set; }
    }
}