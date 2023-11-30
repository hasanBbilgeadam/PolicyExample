using ApiConsume.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace ApiConsume.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            HttpClient client = new HttpClient();


            StringContent content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");


            var response = await client.PostAsync("https://localhost:7027/api/Account/Login",content);


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var data =  await response.Content.ReadAsStringAsync();
               //var myToken = JsonSerializer.Deserialize<string>(data);

                 Console.WriteLine("token : "+ data);
                 Console.WriteLine("---------------------------");
                 Console.WriteLine("token : "+ data);
                 Console.WriteLine("---------------------------");
                 Console.WriteLine("token : "+ data);
                 Console.WriteLine("---------------------------");

                return Redirect("/");


                //aldığın token'ı cookie sakla

            }

            return View();
          
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
