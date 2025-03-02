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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateDTO updateDTO)
        {
            if(updateDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Dados inválidos ou modelo incompleto");
            }
            try
            {
                var existingUser = await _userRepository.GetUserById(id);
                if (existingUser == null) return NotFound(new { message = "Usuário não encontrado." });
                var updateUser = _mapper.Map<User>(updateDTO);
                var result = await _userRepository.UpdateUserAsync(id, updateUser);

                if (result == null)
                {
                    return NotFound(new { message = "Falha ao atualizar o usuário" });

                }
                var updateUserDTO = _mapper.Map<UpdateDTO>(result);
                return Ok(updateUserDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário");
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var userExists = await _userRepository.GetUserById(id);
                if (userExists == null)
                {
                    return NotFound(new { message = "Usuário não encontrado." });
                }

                var deleted = await _userRepository.DeleteUserAsync(id);

                if (!deleted)
                {
                    return StatusCode(500, "Falha ao excluir o usuário.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir usuário.");
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetUsersByFilter(string? role,string? email)
        {
            if (string.IsNullOrEmpty(role) && string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Informe pelo menos um filtro: role ou email." });
            }

            try
            {
                var users = await _userRepository.GetUserAsync(role, email);

                if (users == null || users.Count == 0)
                {
                    return NotFound(new { message = "Nenhum usuário encontrado com os critérios informados." });
                }

                var usersDTO = _mapper.Map<List<GetUserDTO>>(users);
                return Ok(usersDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários.");
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();

                if (users == null || users.Count == 0)
                {
                    return NotFound(new { message = "Nenhum usuário encontrado." });
                }

                var usersDTO = _mapper.Map<List<GetUserDTO>>(users);
                return Ok(usersDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar todos os usuários.");
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }


    }
}

