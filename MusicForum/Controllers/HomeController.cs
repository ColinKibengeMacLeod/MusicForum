using System.Diagnostics;
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


        public async Task<IActionResult> GetDiscussion(int id)
        {
            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                .ThenInclude(c => c.ApplicationUser)  // Ensure the related user info for comments is included
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            // Ensure that Comments is not null
            if (discussion != null && discussion.Comments == null)
            {
                discussion.Comments = new List<Comment>();
            }

            // Ensure that ImageFilename is set to null if it doesn't exist or is empty, or set a default value
            if (discussion != null && string.IsNullOrEmpty(discussion.ImageFilename))
            {
                discussion.ImageFilename = null;  // Or use a default image URL if needed
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
