using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrdersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

       
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.City).Where(o=>o.IsDeleted==false).OrderByDescending(o=>o.CreationDate).Include(o => o.DiscountCode).Where(o=>o.IsDeleted==false).OrderByDescending(o=>o.CreationDate).Include(o => o.OrderStatus).Where(o=>o.IsDeleted==false).OrderByDescending(o=>o.CreationDate).Include(o => o.User).Where(o=>o.IsDeleted==false).OrderByDescending(o=>o.CreationDate);
            return View(orders.ToList());
        }

     
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title");
            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code");
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,UserId,Address,TotalAmount,OrderStatusId,CityId,SaleReferenceId,IsPaid,DiscountCodeId,ShippingAmount,SubTotal,DiscountAmount,DeliverFullName,DeliverCellNumber,PostalCode,PaymentDate,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] Order order)
        {
            if (ModelState.IsValid)
            {
				order.IsDeleted=false;
				order.CreationDate= DateTime.Now; 
                order.Id = Guid.NewGuid();
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", order.CityId);
            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code", order.DiscountCodeId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Title", order.OrderStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", order.CityId);
            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code", order.DiscountCodeId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Title", order.OrderStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,UserId,Address,TotalAmount,OrderStatusId,CityId,SaleReferenceId,IsPaid,DiscountCodeId,ShippingAmount,SubTotal,DiscountAmount,DeliverFullName,DeliverCellNumber,PostalCode,PaymentDate,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] Order order)
        {
            if (ModelState.IsValid)
            {
				order.IsDeleted=false;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", order.CityId);
            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code", order.DiscountCodeId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Title", order.OrderStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.Orders.Find(id);
			order.IsDeleted=true;
			order.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private BaseViewModelHelper baseHelper = new BaseViewModelHelper();

        [AllowAnonymous]
        public ActionResult List()
        {
            OrderListViewModel orderList=new OrderListViewModel()
            {
                FooterRecentBlog = baseHelper.GetFooterRecentBlog(),
                MenuServiceGroups = baseHelper.GetMenuServices(),
                MenuProductGroups = baseHelper.GetMenu()
            };

            bool isLogin = HttpContext.User.Identity.IsAuthenticated;

            if (!isLogin)
            {
                return Redirect("/account/login?ReturnUrl=/orders/list");
            }

            User user = db.Users.FirstOrDefault(current => current.CellNum == HttpContext.User.Identity.Name);

            if (user != null)
            {
                List<Order> orders = db.Orders.Where(current => current.UserId == user.Id && current.IsDeleted == false&&current.IsPaid)
                    .ToList();

                if (orders.Any())
                {
                    List<OrderItemViewModel> orderItems=new List<OrderItemViewModel>();

                    foreach (Order order in orders)
                    {
                        orderItems.Add(new OrderItemViewModel()
                        {
                            Id = order.Id,
                            Code = order.Code.ToString(),
                            OrderDate = order.CreationDateStr,
                            Status = order.OrderStatus.Title,
                            Total = order.TotalAmount.ToString("N0")
                            
                        });
                    }

                    orderList.Orders = orderItems;
                    orderList.HasOrder = true;
                }
                else
                    orderList.HasOrder = false;
            }

            return View(orderList);
        }

        [AllowAnonymous]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int code = Convert.ToInt32(id);

            Order order = db.Orders.FirstOrDefault(current => current.Code == code);

            if (order == null)
            {
                return HttpNotFound();
            }

            OrderDetailViewModel orderDetail=new OrderDetailViewModel()
            {
                FooterRecentBlog = baseHelper.GetFooterRecentBlog(),
                MenuServiceGroups = baseHelper.GetMenuServices(),
                MenuProductGroups = baseHelper.GetMenu()
            };

            orderDetail.Order = order;
            orderDetail.OrderDetails = db.OrderDetails.Where(current => current.OrderId == order.Id).ToList();

            return View(orderDetail);
        }


        [AllowAnonymous]
        public ActionResult AdminDetails(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.FirstOrDefault(current => current.Id == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            OrderDetailViewModel orderDetail = new OrderDetailViewModel()
            {
                FooterRecentBlog = baseHelper.GetFooterRecentBlog(),
                MenuServiceGroups = baseHelper.GetMenuServices(),
                MenuProductGroups = baseHelper.GetMenu()
            };

            orderDetail.Order = order;
            orderDetail.OrderDetails = db.OrderDetails.Where(current => current.OrderId == order.Id).ToList();

            return View(orderDetail);
        }

    }
}
