﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order : BaseEntity
    {
        public Order()
        {

            OrderDetails = new List<OrderDetail>();
            ZarinpallAuthorities = new List<ZarinpallAuthority>();
            Questions = new List<Question>();
            //Payments = new List<Payment>();
            //PaymentUniqeCodes = new List<PaymentUniqeCodes>();
        }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Order))]
        [Required]
        public int Code { get; set; }

        [Display(Name = "UserId", ResourceType = typeof(Resources.Models.Order))]
        public Guid? UserId { get; set; }


        [Display(Name = "Address", ResourceType = typeof(Resources.Models.Order))]
        public string Address { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Order))]
        [Column(TypeName = "Money")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "OrderStatusId", ResourceType = typeof(Resources.Models.Order))]
        [Required]
        public Guid OrderStatusId { get; set; }

        public Guid? CityId { get; set; }

        [Display(Name = "SaleReferenceId", ResourceType = typeof(Resources.Models.Order))]
        public string SaleReferenceId { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual City City { get; set; }

        [Display(Name = "IsPaid", ResourceType = typeof(Resources.Models.Order))]
        public bool IsPaid { get; set; }
        //public virtual ICollection<Payment> Payments { get; set; }
        //public virtual ICollection<PaymentUniqeCodes> PaymentUniqeCodes { get; set; }
        public Guid? DiscountCodeId { get; set; }

        public virtual DiscountCode DiscountCode { get; set; }
        [Display(Name = "هزینه حمل")]
        public decimal? ShippingAmount { get; set; }

        [Display(Name = "هزینه جمع فاکتور")]
        public decimal? SubTotal { get; set; }


        [Display(Name = "مبلغ تخفیف")]
        public decimal? DiscountAmount { get; set; }

        public virtual List<ZarinpallAuthority> ZarinpallAuthorities { get; set; }
        internal class Configuration : EntityTypeConfiguration<Order>
        {
            public Configuration()
            {
                HasOptional(p => p.User)
                    .WithMany(j => j.Orders)
                    .HasForeignKey(p => p.UserId);

                HasRequired(p => p.OrderStatus)
                    .WithMany(j => j.Orders)
                    .HasForeignKey(p => p.OrderStatusId);

                HasOptional(p => p.City)
                    .WithMany(j => j.Orders)
                    .HasForeignKey(p => p.CityId);

                HasRequired(p => p.DiscountCode)
                    .WithMany(j => j.Orders)
                    .HasForeignKey(p => p.DiscountCodeId);
            }
        }

        public string DeliverFullName { get; set; }
        public string DeliverCellNumber { get; set; }
        public string PostalCode { get; set; }
        [Display(Name = "تاریخ پرداخت")]
        public DateTime? PaymentDate { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
