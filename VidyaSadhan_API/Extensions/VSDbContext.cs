using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Extensions
{
    public class VSDbContext:IdentityDbContext<IdentityUser>
    {
        public VSDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
