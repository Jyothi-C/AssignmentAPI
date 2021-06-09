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
            var assetList = _asset.GetAllAssets();
            try
            {
                if (assetList == null)
                {
                    return NotFound("No Assets available,List empty");
                }
                return Ok(assetList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("SearchAsset/{search}")]
        [Authorize(Roles = "admin,user")]
        public IActionResult SearchAsset(string search)
        {
            var asset = _asset.SearchAsset(search);
            try
            {
                if (asset != null)
                {
                    return Ok(asset);
                }
                return NotFound("The asset Not available");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("AddAsset")]
        [Authorize(Roles = "admin")]
        public IActionResult AddAsset(AssetModel asset)
        {
            var asset1 = _asset.AddAsset(asset);
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(asset1);
                }
                return NotFound("Some details not provided");
            }
            catch (Exception)
            {
                return BadRequest();
            }
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
            try
            {
                if (Modelstate.IsValid)
                {
                    _asset.UpdateAsset(asset);
                    return Ok("Updated Successfully");
                }
                return NotFound("No data");
            }
            catch (Exception)
            {
                return BadRequest();
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

            try
            {
                if (ModelState.IsValid)
                {
                    _asset.AssignAsset(assetAssign);
                    return Ok("Asset succesfully assigned");
                }
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Route("Admin/UnAssignAsset")]
        [Authorize(Roles = "admin")]
        public IActionResult UnAssignAsset([FromBody] AssetAssign assetAssign)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _asset.UnAssignAsset(assetAssign);
                    return Ok("Ässet succesfully unassigned");
                }
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("Admin/GetAllRequest")]
        [Authorize(Roles = "admin")]
        public IActionResult AllRequest()
        {
            var requestList = _asset.GetAllRequest();
            try
            {
                if (requestList != null)
                {
                    return Ok(requestList);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }
    }
}