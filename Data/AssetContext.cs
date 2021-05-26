using Microsoft.EntityFrameworkCore;
using AssetManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AssetManagementSystem.Data
{
    public class AssetManagementAPIContext : IdentityDbContext
    {
        public AssetManagementAPIContext(DbContextOptions<AssetManagementAPIContext> options)
            : base(options)
        {
        }
        public DbSet<BookAsset> BookAssets { get; set; }
        public DbSet<SoftwareAsset> SoftwareAssets { get; set; }
        public DbSet<HardwareAsset> HardwareAssets { get; set; }
        public DbSet<AssetAssign> AssetAssign { get; set; }
    }
}