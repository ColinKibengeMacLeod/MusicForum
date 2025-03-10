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
    public class DiscussionsController : Controller
    {
        private readonly MusicForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DiscussionsController(MusicForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var discussions = await _context.Discussion.Where(m => m.ApplicationUserId == userId).ToListAsync();

            return View(discussions);
        }

        // GET: Discussions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);  // Get the ID of the logged-in user

            // Find the discussion by ID and also ensure it belongs to the current user
            var discussion = await _context.Discussion
                .Where(m => m.ApplicationUserId == userId)
                .Include(d => d.ApplicationUser)
                .ToListAsync();

            if (discussion == null)
            {
                return Forbid();  // If the discussion doesn't exist or doesn't belong to the logged-in user, deny access
            }

            return View(discussion);  // If everything is okay, return the discussion to the view
        }


        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,ImageFile")] Discussion discussion)
        {
            discussion.CreateDate = DateTime.Now;
            // Decode Content to prevent double encoding
            discussion.Content = System.Net.WebUtility.HtmlDecode(discussion.Content);

            // Set user ID for the logged-in user
            var userId = _userManager.GetUserId(User);
            discussion.ApplicationUserId = userId;  // Explicitly set it here

            if (discussion.ImageFile != null && discussion.ImageFile.Length > 0)
            {
                // Only assign a filename if an image is uploaded
                discussion.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile.FileName);

                // Save the uploaded image file
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", discussion.ImageFilename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await discussion.ImageFile.CopyToAsync(fileStream);
                }
            }
            else
            {
                // Ensure ImageFilename remains null if no file is uploaded
                discussion.ImageFilename = null;
            }

            if (ModelState.IsValid)
            {
                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(discussion);
        }




        // GET: Discussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);  // Get the ID of the logged-in user

            // Find the discussion by ID and ensure it belongs to the logged-in user
            var discussion = await _context.Discussion
                .Include(m => m.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id && m.ApplicationUserId == userId);

            if (discussion == null)
            {
                return Forbid();  // If the discussion doesn't exist or doesn't belong to the logged-in user, deny access
            }

            return View(discussion);  // If everything is okay, return the discussion to the view
        }


        // POST: Discussions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFilename,CreateDate")] Discussion discussion)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            // Explicitly set ApplicationUserId to prevent tampering
            var userId = _userManager.GetUserId(User);
            discussion.ApplicationUserId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }


        // GET: Discussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);  // Get the ID of the logged-in user

            // Find the discussion by ID and ensure it belongs to the logged-in user
            var discussion = await _context.Discussion
                .FirstOrDefaultAsync(m => m.DiscussionId == id && m.ApplicationUserId == userId);

            if (discussion == null)
            {
                return Forbid();  // If the discussion doesn't exist or doesn't belong to the logged-in user, deny access
            }

            return View(discussion);  // If everything is okay, return the discussion to the view
        }


        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var discussion = await _context.Discussion
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            // Authorization check: Ensure the user owns the discussion
            if (discussion.ApplicationUserId != userId)
            {
                return Forbid(); // Prevent unauthorized deletion
            }

            _context.Discussion.Remove(discussion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.DiscussionId == id);
        }
    }
}
