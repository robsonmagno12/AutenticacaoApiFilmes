using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data.DTOs;
using UsuariosApi.Data.Requests;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class CadastroService
    {
        private IMapper _mapper;
        private UserManager<IdentityUser<int>> _userManage;

        public CadastroService(IMapper mapper, UserManager<IdentityUser<int>> userManage)
        {
            _mapper = mapper;
            _userManage = userManage;
        }

        public Result CadastroUsuario(CreateUsuarioDto createDto)
        {
            Usuario usuario = _mapper.Map<Usuario>(createDto);
            IdentityUser<int> usuarioIdentity = _mapper.Map<IdentityUser<int>>(usuario);
            Task<IdentityResult> resultadoIdentity = _userManage.CreateAsync(usuarioIdentity, createDto.Password);
            if (resultadoIdentity.Result.Succeeded) return Result.Ok();
            return Result.Fail("Houve uma falha ao cadastra Usuario");
            
        }

        public Result AtivaContaUsuario(AtivaContaRequest request)
        {
            // identificar se o usuario tem o mesmo ID
            var identityUser = _userManage.Users.FirstOrDefault(u => u.Id == request.UsuarioId);
            //confirmando email
            var identityResult = _userManage.ConfirmEmailAsync(identityUser, request.CodigoDeAtivacao).Result;
            if(identityResult.Succeeded)
            {
                return Result.Ok();
            }
            return Result.Fail("Falha ao ativar conta do usuário");
        }
    }
}
