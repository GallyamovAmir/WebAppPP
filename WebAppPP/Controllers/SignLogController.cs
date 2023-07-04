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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Log(string phone, string password)
        {
            ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("Login");
            var form = Request.Form;
            // если phone и/или пароль не установлены, посылаем статусный код ошибки 400
            if (!form.ContainsKey("phone") || !form.ContainsKey("password"))
                return BadRequest("Phone и/или пароль не установлены");

             phone = form["phone"];
             password = form["password"];

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

            logger.LogInformation($"{DateTime.Now}: User with phone: {phone} and password: {password} has been authorized");
            return RedirectToAction("Index", "Home");

           
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
