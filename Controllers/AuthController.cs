using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskMonitorWebAPI.Data;
using TaskMonitorWebAPI.Entities;
using TaskMonitorWebAPI.Dto;
using TaskMonitorWebAPI.Interface;

namespace TaskMonitorWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IUser _userRep;
        private readonly DataContext _context;

        public AuthController(IConfiguration config, IUser userRep, DataContext context)
        {
            _config = config;
            _userRep = userRep;
           
            _context = context;
        }

        [HttpPost("Admin/Register")]
        public ActionResult<User> Register(UserDto request)
        {

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

            if (_userRep.DoesItExist(request.Email))
                return BadRequest("User already exists");

            if (request == null)
            {
                return BadRequest("Invalid token.");
            }

            var dob = DateOnly.Parse(request.DateOfBirth);
            var ageCalc = DateTime.Now.Year - dob.Year;
            var user = new User()
            {
                Name = request.Name,
                Surname = request.Surname,
                UserName = request.UserName,
                EmailAddress = request.Email,
                Address = request.Address,
                Password = passwordHash,
                DateOfBirth = dob,
                age = ageCalc
            };


            _userRep.Insert(user);


            return Ok(user);
        }

        //[HttpPost("Register")]
        //public ActionResult<Supervisior> SupervisorRegister(UserDto request)
        //{
        //    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        //    if (_userRep.DoesItExist(request.Email))
        //        return BadRequest("User already exists");

        //    if (request == null)
        //    {
        //        return BadRequest("Invalid token.");
        //    }

        //    var user = new Supervisior()
        //    {
        //        Username = request.UserName,
        //        PasswordHash = passwordHash,
        //        EmailAddress = request.Email,
        //        Name = request.UserName,
        //        Surname = request.Surname,
        //    };

        //    _supervisorRep.Insert(user);


        //    return Ok(user);
        //}

        [HttpPost("Login")]
        public ActionResult<User> Login(LoginDto request)
        {

            //var supervisor = _supervisorRep.Find(request.Email);
            var user = _userRep.Find(request.Email);
            var userRole = "";
            var token = "";

            if (user != null)
            {

                if (!_userRep.DoesItExist(request.Email))
                {
                    return BadRequest("User not found");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    return BadRequest("wrong password");
                }


                userRole = user.Roles;
                token = CreateToken(user);
                return Ok(token);
            }
        
            


            return Ok(token);
        }
     
        private string CreateToken(User user)
        {

            var claims = new[]
            {
                  new Claim(ClaimTypes.NameIdentifier, user.UserName),
                  new Claim(ClaimTypes.Email, user.EmailAddress),
                  new Claim(ClaimTypes.Surname, user.Surname),
                  new Claim(ClaimTypes.GivenName, user.Name),
                  new Claim(ClaimTypes.Role,user.Roles),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtSettings:Key").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}