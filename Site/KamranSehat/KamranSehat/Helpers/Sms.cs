using System;
using System.Collections.Generic;
using SmsIrRestful;

namespace Helpers
{
    public class Sms
    {
        public void SendSms(string cellNumber, string code)
        {
            var token = new Token().GetToken("773e6490afdaeccca1206490", "123qwe!@#QWE");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = Convert.ToInt64(cellNumber),
                TemplateId = 16187,
                ParameterArray = new List<UltraFastParameters>()
                {
                    new UltraFastParameters()
                    {
                        Parameter = "verifyCode" , ParameterValue = code
                    }
                }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            if (ultraFastSendRespone.IsSuccessful)
            {

            }
            else
            {

            }
        }

        public void SendCommonSms(string cellNumber,string message)
        {
            var token = new Token().GetToken("773e6490afdaeccca1206490", "123qwe!@#QWE");

            SmsIrRestful.MessageSend messageSend = new SmsIrRestful.MessageSend();

            var res = messageSend.Send(token, new SmsIrRestful.MessageSendObject()
            {
                MobileNumbers = new List<string>() { cellNumber}.ToArray(),
                Messages =new List<string>(){ message}.ToArray(),
                LineNumber = "30004747475709",
                SendDateTime = null,
                CanContinueInCaseOfError = false
            });

            SmsIrRestful.Credit credit=new SmsIrRestful.Credit();
            SmsIrRestful.CreditResponse creditResponse=new SmsIrRestful.CreditResponse();

        }

        public void SendSmsForRecoverPassword(string cellNumber, string code)
        {
            var token = new Token().GetToken("773e6490afdaeccca1206490", "123qwe!@#QWE");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = Convert.ToInt64(cellNumber),
                TemplateId = 16957,
                ParameterArray = new List<UltraFastParameters>()
                {
                    new UltraFastParameters()
                    {
                        Parameter = "password" , ParameterValue = code
                    }
                }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            if (ultraFastSendRespone.IsSuccessful)
            {

            }
            else
            {

            }
        }
    }
}