using System.Collections.Generic;
using System.Linq;
using AssetManagementSystem.Data;
using AssetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace AssetManagementSystem.Models
{
    public class SqlAssetData : IAssetData
    {
        private readonly AssetManagementAPIContext _Context;
        public SqlAssetData(AssetManagementAPIContext context)
        {
            _Context = context;
        }
        public AssetModel AddAsset(AssetModel assetCreate)
        {
            switch ((int)assetCreate.AssetType)
            {
                case 1:
                    BookAsset book = new BookAsset();
                    book.Id = assetCreate.Id;
                    book.Name = assetCreate.Name;
                    book.DateOfPublish = assetCreate.DateOfPublish;
                    book.Author = assetCreate.Author;
                    book.Genre = assetCreate.Genre;
                    _Context.BookAssets.Add(book);
                    break;
                case 2:
                    SoftwareAsset software = new SoftwareAsset();
                    software.Id = assetCreate.Id;
                    software.Name = assetCreate.Name;
                    software.DateOfPublish = assetCreate.DateOfPublish;
                    software.OsPlatform = assetCreate.OsPlatform;
                    software.Type = assetCreate.Type;
                    software.SoftwareCompany = assetCreate.SoftwareCompany;
                    _Context.SoftwareAssets.Add(software);
                    break;
                case 3:
                    HardwareAsset hardware = new HardwareAsset();
                    hardware.Id = assetCreate.Id;
                    hardware.Name = assetCreate.Name;
                    hardware.DateOfPublish = assetCreate.DateOfPublish;
                    hardware.HardwareCompany = assetCreate.HardwareCompany;
                    hardware.SupportedDevice = assetCreate.SupportedDevice;
                    _Context.HardwareAssets.Add(hardware);
                    break;
            }
            _Context.SaveChanges();
            return assetCreate;
        }
        public List<AssetModel> GetAllAssets()
        {
            List<AssetModel> assetList = new List<AssetModel>();
            assetList.AddRange(_Context.BookAssets.
               Select(x => new AssetModel
               {
                   AssetType = TypeOfAsset.Book,
                   Id = x.Id,
                   Name = x.Name,
                   Author = x.Author,
                   DateOfPublish = x.DateOfPublish,
                   Genre = x.Genre
               }));
            assetList.AddRange(_Context.SoftwareAssets.
            Select(x => new AssetModel
            {
                AssetType = TypeOfAsset.Software,
                Id = x.Id,
                Name = x.Name,
                OsPlatform = x.OsPlatform,
                DateOfPublish = x.DateOfPublish,
                Type = x.Type,
                SoftwareCompany = x.SoftwareCompany
            }));
            assetList.AddRange(_Context.HardwareAssets.
            Select(x => new AssetModel
            {
                AssetType = TypeOfAsset.Hardware,
                Id = x.Id,
                Name = x.Name,
                HardwareCompany = x.HardwareCompany,
                DateOfPublish = x.DateOfPublish,
                SupportedDevice = x.SupportedDevice
            }));
            return assetList;
        }
        public List<AssetModel> SearchAsset(string search)
        {
            List<AssetModel> assetList = new List<AssetModel>();
            assetList.AddRange(_Context.BookAssets.
               Where(b => string.IsNullOrEmpty(search) ? true : (b.Author.Contains(search) || b.Name.Contains(search) || b.DateOfPublish.Contains(search) || b.Genre.Contains(search))).Select(x => new AssetModel
               {
                   AssetType = TypeOfAsset.Book,
                   Id = x.Id,
                   Name = x.Name,
                   Author = x.Author,
                   DateOfPublish = x.DateOfPublish,
                   Genre = x.Genre
               }));
            assetList.AddRange(_Context.SoftwareAssets.
            Where(b => string.IsNullOrEmpty(search) ? true : (b.OsPlatform.Contains(search) || b.Name.Contains(search) || b.DateOfPublish.Contains(search) || b.SoftwareCompany.Contains(search) || b.Type.Contains(search))).Select(x => new AssetModel
            {
                AssetType = TypeOfAsset.Software,
                Id = x.Id,
                Name = x.Name,
                OsPlatform = x.OsPlatform,
                DateOfPublish = x.DateOfPublish,
                Type = x.Type,
                SoftwareCompany = x.SoftwareCompany
            }));
            assetList.AddRange(_Context.HardwareAssets.
            Where(b => string.IsNullOrEmpty(search) ? true : (b.Name.Contains(search) || b.DateOfPublish.Contains(search) || b.HardwareCompany.Contains(search) || b.SupportedDevice.Contains(search))).Select(x => new AssetModel
            {
                AssetType = TypeOfAsset.Hardware,
                Id = x.Id,
                Name = x.Name,
                HardwareCompany = x.HardwareCompany,
                DateOfPublish = x.DateOfPublish,
                SupportedDevice = x.SupportedDevice
            }));
            return assetList;
        }
        public AssetModel UpdateAsset(AssetModel assetEdit)
        {
            switch ((int)assetEdit.AssetType)
            {
                case 1:
                    BookAsset book = new BookAsset();
                    book.Id = assetEdit.Id;
                    book.Name = assetEdit.Name;
                    book.DateOfPublish = assetEdit.DateOfPublish;
                    book.Author = assetEdit.Author;
                    book.Genre = assetEdit.Genre;
                    var asset = _Context.BookAssets.Attach(book);
                    asset.State = EntityState.Modified;
                    break;

                case 2:
                    SoftwareAsset software = new SoftwareAsset();
                    software.Name = assetEdit.Name;
                    software.DateOfPublish = assetEdit.DateOfPublish;
                    software.OsPlatform = assetEdit.OsPlatform;
                    software.Type = assetEdit.Type;
                    var asset1 = _Context.SoftwareAssets.Attach(software);
                    asset1.State = EntityState.Modified;
                    break;

                case 3:
                    HardwareAsset hardware = new HardwareAsset();
                    hardware.Name = assetEdit.Name;
                    hardware.DateOfPublish = assetEdit.DateOfPublish;
                    hardware.HardwareCompany = assetEdit.HardwareCompany;
                    hardware.SupportedDevice = assetEdit.SupportedDevice;
                    var asset2 = _Context.HardwareAssets.Attach(hardware);
                    asset2.State = EntityState.Modified;
                    break;
            }
            _Context.SaveChanges();
            return assetEdit;
        }

        public void DeleteAsset(TypeOfAsset assetType, string name)
        {
            switch ((int)assetType)
            {
                case 1:
                    var asset = _Context.BookAssets.Find(name);
                    _Context.BookAssets.Remove(asset);
                    break;
                case 2:
                    var asset1 = _Context.BookAssets.Find(name);
                    _Context.BookAssets.Remove(asset1);
                    break;
                case 3:
                    var asset2 = _Context.BookAssets.Find(name);
                    _Context.BookAssets.Remove(asset2);
                    break;
            }
            _Context.SaveChanges();
        }
        public void RequestAsset(AssetAssign assetAssign)
        {
            _Context.AssetAssign.Add(assetAssign);
            _Context.SaveChanges();
        }
        public bool IsAssetAssigned(string typeOfAsset, int assetId)
        {
            var result = _Context.AssetAssign.Where(x => x.assetId == assetId && x.AssetType == typeOfAsset).ToList();
            if (result.Count >= 1)
            {
                foreach (var request in result)
                {
                    if (request.status == "assigned")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void AssignAsset(AssetAssign assetAssign)
        {
            var asset = _Context.AssetAssign.Attach(assetAssign);
            asset.State = EntityState.Modified;
            _Context.SaveChanges();
            var result = _Context.AssetAssign.
            Where(x => x.assetId == assetAssign.assetId && x.AssetType == assetAssign.AssetType && x.status == "pending").ToList();
            if (result.Count >= 1)
            {
                foreach (var request in result)
                {
                    request.status = "declined";
                    var assetDecline = _Context.AssetAssign.Attach(request);
                    assetDecline.State = EntityState.Modified;
                    _Context.SaveChanges();
                }
            }
        }
        public void UnAssignAsset(AssetAssign assetAssign)
        {
            var asset = _Context.AssetAssign.Attach(assetAssign);
            asset.State = EntityState.Modified;
            _Context.SaveChanges();
        }
        public List<AssetAssign> GetAllRequest()
        {
            return _Context.AssetAssign.ToList();
        }
    }
}