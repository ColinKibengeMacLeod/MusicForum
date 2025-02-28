using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicForum.Models;

namespace MusicForum.Data
{
    public class MusicForumContext : IdentityDbContext //Changed DbContext to IdentityDbContext
    {
        public MusicForumContext (DbContextOptions<MusicForumContext> options)
            : base(options)
        {
        }

        public DbSet<MusicForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<MusicForum.Models.Comment> Comment { get; set; } = default!;
    }
}
