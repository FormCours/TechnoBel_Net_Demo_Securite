using Demo_Securite.DAL.Interfaces;
using Demo_Securite.Models;
using Demo_Securite.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_Securite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private IMemberService _MemberService;
        private EncryptionManager _EncryptionManager;

        public MemberController(IMemberService memberService, EncryptionManager encryptionManager)
        {
            _MemberService = memberService;
            _EncryptionManager = encryptionManager;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(MemberRegister member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string salt = _EncryptionManager.GenerateSalt();
            string passwordHash = _EncryptionManager.Hash(member.Password, salt);

            int id = _MemberService.Create(new DAL.Entities.Member
            {
                Username = member.Username,
                Email = member.Email,
                Password = passwordHash,
                Salt = salt
            });

            return Ok(new
            {
                Message = $"You account is create with id « {id} »"
            });
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(MemberLogin member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            DAL.Entities.MemberCredential credential = _MemberService.GetCredential(member.Email);
            if (credential == null || string.IsNullOrWhiteSpace(credential.Salt))
            {
                return new ForbidResult();
            }

            bool loginValid = _EncryptionManager.Verify(member.Password, credential.Salt, credential.HashPassword);
            if(!loginValid)
            {
                return new ForbidResult();
            }

            // DAL.Entities.Member memberData = _MemberService.GetByEmail(member.Email);
            //  ↑ A utiliser pour générer le JWT ;)

            return Ok(new
            {
                Message= "You account is valid :)"
            });
        }
    }
}
