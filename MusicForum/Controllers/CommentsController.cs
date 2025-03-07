using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicForum.Data;
using MusicForum.Models;

namespace MusicForum.Controllers
{
    //AC for logged in users
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly MusicForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(MusicForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments
        // GET: Comments/Details/5
        // GET: Comments/Create
        // GET: Comments/Edit/5
        // POST: Comments/Edit/5
        // GET: Comments/Delete/5
        // POST: Comments/Delete/5
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["DiscussionId"] = id;

            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,DiscussionId")] Comment comment)
        {
            // Ensure the Discussion exists before proceeding
            if (!_context.Discussion.Any(d => d.DiscussionId == comment.DiscussionId))
            {
                ModelState.AddModelError("", "Discussion does not exist.");
                return View(comment);
            }

            // Ensure the user is logged in
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "User must be logged in to comment.");
                return View(comment);
            }

            // Assign logged-in user's ID
            comment.ApplicationUserId = userId;
            comment.CreateDate = DateTime.Now; // Set timestamp

            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect to Home/GetDiscussion/id
                return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
            }

            ViewData["DiscussionId"] = new SelectList(_context.Discussion, "DiscussionId", "DiscussionId", comment.DiscussionId);
            return View(comment);
        }

    }
}
