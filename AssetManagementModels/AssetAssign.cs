using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Models
{
    public class AssetAssign
    {
        public int id { get; set; }
        [Required]
        [EmailAddress]
        public string userEmail { get; set; }

        public string AssetType { get; set; }

        public int assetId { get; set; }

        public string status { get; set; }
    }
}