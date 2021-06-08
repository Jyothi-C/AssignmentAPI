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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
              {
                  new Claim(ClaimTypes.Name,user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
              };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                Response response = new Response()
                {
                    Tokens = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    // User = user.UserName,
                    LoggerMessage = "Login successfully"

                };
                return Ok(response);
            }
            return Unauthorized();
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