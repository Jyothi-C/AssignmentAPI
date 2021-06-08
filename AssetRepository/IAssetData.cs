using System.Collections.Generic;
using AssetManagementSystem.Models;

namespace AssetManagementSystem.Models
{
    public interface IAssetData
    {
        List<AssetModel> GetAllAssets();
        List<AssetModel> SearchAsset(string search);
        AssetModel AddAsset(AssetModel asset);
        AssetModel UpdateAsset(AssetModel asset);
        AssetModel Details(TypeOfAsset assetType, int id);
        void DeleteAsset(TypeOfAsset assetType, int id);

        void RequestAsset(AssetAssign assetAssign);

        bool IsAssetAssigned(string typeOfAsset, int assetId);

        void UnAssignAsset(AssetAssign assetAssign);

        void AssignAsset(AssetAssign assetAssign);

        List<AssetAssign> GetAllRequest();

    }
}