using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESD.Data;
using ESD.Models;
using Microsoft.AspNetCore.Identity;
using ESD.Services.EmailService;

namespace ESD.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public CommentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
              return _context.Comments != null ? 
                          View(await _context.Comments.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Comments'  is null.");
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            int? CurrentIdea = TempData["CurrentIdea"] as int?;
            TempData["CurrentIdea"] = CurrentIdea;

            return View(comment);
        }

        public IActionResult Create(int? CurrentIdea)
        {
            if (CurrentIdea == null)
            {
                return NotFound();
            }

            TempData["CurrentIdea"] = CurrentIdea;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,IdeaId,UserId")] Comment comment)
        {
            //if (ModelState.IsValid)
            {
                int? CurrentIdea = TempData["CurrentIdea"] as int?;
                int ideaID = (int)CurrentIdea;
                var user = await _userManager.GetUserAsync(HttpContext.User);
                comment.IdeaId = ideaID;
                comment.UserId = user.Id;
                _context.Add(comment);
                await _context.SaveChangesAsync();

                SendEmail(CurrentIdea);

                TempData["CurrentIdea"] = CurrentIdea;

                return RedirectToAction("Details", "Ideas", new { id = CurrentIdea });
            }
            //return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,IdeaId,UserId")] Comment comment)
        {
            int? CurrentIdea = TempData["CurrentIdea"] as int?;
            TempData["CurrentIdea"] = CurrentIdea;

            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Ideas", new { id = CurrentIdea });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
          return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public void SendEmail(int? id)
        {
            var currentIdea = _context.Ideas.FirstOrDefault(c => c.Id == id);
            var ideaUser = _context.Users.FirstOrDefault(c => c.Id == currentIdea.UserId);
            EmailDio emailDio = new EmailDio();

            emailDio.To = ideaUser.Email;
            emailDio.Subject = "Comment notification.";
            emailDio.Body = "There is someone comment on your ideas.";

            _emailService.SendEmail(emailDio);
        }
    }
}
