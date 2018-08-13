using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DojoSecrets.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DojoSecrets.Controllers
{
    public class HomeController : Controller
    {
        private DojoSecretsContext _context;
		public HomeController(DojoSecretsContext context)
		{
			_context = context;
		}

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("secrets")]
        public IActionResult RecentSecrets()
        {
            List<Secret> recentsecs = _context.secrets.Include(s => s.SecretLikes).OrderByDescending(s => s.CreatedAt).Take(5).ToList();
            return View("RecentSecrets", recentsecs);
        }

        [HttpGet("mostpopular")]
        public IActionResult MostPopular()
        {
            List<Secret> popularsecrets = _context.secrets.Include(s => s.SecretLikes).OrderByDescending(s => s.SecretLikes.Count).ToList();
            return View("MostPopular", popularsecrets);
        }

        [HttpPost]
        [Route("registerprocess")]
        public IActionResult RegisterProcess(UserValidator newuser)
        {
            if(ModelState.IsValid)
            {
                User DBUser = _context.users.SingleOrDefault(u=>u.Email == newuser.Email);
                if(DBUser != null)
                {
                    ViewBag.Error = "Email already exists in Database";
                    return View("Index");
                }
                PasswordHasher<UserValidator> Hasher = new PasswordHasher<UserValidator>();
                newuser.Password = Hasher.HashPassword(newuser, newuser.Password);
                User this_user = new User
                {
                    FirstName = newuser.FirstName,
                    LastName = newuser.LastName,
                    Email = newuser.Email,
                    Password = newuser.Password,
                };
                _context.Add(this_user);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", this_user.UserId);
                HttpContext.Session.SetString("UserFirstName", this_user.FirstName);
                return RedirectToAction("RecentSecrets");
            }
            else{
                return View("Index");
            }
        }

        [HttpPost]
        [Route("loginprocess")]
        public IActionResult LoginProcess(string LEmail, string LPassword)
        {
            User myUser = _context.users.SingleOrDefault(u => u.Email == LEmail);
            if(myUser != null && LPassword != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(myUser, myUser.Password, LPassword))
                {
                    HttpContext.Session.SetInt32("UserId", myUser.UserId);
                    HttpContext.Session.SetString("UserFirstName", myUser.FirstName);
                    return RedirectToAction("RecentSecrets");
                }
                else
                {
                    ViewBag.BadPass = "Password Incorrect.";
                    return View("Index");
                }
            }
            else{
                if(myUser == null)
                {
                    ViewBag.NoUser = "Could not locate user with that email.";
                }
                if(LPassword == null)
                {
                    ViewBag.PassNull = "You must enter a password.";
                }
                return View("Index");
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("addsecret")]
        public IActionResult AddSecret(string new_secret)
        {
            Secret this_secret = new Secret
            {
                Content = new_secret,
                UserId = (int) HttpContext.Session.GetInt32("UserId")
            };
            _context.Add(this_secret);
            _context.SaveChanges();
            
            return RedirectToAction("RecentSecrets");
        }

        [HttpPost]
        [Route("deletesecret")]
        public IActionResult DeleteSecret(int secretid)
        {
            Secret this_secret = _context.secrets.SingleOrDefault(s => s.SecretId == secretid);
            List<Like> this_secret_likes = _context.likes.Where(l => l.SecretId == secretid).ToList();
            foreach(var like in this_secret_likes){
                _context.likes.Remove(like);
            }
            _context.secrets.Remove(this_secret);
            _context.SaveChanges();
            return RedirectToAction("RecentSecrets");
        }

        [HttpPost]
        [Route("like")]
        public IActionResult LikeSecret(int secretid)
        {
            Like new_like = new Like
            {
                UserId= (int) HttpContext.Session.GetInt32("UserId"),
                SecretId = secretid
            };
            _context.Add(new_like);
            _context.SaveChanges();
            return RedirectToAction("RecentSecrets");
        }

        [HttpPost]
        [Route("unlike")]
        public IActionResult Un_LikeSecret(int secretid)
        {
            Like this_like = _context.likes.SingleOrDefault(a => a.UserId == HttpContext.Session.GetInt32("UserId") && a.SecretId == secretid);
            _context.likes.Remove(this_like);
            _context.SaveChanges();
            return RedirectToAction("RecentSecrets");
        }








        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}












namespace DojoSecrets
{
	public static class SessionExtensions
	{
		public static void SetObjectAsJson(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
		public static T GetObjectFromJson<T>(this ISession session, string key)
		{
			string value = session.GetString(key);
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}
}
