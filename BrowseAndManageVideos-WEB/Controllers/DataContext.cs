using Microsoft.AspNetCore.Hosting.Server;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;

using BrowseAndManageVideos_WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace BrowseAndManageVideos_WEB.Controllers
{
    public class DataContext : DbContext
    { 
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
    }
}
