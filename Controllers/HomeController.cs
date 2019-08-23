using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using csharp_belt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using csharp_belt.Models;

namespace csharp_belt.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
     
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User NewUser)
        {
            if(ModelState.IsValid)
            {
                    // If a User exists with provided email
                if(dbContext.Users.Any(u => u.Email == NewUser.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    
                    // You may consider returning to the View at this point
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                    dbContext.Add(NewUser);
                    dbContext.SaveChanges();
                    NewUser.CreatedAt = DateTime.Now;
                    NewUser.UpdatedAt = DateTime.Now;
                    dbContext.SaveChanges();
                    
                    HttpContext.Session.SetInt32("id",dbContext.Users.Last().UserId);

                    return RedirectToAction("Dashboard");
                }   
            }
            else
            {
                return View("Index");
            }
        }



        [HttpPost("logging")]
        public IActionResult Logging(LogInUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                
                // Initialize hasher object
                var hasher = new PasswordHasher<LogInUser>();
                
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    return View("Index");
                }

                User currentUser = dbContext.Users
                    .FirstOrDefault(user => user.Email == userSubmission.LoginEmail); 

                HttpContext.Session.SetInt32("id", currentUser.UserId);

                int? id = HttpContext.Session.GetInt32("id");

                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {

            int? UserId = HttpContext.Session.GetInt32("id");
            if(UserId == null)
            {
                return View("Index");
            }
            else
            {
                ViewBag.UserId = UserId;
                User user = dbContext.Users
                    .FirstOrDefault(u => u.UserId == UserId);
                ViewBag.UserName = user.FirstName;

                var allActivities = dbContext.Activities
                    .Include(w => w.Creator)
                    .Include(w => w.AllParticipants)
                       .ThenInclude(g => g.Rsvped)
                    .OrderBy(w => w.Date)
                    .ToList();

                DateTime currDate = DateTime.Now;

                // Looping through the current list of activities, if the activity date is expired delete it from the list
                foreach(var act in allActivities)
                {
                    DateTime actDate = Convert.ToDateTime(act.Date);

                    if (actDate < currDate)
                    {
                        Delete(act.ActivityId);
                    }
                }

                return View("Dashboard", allActivities);
            }
        }

        [HttpGet("create/activity")]
        public IActionResult CreateActivity()
        {
            int? UserId = HttpContext.Session.GetInt32("id");
            var User = dbContext.Users
                .FirstOrDefault(u => u.UserId == UserId);

            ViewBag.UserName = User.FirstName;

            return View();
        }

        [HttpPost("add")]
        public IActionResult AddActivity(Activity newAct)
        {
            int? UserId = HttpContext.Session.GetInt32("id");
            if(ModelState.IsValid)
            {
                dbContext.Add(newAct);
                newAct.UserId = (int)UserId;
                dbContext.SaveChanges();

                return RedirectToAction("ActivityDetails", new{actId = newAct.ActivityId});
            }
            return View("CreateActivity");
        }

        [HttpGet("details/activity/{actId}")]
        public IActionResult ActivityDetails(int actId)
        {
            int? UserId = HttpContext.Session.GetInt32("id");

            ViewBag.UserId = UserId;

            User user = dbContext.Users
                .FirstOrDefault(u => u.UserId == UserId);
            ViewBag.UserName = user.FirstName;

            var theActivity = dbContext.Activities
                .Include(a => a.AllParticipants)
                    .ThenInclude(p => p.Rsvped)
                .Include(a => a.Creator)
                .FirstOrDefault(a => a.ActivityId == actId);

            return View(theActivity);
        }

        [HttpGet("delete/{actId}")]
        public IActionResult Delete(int actId)
        {
            Activity actToDelete = dbContext.Activities
                .FirstOrDefault(a => a.ActivityId == actId);
            
            dbContext.Remove(actToDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("join/{actId}")]
        public IActionResult Join(int actId)
        {
            int? UserId = HttpContext.Session.GetInt32("id");

            if(ModelState.IsValid)
            {

                Rsvp newRsvp = new Rsvp()
                {
                    UserId = (int)UserId,
                    ActivityId = actId
                };
                dbContext.Rsvps.Add(newRsvp);
                dbContext.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Dashboard");
            }
        }

        [HttpGet("flake/{actId}")]
        public IActionResult Flake(int actId)
        {
            int? UserId = HttpContext.Session.GetInt32("id");
            Rsvp actToFlake = dbContext.Rsvps.FirstOrDefault(r => r.UserId == UserId && r.ActivityId == actId);
            
            if(actToFlake != null)
            {
                dbContext.Remove(actToFlake);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Dashboard");

        }



    }
}
