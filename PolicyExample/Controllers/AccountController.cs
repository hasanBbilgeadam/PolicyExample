using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PolicyExample.Context;
using PolicyExample.Dtos;
using PolicyExample.Extentions;
using PolicyExample.Tokens;
using System.Security.Claims;

namespace PolicyExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IValidator<UserLoginDto> userLoginDtoValidator;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IValidator<UserLoginDto> userLoginDtoValidator)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userLoginDtoValidator = userLoginDtoValidator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {

            var result = await userManager.CreateAsync(new AppUser()
            {

                UserName = dto.UserName,
                Email = dto.Email,
                BirthDay = dto.BirthDay,
                City = dto.City,
                Name=dto.Name,
                SurName=dto.SurName,
                Balance=0

            }, dto.Password);





            return Ok(result.Succeeded ? "başarılı" : "başarısız");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {

            var validationResult = userLoginDtoValidator.Validate(dto);
                    
           if (!validationResult.IsValid)
                return BadRequest(validationResult.CustomValidationErrorList());

            var data = await userManager.FindByNameAsync(dto.UserName);

            if (data == null)
            {
                return BadRequest();
            }

            if (!await userManager.CheckPasswordAsync(data, dto.Password))
            {
                return BadRequest();
            }

            TokenGenerator tokenGenerator = new TokenGenerator();


            var userClaims = (await userManager.GetClaimsAsync(data)).ToList();
            var RoleClaims = (await userManager.GetRolesAsync(data)).ToList().Select(x => new Claim(ClaimTypes.Role, x));

            var userInfoClaims = new List<Claim>()
         {

             new(ClaimTypes.NameIdentifier,data.Id),
             new(ClaimTypes.Email,data.Email),
             new(ClaimTypes.DateOfBirth,data.BirthDay.ToString()??string.Empty),
             new("city",data.City??"yok"),

         };

            userClaims.AddRange(RoleClaims);
            userClaims.AddRange(userInfoClaims);

            var token = tokenGenerator.GenerateToken(userClaims);


            return Ok(token);
        }


        [HttpPost("AddClaimToUser")]
        public async Task<IActionResult> AddClaimToUser(UserClaimAddDto dto)
        {

            var user = await userManager.FindByIdAsync(dto.UserId);

            if (user == null) { return BadRequest(); }

            await userManager.AddClaimAsync(user, new Claim(dto.ClaimType, dto.ClaimValue));

            return Ok();
        }

        [Authorize(Policy = "AnkaraPolicy")]
        [HttpGet("AccesWithCityClaim")]
        public IActionResult AccesWithCityClaim()
        {
            return Ok();
        }
        [Authorize(Policy = "FreeAccessPolicy")]
        [HttpGet("FreeAcccessTest")]
        public IActionResult FreeAccessTest()
        {
            return Ok(User.FindFirst(x => x.Type == "AccessDate").Value);
        }

        ///muhasebe uygula fatura detay 
    }
}
