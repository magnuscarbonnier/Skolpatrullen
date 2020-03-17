using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class PasswordRecoveryController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> RecoverPassword()
        {
            var message = await GetUser();
            if (User != null)
            {
                SetFailureMessage("Du är redan inloggad");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> RecoverPassword(UserEmail vm)
        {
            var response = await APIRecoverPasswordWithEmail(vm.Email);
            SetResponseMessage(response);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("[controller]/NewPassword/{token}")]
        public async Task<IActionResult> NewPassword(string token)
        {
            var message = await GetUser();
            if (User != null)
            {
                SetFailureMessage("Du är redan inloggad");
                return RedirectToAction("Index", "Home");
            }
            return View(new NewPassword() { Token = token });
        }
        [HttpPost]
        [Route("[controller]/NewPassword")]
        public async Task<IActionResult> SetNewPassword(NewPassword newPassword)
        {
            if (newPassword.Password == newPassword.RePassword)
            {
                var recoveryResponse = await APIGetPasswordRecoveryByToken(newPassword.Token);
                if (recoveryResponse.Success)
                {
                    APIDeletePasswordRecoveryByToken(newPassword.Token);
                    var passwordResponse = await APIForceChangePassword(new ChangePasswordBody() { NewPassword = newPassword.Password, UserId = recoveryResponse.Data.UserId });
                    SetResponseMessage(passwordResponse);
                }
                else
                {
                    SetFailureMessage(recoveryResponse.FailureMessage);
                }
                return RedirectToAction("LoginPage", "Login");
            }
            else
            {
                SetFailureMessage("Lösenorden matchar inte");
                return View(newPassword);
            }
        }
    }
}