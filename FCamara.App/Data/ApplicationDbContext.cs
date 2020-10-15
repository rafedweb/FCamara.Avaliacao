using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FCamara.App.Models;

namespace FCamara.App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FCamara.App.Models.FuncionarioViewModel> FuncionarioViewModel { get; set; }
        public DbSet<FCamara.App.Models.DependenteViewModel> DependenteViewModel { get; set; }
        public DbSet<FCamara.App.Models.EnderecoViewModel> EnderecoViewModel { get; set; }
    }
}
