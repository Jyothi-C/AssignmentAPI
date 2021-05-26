using System.Threading.Tasks;
using AssetManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AssetManagementSystem.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAssetData _asset;
        public EmployeeController(UserManager<IdentityUser> userManager,
                                  SignInManager<IdentityUser> signInManager,
                                  IAssetData assetData,
                                  RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _asset = assetData;
            _roleManager = roleManager;
        }
        [HttpPost]
        [Route("Employee/Register")]
        public async Task<string> Register([FromBody] EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = employee.UserName,
                    Email = employee.Email
                };
                var result = await userManager.CreateAsync(user, employee.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return "Registered Successfully";
                }
                else
                {
                    return "Details are Incorrect";

                }
            }
            else
            {
                return "All Details not provided";
            }
        }
        [HttpPost]
        [Route("Employee/Login")]
        public async Task<string> Login([FromBody] LoginModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
            if (result.Succeeded)
            {
                return "Login Successfully";
            }
            else
            {
                return "Details are Incorrect";
            }
        }
        [HttpPost]
        [Route("Employee/RequestAsset")]
        [Authorize(Roles = "user")]
        public IActionResult RequestAsset([FromBody] AssetAssign assetAssign)
        {
            if (_asset.IsAssetAssigned(assetAssign.AssetType, assetAssign.assetId))
            {
                return Ok("Asset Already Assigned");
            }
            _asset.RequestAsset(assetAssign);
            return Ok("Request Submitted succesfully");
        }
        [HttpPut]
        [Route("Employee/Unassign")]
        [Authorize(Roles = "user")]
        public IActionResult Unassign([FromBody] AssetAssign assetAssign)
        {
            _asset.UnAssignAsset(assetAssign);
            return Ok("Unassign Request Submitted succesfully");
        }

    }
}