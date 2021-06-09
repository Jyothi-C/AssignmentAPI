using System;
using System.Linq;
using AssetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AssetManagementSystem.Controllers
{
    public class AssetManagementController : Controller
    {
        private HttpClient client;
        public static string token;
        public AssetManagementController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.UseCookies = true;
            clientHandler.CookieContainer = new CookieContainer();
            client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri("http://localhost:8001");
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        [HttpGet]
        public async Task<IActionResult> SearchAsset(string search)
        {
            List<AssetModel> assetList = new List<AssetModel>();
            IEnumerable<AssetModel> assetModels = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/api/AssetManagement/GetAllAssets");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("Bearer {0}", token));
                var responseTask = await client.GetAsync("GetAllAssets");
                if (!string.IsNullOrEmpty(search))
                {
                    responseTask = await client.GetAsync(string.Format("SearchAsset/{0}", search));
                }
                try
                {
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var readTask = await responseTask.Content.ReadAsAsync<IList<AssetModel>>();
                        assetModels = readTask;
                    }
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }
            }
            foreach (var Asset in assetModels)
            {
                assetList.Add(Asset);
            }
            return View(assetList);
        }
        [HttpGet]
        public async Task<IActionResult> Details(TypeOfAsset assetType, int id)
        {
            AssetModel assetModel = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/api/AssetManagement/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("{0}", token));
                var responseTask = await client.GetAsync(String.Format("Details/{0}/{1}", assetType, id));
                var result = responseTask.Result;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<AssetModel>();
                        assetModel = readTask;
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("SearchAsset");
                }
            }
            return View(assetModel);
        }
        [HttpGet]
        public ActionResult AddAsset()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddAsset(AssetModel assetCreate)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/api/AssetManagement/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("{0}", token));
                var postTask = await client.PostAsJsonAsync<AssetModel>("AddAsset", assetCreate);
                var result = postTask;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("SearchAsset");
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("AddAsset");
                }
            }
            return View(assetCreate);
        }
        [HttpGet]
        public async Task<ActionResult> UpdateAsset(TypeOfAsset assetType, int id)
        {
            AssetModel assetModel = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/api/AssetManagement/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("{0}", token));
                var responseTask = await client.GetAsync(String.Format("UpdateAsset/{0}/{1}", assetType, id));
                var result = responseTask;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<AssetModel>();
                        assetModel = readTask;
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("UpdateAsset");
                }
            }
            return View(assetModel);
        }
        [HttpPost]
        public ActionResult UpdateAsset(AssetModel assetEdit)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/api/AssetManagement/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("{0}", token));
                var putTask = await client.PutAsJsonAsync<AssetModel>("UpdateAsset", assetEdit);
                var result = putTask;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("SearchAsset");
                    }
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }
            }
            return View(assetEdit);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteAsset(TypeOfAsset assetType, int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/api/AssetManagement/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("{0}", token));
                var deleteTask = await client.DeleteAsync(String.Format("DeleteAsset/{0}/{1}", assetType, id));
                var result = deleteTask;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("SearchAsset");
                    }
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }

            }
            return RedirectToAction("SearchAsset");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            client.BaseAddress = new Uri("http://localhost:5001/Employee/");
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string loginData = JsonConvert.SerializeObject(new
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password

            });
            StringContent content = new StringContent(loginData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage loginResponse = client.PostAsync("Register", content).Result;
            string loginResult = loginResponse.Content.ReadAsStringAsync().Result;
            loginResult = JsonConvert.DeserializeObject<string>(loginResult);
            try
            {
                if (loginResult == "Registered Successfully")
                {
                    return RedirectToAction("Login");

                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            return RedirectToAction("Register");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            client.BaseAddress = new Uri("http://localhost:5001/Employee/");
            string loginData = JsonConvert.SerializeObject(
                new
                {
                    UserName = model.UserName,
                    Password = model.Password

                });
            StringContent content = new StringContent(loginData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage loginResponse = client.PostAsync("Login", content).Result;
            string loginResult = loginResponse.Content.ReadAsStringAsync().Result;
            ResponseModel response = JsonConvert.DeserializeObject<ResponseModel>(loginResult);
            token = response.Tokens.ToString();
            try
            {
                if (response.LoggerMessage == "Login successfully")
                {
                    return RedirectToAction("SearchAsset");
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> RequestAsset()
        {
            IEnumerable<AssetModel> assetModels = null;
            List<AssetModel> assetList = new List<AssetModel>();

            client.BaseAddress = new Uri("https://localhost:5001/api/AssetManagement/");
            var responseTask = await client.GetAsync("GetAllAssets");
            var result = responseTask;
            try
            {
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<AssetModel>>();
                    assetModels = readTask;
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            foreach (var Asset in assetModels)
            {
                assetList.Add(Asset);
            }
            return View(assetList);
        }
        [HttpPost]
        public async Task<IActionResult> RequestAsset(AssetModel assetModels)
        {
            AssetAssign assetStatus = new AssetAssign()
            {
                assetId = assetModels.Id,
                userEmail = assetModels.userEmail,
                AssetType = assetModels.AssetType.ToString(),
                status = "pending",
            };

            client.BaseAddress = new Uri("http://localhost:5001/Employee/");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("{0}", token));
            var postTask = await client.PostAsJsonAsync<AssetAssign>("RequestAsset", assetStatus);
            var result = postTask;
            try
            {
                if (result.IsSuccessStatusCode)
                {
                    string message = "request successfully submitted";
                    ViewBag.Message = message;
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

            return RedirectToAction("SearchAsset");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRequest()
        {
            IEnumerable<AssetAssign> assetModels = null;
            List<AssetAssign> assetList = new List<AssetAssign>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/api/AssetManagement/Admin/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("{0}", token));
                var responseTask = await client.GetAsync("GetAllRequest");
                var result = responseTask;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<AssetAssign>>();
                        assetModels = readTask;
                    }
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }
            }
            foreach (var Asset in assetModels)
            {
                assetList.Add(Asset);
            }
            return View(assetList);
        }

        [HttpPost]
        public async Task<IActionResult> AssignAsset(AssetAssign assetAssign)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/api/AssetManagement/AssignAsset");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("{0}", token));
                var putTask = await client.PutAsJsonAsync<AssetAssign>("AssignAsset", assetAssign);
                var result = putTask;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        string message = "assigned successfully submitted";
                        ViewBag.Message = message;
                    }
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }
            }
            return RedirectToAction("SearchAsset");
        }
        [HttpPost]
        public async Task<IActionResult> UnAssignAsset(AssetAssign assetAssign)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/api/AssetManagement/UnAssignAsset");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", String.Format("{0}", token));
                var putTask = await client.PutAsJsonAsync<AssetAssign>("UnAssignAsset", assetAssign);
                var result = putTask;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        string message = "unassigned successfully submitted";
                        ViewBag.Message = message;
                    }
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }
                return RedirectToAction("SearchAsset");
            }
        }
    }
}
