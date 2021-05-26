using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Models
{
    public enum TypeOfAsset
    {
        Book = 1,
        Software,
        Hardware
    };

    public class AssetModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Prompt = "Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "Year of publish")]
        public string DateOfPublish { get; set; }
        public TypeOfAsset AssetType { get; set; }

        [RequiredIf("AssetType", (int)TypeOfAsset.Book, ErrorMessage = "Provide Author name")]
        public string Author { get; set; }
        [RequiredIf("AssetType", (int)TypeOfAsset.Book, ErrorMessage = "Enter type of Book")]
        public string Genre { get; set; }

        [RequiredIf("AssetType", (int)TypeOfAsset.Software, ErrorMessage = "Provide Company name")]
        public string SoftwareCompany { get; set; }
        [RequiredIf("AssetType", (int)TypeOfAsset.Book, ErrorMessage = "Enter OsPlatform")]
        public string OsPlatform { get; set; }
        [RequiredIf("AssetType", (int)TypeOfAsset.Book, ErrorMessage = "Enter type of software")]
        public string Type { get; set; }

        [RequiredIf("AssetType", (int)TypeOfAsset.Hardware, ErrorMessage = "Provide company name")]
        public string HardwareCompany { get; set; }
        [RequiredIf("AssetType", (int)TypeOfAsset.Book, ErrorMessage = "Enter supported device")]
        public string SupportedDevice { get; set; }
    }
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateOfPublish { get; set; }

    }
    public class BookAsset : Asset
    {
        public string Author { get; set; }
        public string Genre { get; set; }

    }
    public class SoftwareAsset : Asset
    {
        public string SoftwareCompany { get; set; }
        public string OsPlatform { get; set; }
        public string Type { get; set; }

    }
    public class HardwareAsset : Asset
    {
        public string HardwareCompany { get; set; }
        public string SupportedDevice { get; set; }

    }
}