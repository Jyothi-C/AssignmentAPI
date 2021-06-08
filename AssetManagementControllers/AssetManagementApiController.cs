using Microsoft.AspNetCore.Mvc;
using AssetManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AssetManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetManagementController : ControllerBase
    {
        private readonly IAssetData _asset;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AssetManagementController(IAssetData asset, RoleManager<IdentityRole> roleManager)
        {
            _asset = asset;
            _roleManager = roleManager;
        }
        [HttpGet]
        [Route("GetAllAssets")]
        [Authorize(Roles = "admin,user")]
        public IActionResult GetAllAssets()
        {
            return Ok(_asset.GetAllAssets());
        }
        [HttpGet]
        [Route("SearchAsset/{search}")]
        [Authorize(Roles = "admin,user")]
        public IActionResult SearchAsset(string search)
        {
            var asset = _asset.SearchAsset(search);
            if (asset != null)
            {
                return Ok(asset);
            }
            else
            {
                return NotFound("The asset Not available");
            }
        }
        [HttpPost]
        [Route("AddAsset")]
        [Authorize(Roles = "admin")]
        public IActionResult AddAsset(AssetModel asset)
        {
            var asset1 = _asset.AddAsset(asset);
            return Ok(asset1);
        }
        [HttpGet]
        [Route("Details/{assetType}/{id}")]
        [Authorize(Roles = UserRoleModel.Admin)]
        public IActionResult Details(TypeOfAsset assetType, int id)
        {
            var asset = _asset.Details(assetType, id);
            return Ok(asset);
        }
        [HttpPut]
        [Route("UpdateAsset")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateAsset(AssetModel asset)
        {
            if (asset != null)
            {
                var asset1 = _asset.UpdateAsset(asset);
                return Ok(asset1);
            }
            else
            {
                return NotFound("No data");
            }
        }
        [HttpDelete]
        [Route("DeleteAsset/{assetType}/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteAsset(TypeOfAsset assetType, int id)
        {
            _asset.DeleteAsset(assetType, id);
            return Ok("Deleted Successfully");
        }
        [HttpPut]
        [Route("Admin/AssignAsset")]
        [Authorize(Roles = "admin")]
        public IActionResult AssignAsset([FromBody] AssetAssign assetAssign)
        {
            _asset.AssignAsset(assetAssign);
            return Ok("Asset succesfully assigned");
        }
        [HttpPut]
        [Route("Admin/UnAssignAsset")]
        [Authorize(Roles = "admin")]
        public IActionResult UnAssignAsset([FromBody] AssetAssign assetAssign)
        {
            _asset.UnAssignAsset(assetAssign);
            return Ok("Ässet succesfully unassigned");
        }
        [HttpGet]
        [Route("Admin/GetAllRequest")]
        [Authorize(Roles = "admin")]
        public IActionResult AllRequest()
        {
            return Ok(_asset.GetAllRequest());

        }
    }
}