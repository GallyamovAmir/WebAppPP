using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppPP.Models;
using System.Diagnostics;

namespace WebAppPP.Controllers
{
    public class SignLogController : Controller
    {
        private readonly ILogger<SignLogController> _logger;

        public SignLogController(ILogger<SignLogController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Log(string returnUrl)
        {
            var form = Request.Form;
            // если phone и/или пароль не установлены, посылаем статусный код ошибки 400
            if (!form.ContainsKey("phone") || !form.ContainsKey("password"))
                return BadRequest("Phone и/или пароль не установлены");

            string phone = form["phone"];
            string password = form["password"];

            VarkaDbContext db = new VarkaDbContext();
            // находим пользователя 
            var user = db.Users.FirstOrDefault(p => p.Phone == phone && p.Password == password && p.RoleId == 1);
            // если пользователь не найден, отправляем статусный код 401
            if (user is null)
            {
                return Unauthorized("Недостаточно прав");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Phone) };
            // создаем объект ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction(returnUrl ?? "Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
