using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;
using System.Text.RegularExpressions;
using System.Security.Claims;
using Eshop.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace KamranSehat.Controllers
{
    public class CartController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private BaseViewModelHelper baseViewModel = new BaseViewModelHelper();
        private Sms sms = new Sms();


        public BaseViewModelHelper BaseViewModel { get => baseViewModel; set => baseViewModel = value; }

        [Route("addtobasket/{code}")]
        public ActionResult AddToBasket(string code)
        {
            if (code != null)
                SetCookie(code);

            return RedirectToAction("Basket");
        }



        [Route("basket")]
        public ActionResult Basket()
        {

            return View(GetBasketInfo());
        }

        public BasketViewModel GetBasketInfo()
        {
            decimal total = 0;
            string[] basketItems = GetCookie();

            List<Product> products = new List<Product>();

            if (basketItems != null && basketItems.Length != 0)
            {
                for (int i = 0; i < basketItems.Length - 1; i++)
                {
                    string basketItemCode = basketItems[i];

                    Product product = db.Products.FirstOrDefault(current =>
                        current.IsDeleted == false && current.IsActive && current.Code == basketItemCode);

                    if (product != null)
                    {
                        products.Add(product);
                        total = total + product.Amount;
                    }
                }
            }

            decimal discount = GetDiscount();

            BasketViewModel basket = new BasketViewModel
            {
                Products = products,
                SubTotal = total.ToString("n0") + " تومان",
                Total = (total - discount),
                DiscountAmount = discount.ToString("n0") + " تومان",
                MenuProductGroups = baseViewModel.GetMenu(),
                FooterRecentBlog = baseViewModel.GetFooterRecentBlog(),
                MenuServiceGroups = baseViewModel.GetMenuServices()
            };


            return basket;
        }

        public void SetCookie(string code)
        {
            string cookievalue = null;

            if (Request.Cookies["basket"] != null)
            {
                cookievalue = Request.Cookies["basket"].Value;

                string[] coockieItems = cookievalue.Split('/');

                if (coockieItems.All(current => current != code))
                    cookievalue = cookievalue + code + "/";
            }
            else
                cookievalue = code + "/";

            HttpContext.Response.Cookies.Set(new HttpCookie("basket")
            {
                Name = "basket",
                Value = cookievalue,
                Expires = DateTime.Now.AddDays(1)
            });
        }

        public string[] GetCookie()
        {
            if (Request.Cookies["basket"] != null)
            {
                string cookievalue = Request.Cookies["basket"].Value;

                string[] basketItems = cookievalue.Split('/');

                return basketItems;
            }

            return null;
        }

        public decimal GetDiscount()
        {
            if (Request.Cookies["discount"] != null)
            {
                try
                {
                    string cookievalue = Request.Cookies["discount"].Value;

                    string[] basketItems = cookievalue.Split('/');
                    return Convert.ToDecimal(basketItems[0]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 0;
        }

        public void SetDiscountCookie(string discountAmount, string discountCode)
        {
            HttpContext.Response.Cookies.Set(new HttpCookie("discount")
            {
                Name = "discount",
                Value = discountAmount + "/" + discountCode,
                Expires = DateTime.Now.AddDays(1)
            });
        }

        [AllowAnonymous]
        public ActionResult DiscountRequestPost(string coupon)
        {
            DiscountCode discount = db.DiscountCodes.FirstOrDefault(current => current.Code == coupon && current.IsActive);

            string result = CheckCouponValidation(discount);

            if (result != "true")
                return Json(result, JsonRequestBehavior.AllowGet);

            List<ProductInCart> productInCarts = GetProductInBasketByCoockie();
            decimal subTotal = GetSubtotal(productInCarts);

            decimal total = subTotal;

            DiscountHelper helper = new DiscountHelper();

            decimal discountAmount = helper.CalculateDiscountAmount(discount, total);

            SetDiscountCookie(discountAmount.ToString(), coupon);

            return Json("true", JsonRequestBehavior.AllowGet);
        }
        public decimal GetSubtotal(List<ProductInCart> orderDetails)
        {
            decimal subTotal = 0;

            foreach (ProductInCart orderDetail in orderDetails)
            {
                decimal amount = orderDetail.Product.Amount;

                subTotal = subTotal + (amount * orderDetail.Quantity);
            }

            return subTotal;
        }

        public List<ProductInCart> GetProductInBasketByCoockie()
        {
            List<ProductInCart> productInCarts = new List<ProductInCart>();

            string[] basketItems = GetCookie();

            for (int i = 0; i < basketItems.Length - 1; i++)
            {
                string code = basketItems[i];
                Product product =
                    db.Products.FirstOrDefault(current => current.IsDeleted == false && current.Code == code);

                productInCarts.Add(new ProductInCart()
                {
                    Product = product,
                });
            }

            return productInCarts;
        }
        [AllowAnonymous]
        public string CheckCouponValidation(DiscountCode discount)
        {
            if (discount == null)
                return "Invald";

            if (!discount.IsMultiUsing)
            {
                if (db.Orders.Any(current => current.DiscountCodeId == discount.Id))
                    return "Used";
            }

            if (discount.ExpireDate < DateTime.Today)
                return "Expired";

            return "true";
        }

        public ActionResult InsertUser(string fullname, string mobile)
        {
            bool isValidMobile = Regex.IsMatch(mobile, @"(^(09|9)[0123456789][0123456789]\d{7}$)|(^(09|9)[0123456789][0123456789]\d{7}$)", RegexOptions.IgnoreCase);

            if (!isValidMobile)
                return Json("InvalidMobile", JsonRequestBehavior.AllowGet);

            User oUser = db.Users.FirstOrDefault(current => current.CellNum == mobile && current.IsDeleted == false);
            CodeCreator codeCreator = new CodeCreator();

            if (oUser == null)
            {
                User user = new Models.User();
                user.Id = Guid.NewGuid();
                user.FullName = fullname;
                user.CellNum = mobile;
                user.Password = RandomCode().ToString();
                user.IsActive = false;
                user.IsDeleted = false;
                user.CreationDate = DateTime.Now;
                user.RoleId = db.Roles.Where(current => current.Name.ToLower() == "customer").FirstOrDefault().Id;
                user.Code = codeCreator.ReturnUserCode();

                db.Users.Add(user);

                sms.SendSms(mobile, user.Password);
                LoginUser(user);
            }
            else
            {
                oUser.Password = RandomCode().ToString();
                oUser.LastModifiedDate = DateTime.Now;
                oUser.FullName = fullname;

                sms.SendSms(mobile, oUser.Password);
                LoginUser(oUser);
            }

            db.SaveChanges();

            return Json("true", JsonRequestBehavior.AllowGet);
        }

        public int RandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000).ToString("D5");
            return Convert.ToInt32(r);
        }

        [AllowAnonymous]
        public ActionResult InsertOrder(string activationCode, string mobile)
        {
            try
            {

           
            User oUser = db.Users.FirstOrDefault(current =>
                current.CellNum == mobile && current.Password == activationCode && current.IsDeleted == false);

            if (oUser == null)
                return Json("invalidCode", JsonRequestBehavior.AllowGet);

            oUser.IsActive = true;
            oUser.LastModifiedDate = DateTime.Now;

            BasketViewModel basket = GetBasketInfo();

            if (basket.Products.Count() < 1)
                return Json("empty", JsonRequestBehavior.AllowGet);
            else
            {
                Order order = new Order();
                order.Id = Guid.NewGuid();
                order.IsDeleted = false;
                order.IsActive = true;
                order.CreationDate = DateTime.Now;
                order.TotalAmount = basket.Total;
                order.UserId = oUser.Id;
                order.IsPaid = false;
                order.OrderStatusId = db.OrderStatuses.Where(current => current.Code == 0).FirstOrDefault().Id;
                order.Code = ReturnOrderCode();

                db.Orders.Add(order);

                foreach (Product product in basket.Products)
                {
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
                db.SaveChanges();
                string returnUrl = "/paymentrequest?amount=" + basket.Total + "&orderId=" + order.Id;
                return Json(returnUrl, JsonRequestBehavior.AllowGet);
            }
            }
            catch (Exception e)
            {
                return Json("error", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult CheckLogin(string mobile, string password)
        {
            User user = db.Users.Where(current => current.CellNum == mobile && current.Password == password && current.IsDeleted == false && current.IsActive == true).FirstOrDefault();
            if (user == null)
                return Json("false", JsonRequestBehavior.AllowGet);
            else
            {
                LoginUser(user);

                return Json("true", JsonRequestBehavior.AllowGet);
            }

        }

        public void LoginUser(User user)
        {
            var ident = new ClaimsIdentity(
                      new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, user.CellNum),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name, user.CellNum),

              // optionally you could add roles if any
              new Claim(ClaimTypes.Role, user.Role.Name),

                      },
                      DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(
               new AuthenticationProperties { IsPersistent = true }, ident);
        }

        public int ReturnOrderCode()
        {
            int code = 1000;
            Order order = db.Orders.OrderByDescending(current => current.Code).FirstOrDefault();

            if (order != null)
                code = order.Code + 1;
          
            return code;

        }
    }
}