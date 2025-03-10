using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicForum.Data;
using MusicForum.Models;

namespace MusicForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicForumContext _context;

        public HomeController(MusicForumContext context)
        {
            _context = context;
        }

        //Homepage
        public async Task<IActionResult> Index()
        {
            // Get ALL Discussions from DB and include ApplicationUser for creator's info
            var discussions = await _context.Discussion
                                            .Include(d => d.Comments)  // Includes related comments
                                            .Include(d => d.ApplicationUser)  // Includes the creator's ApplicationUser
                                            .ToListAsync();

            return View(discussions);
        }
        public async Task<IActionResult> GetDiscussion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .Include(d => d.ApplicationUser) // Include creator's user info
                .Include(d => d.Comments)
                    .ThenInclude(c => c.ApplicationUser) // Include each comment's user info
                .FirstOrDefaultAsync(m => m.DiscussionId == id); // <-- Removed userId check

            if (discussion == null)
            {
                return NotFound(); // Discussion not found at all
            }

            return View(discussion);
        }



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
