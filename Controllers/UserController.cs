using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialbackend.Entities;
using socialbackend.Data;

namespace socialbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

    }
}