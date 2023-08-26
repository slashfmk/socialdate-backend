using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialbackend.Entities;
using socialbackend.Data;

namespace socialbackend.Controllers
{
    public class UserApiController : BaseApiController
    {
        private readonly DataContext _context;

        public UserApiController(DataContext context)
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
            var result = await _context.Users.FindAsync(id);

            if (result is null) return NotFound();
            return result;
        }
    }
}