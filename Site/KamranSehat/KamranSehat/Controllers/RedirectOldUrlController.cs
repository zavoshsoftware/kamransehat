using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KamranSehat.Controllers
{
    public class RedirectOldUrlController : Controller
    {
        [Route("مشاوره.html")]
        [AllowAnonymous]
        public ActionResult Route1()
        {
            return RedirectPermanent("/consult-request");
        }


        [Route("فروشگاه-محصولات-آموزشی-دکتر-کامران-صحت.html")]
        [AllowAnonymous]
        public ActionResult Route2()
        {
            return RedirectPermanent("/product/video");
        }

        [Route("فروشگاه-محصولات-آموزشی-دکتر-کامران-صحت/16-آموزش-تغییر-در-کسب-و-کار.html")]
        [AllowAnonymous]
        public ActionResult Route3()
        {
            return RedirectPermanent("/product/video/1002");
        }


        [Route("فروشگاه-محصولات-آموزشی-دکتر-کامران-صحت/7-موفقیت-نامحدود-در-کسب-و-کار.html")]
        [AllowAnonymous]
        public ActionResult Route4()
        {
            return RedirectPermanent("/product/video/1004");
        }


        [Route("فروشگاه-محصولات-آموزشی-دکتر-کامران-صحت/12-فروش-موفق-و-فروشنده-موفق.html")]
        [AllowAnonymous]
        public ActionResult Route5()
        {
            return RedirectPermanent("/product/video/1005");
        }

        [Route("فروشگاه-محصولات-آموزشی-دکتر-کامران-صحت/14-نگارش-نامه-بازاریابی-و-فروش.html")]
        [AllowAnonymous]
        public ActionResult Route6()
        {
            return RedirectPermanent("/product/video/1007");
        }

        [Route("فروشگاه-محصولات-آموزشی-دکتر-کامران-صحت/9-کارآفرینی-و-ایجاد-یک-کسب-و-کار-موفق.html")]
        [AllowAnonymous]
        public ActionResult Route7()
        {
            return RedirectPermanent("/product/video/1001");
        }

        [Route("فروشگاه-محصولات-آموزشی-دکتر-کامران-صحت/10-هدف-گذاری-در-کسب-و-کار.html")]
        [AllowAnonymous]
        public ActionResult Route8()
        {
            return RedirectPermanent("/product/video/1000");
        }
        [Route("دیسک-disc-کامران-صحت-دکتر-صحت-مشاور-بازاریابی-و-فروش-سالن-زیبایی-تبلیغات.html")]
        [AllowAnonymous]
        public ActionResult Route9()
        {
            return RedirectPermanent("/blog/communication/disc");
        }

        [Route("دوره-های-آموزشی-کلینیک-دکتر-کامران-صحت/82-تخفیف-آبی-blue-offer-دکتر-کامران-صحت.html")]
        [AllowAnonymous]
        public ActionResult Route10()
        {
            return RedirectPermanent("/product/video");
        }

        [Route("دوره-های-آموزشی-کلینیک-دکتر-کامران-صحت/102-بلو-واو-blue-wow-کلینیک-دکتر-صحت.html")]
        [AllowAnonymous]
        public ActionResult Route11()
        {
            return RedirectPermanent("/product/video");
        }
    }
}