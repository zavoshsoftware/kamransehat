using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Eshop.Helpers;
using Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models;
using ViewModels;

namespace KamranSehat.Controllers
{
    public class QuestionsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

    [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.User).Where(q => q.IsDeleted == false).OrderByDescending(q => q.CreationDate);
            return View(questions.ToList());
        }


    [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", question.UserId);
            return View(question);
        }

        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Subject,Body,Response,IsActive,CreationDate,CreateUserId,LastModifiedDate,IsDeleted,DeletionDate,DeleteUserId,Description")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.IsDeleted = false;
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", question.UserId);
            return View(question);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Question question = db.Questions.Find(id);
            question.IsDeleted = true;
            question.DeletionDate = DateTime.Now;

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



        [AllowAnonymous]
        public ActionResult Activate(int id,Guid questionCode)
        {

            ActivateUserViewModel businessViewModel = new ActivateUserViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
                UserCode = id,
                QuestionId = questionCode
            };
            ViewBag.PackageId = new SelectList(db.Packages.Where(current => current.IsActive), "Id", "Title");

            return View(businessViewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Activate(ActivateUserViewModel activeCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = db.Users.FirstOrDefault(current => current.Code == activeCode.UserCode && current.Password == activeCode.ActivateCode);

                    if (user == null)
                        ModelState.AddModelError("invalidCode", "کد وارد شده صحیح نمی باشد");
                    else
                    {
                        user.IsActive = true;
                        user.LastModifiedDate = DateTime.Now;

                        Order order = CreateOrder(user.Id);

                        if (order != null)
                        {
                            UpdateQuestionOrderId(order.Id, activeCode.QuestionId);
                            db.SaveChanges();

                            string returnUrl = "/paymentrequest?amount=" + order.TotalAmount + "&orderId=" + order.Id;
                            return Redirect(returnUrl);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("catchError", "مشکلی در پردازش اطلاعات به وجود آمده است. لطفا دوباره وارد سایت شوید.");

            }
            ActivateUserViewModel businessViewModel = new ActivateUserViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog()
            };

            return View(businessViewModel);
        }

        public void UpdateQuestionOrderId(Guid orderId, Guid questionId)
        {
            Question question = db.Questions.FirstOrDefault(current => current.Id == questionId);

            if (question != null)
            {
                question.OrderId = orderId;
                question.LastModifiedDate=DateTime.Now;
                
            }
        }

        public Order CreateOrder(Guid userId)
        {
            String onlineConsult = WebConfigurationManager.AppSettings["online-consult"];

            Product product = db.Products.FirstOrDefault(current => current.ProductGroup.UrlParam == onlineConsult);
            Order order = new Order();

            if (product != null)
            {
                order.Id = Guid.NewGuid();
                order.IsDeleted = false;
                order.IsActive = true;
                order.CreationDate = DateTime.Now;
                order.TotalAmount = product.Amount;
                order.UserId = userId;
                order.IsPaid = false;
                order.OrderStatusId = db.OrderStatuses.Where(current => current.Code == 0).FirstOrDefault().Id;
                order.Code = codeCreator.ReturnOrderCode();

                db.Orders.Add(order);


                OrderDetail orderDetail = new OrderDetail();

                orderDetail.Id = Guid.NewGuid();
                orderDetail.IsActive = true;
                orderDetail.IsDeleted = false;
                orderDetail.CreationDate = DateTime.Now;
                orderDetail.ProductId = product.Id;
                orderDetail.OrderId = order.Id;
                orderDetail.Quantity = 1;
                orderDetail.Price = product.Amount;
                orderDetail.Amount = product.Amount;

                db.OrderDetails.Add(orderDetail);

            }
            return order;

        }

        #region Create




        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();
        [AllowAnonymous]
        public ActionResult Create()
        {

            QuestionCreateViewModel questionViewModel = new QuestionCreateViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenu(),
                MenuServiceGroups = baseViewModel.GetMenuServices(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog()
            };

            return View(questionViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(QuestionCreateViewModel question)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    bool isValidMobile = Regex.IsMatch(question.CellNumber, @"(^(09|9)[0123456789][0123456789]\d{7}$)|(^(09|9)[0123456789][0123456789]\d{7}$)", RegexOptions.IgnoreCase);

                    if (!isValidMobile)
                        ModelState.AddModelError("invalidMobile", "شماره موبایل وارد شده صحیح نمی باشد");

                    else
                    {
                        User user = InsertUser(question.FullName, question.CellNumber, question.Email);
                        db.SaveChanges();
                        Guid questionId = InsertQuestion(user.Id, question.Subject, question.Body);

                        db.SaveChanges();

                        return RedirectToAction("Activate", new { id = user.Code, questionCode = questionId });
                    }

                }

                question.MenuProductGroups = baseViewModel.GetMenu();
                question.MenuServiceGroups = baseViewModel.GetMenuServices();
                question.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();
                ViewBag.PackageId = new SelectList(db.Packages.Where(current => current.IsActive), "Id", "Title");


                return View(question);

            }
            catch (Exception e)
            {
                QuestionCreateViewModel questionViewModel = new QuestionCreateViewModel()
                {
                    MenuProductGroups = baseViewModel.GetMenu(),
                    MenuServiceGroups = baseViewModel.GetMenuServices(),
                    FooterRecentBlog = baseViewModel.GetFooterRecentBlog()
                };

                return View(questionViewModel);
            }
        }
        Sms sms = new Sms();
        CodeCreator codeCreator = new CodeCreator();
        public User InsertUser(string fullname, string mobile, string email)
        {
            User oUser = db.Users.FirstOrDefault(current => current.CellNum == mobile && current.IsDeleted == false);

            if (oUser == null)
            {
                User user = new User();
                user.Id = Guid.NewGuid();
                user.FullName = fullname;
                user.CellNum = mobile;
                user.Password = RandomCode().ToString();
                user.IsActive = false;
                user.IsDeleted = false;
                user.CreationDate = DateTime.Now;
                user.RoleId = db.Roles.Where(current => current.Name.ToLower() == "customer").FirstOrDefault().Id;
                user.Code = codeCreator.ReturnUserCode();
                user.Email = email;

                db.Users.Add(user);

                sms.SendSms(mobile, user.Password);

                return user;

            }
            else
            {
                oUser.Password = RandomCode().ToString();
                oUser.LastModifiedDate = DateTime.Now;
                oUser.FullName = fullname;
                oUser.Email = email;

                if (oUser.Code == null)
                    oUser.Code = codeCreator.ReturnUserCode();

              sms.SendSms(mobile, oUser.Password);
                return oUser;
            }

            db.SaveChanges();

        }

        public int RandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000).ToString("D5");
            return Convert.ToInt32(r);
        }

        public Guid InsertQuestion(Guid userId, string subject, string body)
        {
            Question question = new Question()
            {
                UserId = userId,
                Subject = subject,
                Body = body,
                CreationDate = DateTime.Now,
                IsDeleted = false,
                IsActive = false,
                Id = Guid.NewGuid()
            };

            db.Questions.Add(question);

            return question.Id;
        }
        #endregion
    }
}
