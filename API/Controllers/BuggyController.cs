﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers{

    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("server-error")]
        public ActionResult<string> FirstNotFound()
        {
            var thing = _context.Users.Find(-1);
            if(thing == null) return thing.UserName;

            return thing.UserName;
        }

    }
}


