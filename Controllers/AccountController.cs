using Microsoft.AspNetCore.Mvc;
using Music_Club.Models;
using Music_Club.Repository;
using System.Security.Cryptography;
using System.Text;

namespace Music_Club.Controllers
{
    public class AccountController : Controller
    {
        IRepository<Users> _repository;


        public AccountController(IRepository<Users> context)
        {
            _repository = context;
        }

        public ActionResult Login()

        {
            HttpContext.Session.Clear();
            return View();
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel logon)
        {

            if (ModelState.IsValid)
            {
                if (_repository.GetList().Result.Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                var users = _repository.GetList().Result.Where(a => a.Login == logon.Login).FirstOrDefault();
                if (users == null)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                //var user = users.First();
                string? salt = users.Salt;

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (users.Password != hash.ToString())
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                HttpContext.Session.SetString("UserID", users.Id.ToString());
                HttpContext.Session.SetString("FirstName", users.FirstName);
                HttpContext.Session.SetString("LastName", users.LastName);
                return View("~/Views/MusicClips/Create.cshtml");
            }
            return View(logon);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistratModel reg)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users();
                user.FirstName = reg.FirstName;
                user.LastName = reg.LastName;
                user.Login = reg.Login;

                byte[] saltbuf = new byte[16];

                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Password = hash.ToString();
                user.Salt = salt;
                _repository.Create(user);
                _repository.Save();
                return RedirectToAction("Login");
            }

            return View(reg);
        }
    }
}
