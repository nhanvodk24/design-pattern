using KhachSan.Models;
using KhachSan.ViewModel;
using Microsoft.AspNetCore.Mvc;
namespace KhachSan.Controllers
{
	public class UserController : Controller
	{
		private readonly QLKSContext _context;
		public UserController(QLKSContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult Login() 		
		{
			if (HttpContext.Session.GetString("username") == null)
			{
				return View();
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}
		[HttpPost]
		public IActionResult Login(string username, string password)
		{
            if (HttpContext.Session.GetString("username") == null)
			{
				var u = _context.Users.Where(x=>x.username.Equals(username) &&
				x.password.Equals(password)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("userId", u.Id.ToString());
                    HttpContext.Session.SetString("username", u.username.ToString());
                    HttpContext.Session.SetString("Role", u.role.ToString());
					if(u.role == "Admin")
					{
                        return RedirectToAction("Index", "Room", new { area = "Admin" });
                    }
					return RedirectToAction("Index", "Home");
				}
				else
				{
                    ModelState.AddModelError("", "Sai tên tài khoản hoặc mật khẩu!");
                    return View();
                }
            }
			return View();
        }
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
        public async Task<IActionResult> Register(User user)
        {
             var checkPhone = _context.Users.SingleOrDefault(x => x.username == user.username);
             if (checkPhone != null)
             {
                 ModelState.AddModelError("", "Số điện thoại đã tồn tại");
                 return View();
			 }
				user.role = "Customer";
				 _context.Users.Add(user);
				await _context.SaveChangesAsync();
				return RedirectToAction("Login", "User");
        }
        public IActionResult Logout()
		{
            HttpContext.Session.Clear();
            return RedirectToAction("Login","User");
		}
		[HttpGet]
        public IActionResult ChangePassword()
		{
			if (HttpContext.Session.GetString("username") != null)
			{
				return View();
			}
			else
			{
                return RedirectToAction("Login", "User");
            }
		}

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
			if (HttpContext.Session.GetString("username") != null)
			{
				int userId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
				var u = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
				if(u.password != currentPassword)
				{
					ModelState.AddModelError("", "Mật khẩu cũ sai!");
					return View();
				}
				if(u.password == newPassword)
				{
                    ModelState.AddModelError("", "Mật khẩu mới không được trùng với mật khẩu cũ");
                    return View();
                }
				if(newPassword != confirmPassword)
				{
                    ModelState.AddModelError("", "Xác nhận mật khẩu mới sai!");
                    return View();
                }
				if(ModelState.IsValid)
				{
                    u.password = newPassword;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
				else
				{
                    return View();
                }
			}
			else
			{
                return RedirectToAction("Login", "User");
            }
		}
		[HttpGet]
		public IActionResult UserInfo()
		{
			if (HttpContext.Session.GetString("username") != null)
			{
				string userName = HttpContext.Session.GetString("username");
				var user = _context.Users.Where(x => x.username.Equals(userName)).FirstOrDefault();
				return View(user);
			}
			else
			{
                return RedirectToAction("Login", "User");
            }
		}
    }
}
