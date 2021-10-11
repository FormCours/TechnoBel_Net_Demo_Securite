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

        public MemberController(IMemberService memberService)
        {
            _MemberService = memberService;
        }

        [HttpPost]
        public IActionResult Register(MemberRegister member)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            string salt = EncryptionManager.GenerateSalt();
            string passwordHash = EncryptionManager.Hash(member.Password, salt);

            int id = _MemberService.Create(new DAL.Entities.Member
            {
                Username = member.Username,
                Email = member.Email,
                Password = passwordHash,
                Salt = salt
            });

            return Ok(new {
                MemberId = id
            });
        }
    }
}
