using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicForum.Models;

namespace MusicForum.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }
        //Homepage
        public IActionResult Index()
        {
            //Create list of Discussions
            List<Discussion> discussions = new List<Discussion>();

            //Create 3 Discussion OBJ's
            Discussion discussion1 = new Discussion()
            {
                DiscussionId = 1,
                Title = "Les Paul for Sale",
                Content = "I have a 1977 Les Paul for sale.",
                ImageFilename = "77LesPaul.jpeg",
                CreateDate = DateTime.Now,
            };

            Discussion discussion2 = new Discussion()
            {
                DiscussionId = 2,
                Title = "Looking for Humbuckers",
                Content = "Looking for a set of Humbuckers.",
                ImageFilename = "",
                CreateDate = DateTime.Now,
            };

            Discussion discussion3 = new Discussion()
            {
                DiscussionId = 3,
                Title = "Band needs manager",
                Content = "We're a band looking for a manager.",
                ImageFilename = "TheBand.jpeg",
                CreateDate = DateTime.Now,
            };

            //Add OBJ's to List
            discussions.Add(discussion1);
            discussions.Add(discussion2);
            discussions.Add(discussion3);

            return View(discussions);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //Discussion Details Page
        public IActionResult GetDiscussion(int id)
        {
            Discussion discussion = new Discussion()
            {
                //Set private key to id (discussionId)
                DiscussionId = id,
                Title = "Band needs manager",
                Content = "We're a band looking for a manager.",
                ImageFilename = "TheBand.jpeg",
                CreateDate = DateTime.Now,
            };

            return View(discussion);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
