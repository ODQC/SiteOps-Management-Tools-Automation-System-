using AspNetIdentityDemo.Api.Models;
using AspNetIdentityDemo.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IServicioEmployee _userService;

        public AuthController(IServicioEmployee userService)
        {
            _userService = userService;

        } 

        [HttpPost("Register")]
        [Authorize(Roles = Roles.ROLE_ADMIN)]
        public async Task<IActionResult> RegisterAsync([FromBody] EmployeeDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.guardarEmployee(model);

                if (result.Codigo == CodigosEstadoHTTP.HTTP_STATUS_CREATED)
                    return StatusCode(201,result); 

                return BadRequest(result);
            }

            return BadRequest(new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "Algunas propiedades son invalidas", null));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);

                if (result.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest(new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "Some properties are not valid", null));
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _userService.ForgetPasswordAsync(email);

            if (result.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
                return Ok(result); 

            return BadRequest(result); 
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(model);

                if (result.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest(new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "Some properties are not valid", null));
        }

    }
}
