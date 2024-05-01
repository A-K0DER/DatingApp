﻿using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers{

    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registorDto)
        {
            if(await UserExists(registorDto.UserName)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registorDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registorDto.Password)),
                PasswordSalt = hmac.Key
                
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            return user;
        } 

        private async Task<bool> UserExists(string userName){
            return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        } 
    }
}

