using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PolicyExample.Context;
using PolicyExample.Dtos;
using PolicyExample.Helpers;
using PolicyExample.Tokens;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;

namespace PolicyExample.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class DailyController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly AppDbContext appDbcontext;
        private readonly UserManager<AppUser> userManager;

        public DailyController(IMapper mapper, AppDbContext appDbcontext, UserManager<AppUser> userManager)
        {
            this.mapper = mapper;
            this.appDbcontext = appDbcontext;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]

        public IActionResult Daily(DailyCreateDto dto)
        {

            var myEntity = mapper.Map<Daily>(dto);


            myEntity.AddedIP = HttpContext.Connection.RemoteIpAddress.ToString();
            myEntity.AppUserId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            appDbcontext.Dailies.Add(myEntity);
            appDbcontext.SaveChanges();

            return Ok();
        }

        //api/daily/ [delete]
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Daily(int id)
        {
            var daily = appDbcontext.Dailies.Find(id);
            if (daily == null)
            {
                return BadRequest();
            }
            if (daily.AppUserId == User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value)
            {
                appDbcontext.Dailies.Remove(daily);
                appDbcontext.SaveChanges();
                return Ok();
            }


            return BadRequest();
        }

        [Authorize]
        [HttpPut]
        public IActionResult Daily(DailyUpdateDto dto)
        {


            var daily = appDbcontext.Dailies.AsNoTracking().Where(x => x.Id == dto.Id).FirstOrDefault();
            if (daily == null)
            {
                return BadRequest();
            }

            if (daily.AppUserId == User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value)
            {
                var data = mapper.Map<Daily>(dto);
                data.AddedIP = HttpContext.Connection.RemoteIpAddress.ToString();
                data.Date = DateTime.Now;
                appDbcontext.Update(data);
                appDbcontext.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        [Authorize]
        [HttpGet]

        public IActionResult Daily()
        {
            var userid = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var data = appDbcontext.Dailies.Where(x => x.AppUserId == userid).ToList();
            return Ok(data);
        }

        [Authorize(policy: "SearchFeature")]
        [HttpGet("Search/{word}")]

        public IActionResult Search(string word)
        {
            var userid = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var data =  appDbcontext.Dailies.Where(x => x.AppUserId == userid && x.Content.Contains(word)).ToList();

            return Ok(data);
        }



      
        [HttpGet("BuySearchFeature")]
        public async Task<IActionResult> BuySearchFeature()
        {

            var userid = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;


            var user = await userManager.FindByIdAsync(userid);

            if (!(user.Balance - 30 > 0))
            {
                return BadRequest("bakiye yetersiz");
            }

            var userClaims = await userManager.GetClaimsAsync(user);

            var feature = userClaims.Where(x => x.Type == AppDefaults.SearchClaim).FirstOrDefault();

            if (feature == null)
            {
                await userManager.AddClaimAsync(user, new Claim(AppDefaults.SearchClaim, DateTime.Now.AddDays(15).ToString())); ;
            }
            else
            {

                var currentDateValue = Convert.ToDateTime(feature.Value);



                if (DateTime.Now > currentDateValue)
                {
                    await userManager.RemoveClaimAsync(user, feature);

                    var endDate = DateTime.Now.AddDays(15);
                    await userManager.AddClaimAsync(user, new Claim(AppDefaults.SearchClaim, endDate.ToString())); ;
                }
                else
                {
                    var currentTime = feature.Value;
                    await userManager.RemoveClaimAsync(user, feature);

                    var newDate = Convert.ToDateTime(currentTime).AddDays(15).ToString();
                    await userManager.AddClaimAsync(user, new Claim(AppDefaults.SearchClaim, newDate));

                }



            }






            TokenGenerator tokenGenerator = new TokenGenerator();


            var userClaimss = (await userManager.GetClaimsAsync(user)).ToList();
            var RoleClaims = (await userManager.GetRolesAsync(user)).ToList().Select(x => new Claim(ClaimTypes.Role, x));

        var userInfoClaims = new List<Claim>()
         {

             new(ClaimTypes.NameIdentifier,user.Id),
             new(ClaimTypes.Email,user.Email),
             new(ClaimTypes.DateOfBirth,user.BirthDay.ToString()??string.Empty),
             new("city",user.City??"yok"),

         };

            userClaimss.AddRange(RoleClaims);
            userClaimss.AddRange(userInfoClaims);

            var token = tokenGenerator.GenerateToken(userClaimss);


            user.Balance -= 15;
           await  userManager.UpdateAsync(user);

            return Ok(JsonSerializer.Serialize(new
            {
                Message = "Satın alma işlemi başarılı yeni token ile oturuma devam edin",
                Token=token
            }));
        }
    }
}
