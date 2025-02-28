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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;


        public UserController(IUserRepository userRepository, IMapper mapper, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(registerDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Dados inválidos ou modelo incompleto.");

            }
            try
            {
                var user = _mapper.Map<User>(registerDTO);
                var createUser = await _userRepository.CreateUser(user);
                var createdUser = _mapper.Map<RegisterDTO>(createUser);
                return CreatedAtAction(nameof(Register), new { name = createdUser.Name }, createdUser);

            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário.");
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }

    }
    }

