using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace InternalOrdersContext {
    public class InternalOrderDbContext : DbContext {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Approver> Approvers { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }

    public class Order {
        [Key] public int OrderId { get; set; }
        [Required] [MaxLength(128)] public string Name { get; set; }
        [MaxLength(512)] public string Description { get; set; }
        [Required] public DateTime Date { get; set; }
        public bool CustomerFunded { get; set; }
        [MaxLength(5)] public string CapexNumber { get; set; } 
        public CurrencyType Currency { get; set; }
        public StatusType Status { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Approver> Approvers { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
    public class Item {
        [Key] public int ItemId { get; set; }
        [MaxLength(7)] public string RekordIndex { get; set; }
        [MaxLength(64)] public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDate { get; set; }
        [Required] public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        [NotMapped] public string DeliveryDateString {
            get { return DeliveryDate.ToShortDateString(); }
            set {
                try {
                    DeliveryDate = DateTime.Parse(value);
                } catch (FormatException ex) {
                    //Not implemented
                }
            }
        }
    }
    public class Approver {
        [Key] public int ApproverId { get; set; }
        [Required] public int Queue { get; set; }
        [Required] public StatusType Status { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
    }
    public class Attachment {
        [Key] public int AttachmentId { get; set; }
        [Required] [MaxLength(256)] public string FileName { get; set; }
        [Required] [MaxLength(256)] public string Description { get; set; }
        [Required] public byte[] FileData { get; set; }
        [Required] public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
    public class User {
        [Key] public int UserId { get; set; }
        [Required] [MaxLength(64)] public string Name { get; set; }
        [Required] public string PasswordHash { get; set; }
        public DepartmentType Department { get; set; }
        public bool IsAdmin { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        [NotMapped]
        public string Password {
            set { PasswordHash = PasswordHasher.Hash(value); }
        }
    }
    public class Role {
        [Key] public RoleType RoleId { get; set; }
        [Required] public int UserId { get; set; }
        public virtual User User { get; set; }
    }

    public enum StatusType {
        Canceled = 0,
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
    public enum CurrencyType {
        PLN = 1,
        EUR = 2,
        USD = 3
    }
    public enum DepartmentType {
        None = 0,
        Engeeniering = 1,
        Quality = 2,
        Production = 3,
        Logistic = 4,
        Maintenance = 5,
        EHS = 6,
        HR = 7,
        Purchasing = 8,
        Accounting = 9,
        IT = 10
    }
    public enum RoleType {
        Engeeniering_Manager = 1,
        Quality_Manager = 2,
        Production_Manager = 3,
        Logistic_Manager = 4,
        Maintenance_Manager = 5,
        EHS_Manager = 6,
        HR_Manager = 7,
        Purchasing_Manager = 8,
        Accounting_Manager = 9,
        IT_Manager = 10,
        Plant_Manager = 100,
        Plant_Controller = 101,
        Division_Controller = 102
    }
}
