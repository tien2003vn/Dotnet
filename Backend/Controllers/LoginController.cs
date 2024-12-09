using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Backend.Services.Interface;

namespace Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly IWebHostEnvironment _env;
		private readonly IUserService _UserContext;

		public LoginController(IUserService context, IWebHostEnvironment env)
		{
			_UserContext = context;
			_env = env;
		}


		[HttpPost]
		public async Task<IActionResult> Login([FromBody] Login account)
		{

			var token = await _UserContext.FindToLogin(account.email, account.password);
			Console.WriteLine("Đậy là: " + token);
			if (token == null)
			{
				return Ok(false);
			}
			Response.Cookies.Append("Security", token, new CookieOptions
			{
				HttpOnly = true,
				Secure = false,
				SameSite = SameSiteMode.Strict,
				Expires = DateTimeOffset.Now.AddMinutes(30)
			});

			return Ok(true);
		}

		[HttpGet("gettoken")]
		[Authorize]
		public IActionResult CheckAuth()
		{
			// Kiểm tra cookie 'Security'
			var token = Request.Cookies["Security"];
			if (!string.IsNullOrEmpty(token))
			{
				return Ok(new { isAuthenticated = true });
			}

			return Ok(new { isAuthenticated = false });
		}

		[HttpGet("logout")]
		[Authorize]
		public IActionResult Logout()
		{
			if (Request.Cookies.ContainsKey("Security"))
			{
				Response.Cookies.Delete("Security");
				return Ok(true);
			}

			return Ok(false);

		}


		[HttpGet("CheckEmail")]
		public async Task<IActionResult> checkEmail(string email)
		{
			return Ok(new { result = _UserContext.IsHasEmail(email) });
		}

	}
}