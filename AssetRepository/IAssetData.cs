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
        void DeleteAsset(TypeOfAsset assetType, string name);

        void RequestAsset(AssetAssign assetAssign);

        bool IsAssetAssigned(string typeOfAsset, int assetId);

        void UnAssignAsset(AssetAssign assetAssign);

        void AssignAsset(AssetAssign assetAssign);

        List<AssetAssign> GetAllRequest();

    }
}