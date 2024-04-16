//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using TaskMonitorWebAPI.Dto;
//using TaskMonitorWebAPI.Entities;

//namespace TaskMonitorWebAPI.Controllers
//{

//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserController : ControllerBase
//    {
//        private readonly UserManager<User> userManager;
//        private readonly SignInManager<User> signInManager;

//        public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
//        {
//            this.userManager = userManager;
//            this.signInManager = signInManager;
//        }

//        [HttpPost("SignUp")]
//        public async Task<IActionResult> SignUp(Register model)
//        {

//            var dob = DateOnly.Parse(model.DateOfBirth);
//            var ageCalc = DateTime.Now.Year - dob.Year;
//            var user = new User()
//            {
//                Name = model.Name,
//                Surname = model.Surname,
//                UserName = model.Username,
//                Address = model.Address,
//                PasswordHash = model.Password,
//                DateOfBirth = dob, 
//                age = ageCalc
//            };

//            var result = await userManager.CreateAsync(user, user.PasswordHash);
//            string Message = string.Empty;

//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            if (result.Succeeded)
//            {
//                return Ok("Successfully created");
//            }
//            return BadRequest("Error occured");
//        }


//        [HttpPost("Login")]
//        public async Task<IActionResult> Login(string email, string password)
//        {
//            var signIn = await signInManager.PasswordSignInAsync(userName: email,
//                password: password,
//                isPersistent: false,
//                lockoutOnFailure: false);

//            if (signIn.Succeeded)
//            {
//                return Ok("succeffully logged in");
//            }

//            return BadRequest("error ocurred");
//        }
//    }
//}
