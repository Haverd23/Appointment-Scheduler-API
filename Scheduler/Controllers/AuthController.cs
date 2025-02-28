using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheduler.DTOs.User;
using Scheduler.Models;
using Scheduler.Repository.Interfaces;
using Scheduler.Services;

namespace Scheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PasswordHasher _passwordHasher;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthController(PasswordHasher passwordHasher, TokenService tokenService,
            IMapper mapper, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null) return BadRequest("Dados nulos");
            var user = _mapper.Map<User>(userLoginDTO);
            var userValidation = await _userRepository.GetUserByEmail(user);

            if(userValidation != null)
            {
                if (!_passwordHasher.VerifyPassword(userLoginDTO.Password, userValidation.Password))
                {
                    return Unauthorized(new { message = "Senha incorreta" });
                }
            }
            else
            {
                return NotFound(new { message = "Email não encontrado" });
            }
            var token = await _tokenService.CreateToken(userValidation);

            return Ok(new
            {
                Email = userLoginDTO.Email,
                Token = token,

            });

        }
    }
}
