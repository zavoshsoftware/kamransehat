using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ViewModels;

namespace KamranSehat.Controllers
{
    public class PaymentController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();
        // GET: Payment
        [Route("paymentrequest")]

        public ActionResult Index(decimal amount, Guid orderId)
        {
            string val = ZarinPalRedirect(amount, orderId);
            if (val == "false")
                return View();
            else
                return Redirect(val);
        }
        private String MerchantId = "9a52912e-92a3-11e9-85fa-000c29344814";


        public string ZarinPalRedirect(decimal amount, Guid orderId)
        {
            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();

            String CallbackURL = WebConfigurationManager.AppSettings["callBackUrl"];

            long Amount = Convert.ToInt64(amount);

            String description = "خرید از وب سایت دکتر کامران صحت";

            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantId, Amount, CallbackURL, description);

           // zarinpal.DisableSandboxMode();
           zarinpal.DisableSandboxMode();
            try
            {
                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                {
                    InsertToAuthority(orderId, res.Authority, amount);
                    db.SaveChanges();
                    return res.PaymentURL;
                }
                else
                    return "false";

            }
            catch (Exception e)
            {
                return "zarrin";
            }
        }
        public void InsertToAuthority(Guid orderId, string authority, decimal amount)
        {
            ZarinpallAuthority zarinpallAuthority = new ZarinpallAuthority()
            {
                OrderId = orderId,
                Authority = authority,
                Amount = amount,
                CreationDate = DateTime.Now,
                IsDeleted = false,
                IsActive = true
            };

            db.ZarinpallAuthorities.Add(zarinpallAuthority);
            db.SaveChanges();
        }
        public void Deletecookie()
        {
            HttpCookie currentUserCookie = Request.Cookies["basket"];
            Response.Cookies.Remove("basket");
            Response.Cookies.Remove("basket");
            if (currentUserCookie != null)
            {
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
            }
        }

        [Route("callback")]
        public ActionResult CallBack(string authority, string status)
        {
            String Status = status;
            CallBackViewModel callBack = new CallBackViewModel();
            callBack.MenuProductGroups = baseViewModel.GetMenu();
            callBack.MenuServiceGroups = baseViewModel.GetMenuServices();
            callBack.FooterRecentBlog = baseViewModel.GetFooterRecentBlog();
            Deletecookie();

            if (Status != "OK")
            {
                callBack.IsSuccess = false;
            }

            else
            {
                try
                {
                    var zarinpal = ZarinPal.ZarinPal.Get();
                  // zarinpal.DisableSandboxMode();
                    zarinpal.DisableSandboxMode();
                    String Authority = authority;
                    long Amount = GetAmountByAuthority(Authority);

                    var verificationRequest = new ZarinPal.PaymentVerification(MerchantId, Amount, Authority);
                    var verificationResponse = zarinpal.InvokePaymentVerification(verificationRequest);
                    if (verificationResponse.Status == 100)
                    {
                        Order order = GetOrderByAuthority(authority);
                        if (order != null)
                        {
                            order.IsPaid = true;
                            order.PaymentDate = DateTime.Now;
                            order.SaleReferenceId = verificationResponse.RefID;
                            order.OrderStatusId = db.OrderStatuses.FirstOrDefault(current => current.Code == 1).Id;

                            db.SaveChanges();
                            List<Product> products= RetrunProducts(order.Id);
                            callBack.IsSuccess = true;
                            callBack.OrderCode = order.Code.ToString();
                            callBack.RefrenceId = verificationResponse.RefID;
                            callBack.Products = products;
                            callBack.CreationDate = order.CreationDateStr;

                            String onlineConsult = WebConfigurationManager.AppSettings["online-consult"];
                            string productType = products.FirstOrDefault()?.ProductGroup.UrlParam;
                            callBack.ProductType = productType;


                            if (productType == onlineConsult)
                            {
                                UpdateQuestionOrderId(order.Id);
                            }
                            CreateSucessMessage("", order.User.CellNum, order.User.FullName);
                        }
                        else
                        {
                            callBack.IsSuccess = false;
                            callBack.RefrenceId = "سفارش پیدا نشد";
                            callBack.Products = null;
                        }
                    }
                    else
                    {
                        callBack.IsSuccess = false;
                        callBack.RefrenceId = verificationResponse.Status.ToString();
                        callBack.Products = null;
                    }
                }
                catch (Exception e)
                {
                    callBack.IsSuccess = false;
                    callBack.RefrenceId = "خطا سیستمی. لطفا با پشتیبانی سایت تماس بگیرید";
                    callBack.Products = null;
                }
            }
            return View(callBack);

        }

        public void UpdateQuestionOrderId(Guid orderId)
        {
            Question question = db.Questions.FirstOrDefault(current => current.OrderId == orderId);
            if (question != null)
            {
                question.IsActive = true;
                question.LastModifiedDate = DateTime.Now;
                db.SaveChanges();
            }
        }
        public long GetAmountByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
                db.ZarinpallAuthorities.Where(current => current.Authority == authority).FirstOrDefault();

            if (zarinpallAuthority != null)
                return Convert.ToInt64(zarinpallAuthority.Amount);

            return 0;
        }


        public Order GetOrderByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
              db.ZarinpallAuthorities.Where(current => current.Authority == authority).FirstOrDefault();

            if (zarinpallAuthority != null)
                return zarinpallAuthority.Order;

            else
                return null;
        }
        public List<Product> RetrunProducts(Guid orderId)
        {
            List<Product> products = new List<Product>();
            List<OrderDetail> details = db.OrderDetails.Where(current => current.OrderId == orderId && current.IsDeleted == false && current.IsActive == true).ToList();
            foreach (OrderDetail item in details)
            {
                products.Add(item.Product);
            }
            return products;
        }



        public void CreateSucessMessage(string url, string cellNumber, string fullName)
        {
            Sms sms = new Sms();
            string nextLine = "\n";

            string message = fullName + " عزیز" + nextLine + "با تشکر از خرید شما از وب سایت کامران صحت";
            sms.SendCommonSms(cellNumber, message);
        }
    }
}