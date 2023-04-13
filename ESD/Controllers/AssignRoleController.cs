using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;
using ESD.Data;
using ESD.Models;

namespace ESD.Controllers
{
    public class AssignRoleController : Controller
    {
        //this is me, the zawqrudo
        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;
        ApplicationDbContext context;

        public AssignRoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }


        public IActionResult Index()
        {
            List<AllUserViewModel> modelList = new List<AllUserViewModel>();
            var users = userManager.Users.ToList();

            users.ForEach(u =>
            {
                AllUserViewModel model = new AllUserViewModel();
                model.User = u;
                IList<string> roleList = new List<string>();
                model.Roles = new SelectList(roleManager.Roles.ToList());
                roleList = userManager.GetRolesAsync(u).Result;
                if (roleList.Count != 0)
                {
                    model.Role = roleList[0];
                }
                modelList.Add(model);
            });


            return View(modelList);
        }

        public ActionResult Register()
        {
            ViewBag.Roles = roleManager.Roles;
            return View();
        }


        public IActionResult Edit(string userId)
        {
            UserRole ur = new UserRole();
            ur.UserID = userId;
            ur.RoleID = string.Empty;

            ViewData["User"] = context.Users.Where(u => u.Id == userId).FirstOrDefault().Email;
            ViewData["RoleId"] = new SelectList(context.Roles, "Id", "Name");
            return View(ur);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserID,RoleID")] UserRole UR)
        {
                var olduser = userManager.FindByIdAsync(UR.UserID).Result;
                IList<string> userRole = new List<string>();
                userRole = userManager.GetRolesAsync(olduser).Result;

                if (userRole.Count != 0)
                {
                    var oldrole = roleManager.FindByNameAsync(userRole[0]).Result;
                    await userManager.RemoveFromRoleAsync(olduser, oldrole.Name);
                }

                var newRole = roleManager.FindByIdAsync(UR.RoleID).Result;
                await userManager.AddToRoleAsync(olduser, newRole.Name);
                await userManager.UpdateAsync(olduser);

                return RedirectToAction(nameof(Index));
        }
    }
}
