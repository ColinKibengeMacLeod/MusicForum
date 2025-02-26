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
            //Get ALL Discussions from DB
            var discussions = await _context.Discussion.Include(d => d.Comments).ToListAsync();


            return View(discussions);
        }

        //Discussion Details Page
        public async Task<IActionResult> GetDiscussion(int id)
        {
            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            // Ensure that ImageFilename is set to null if it doesn't exist or is empty
            if (discussion != null && string.IsNullOrEmpty(discussion.ImageFilename))
            {
                discussion.ImageFilename = null;
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
