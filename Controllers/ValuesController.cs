using DatApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;                   //Datacontext class defined in a constructor
        public ValuesController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var values = await _context.Values.ToListAsync();
            return Ok(values);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValues(int id)
        {
            var values = await _context.Values.FirstOrDefaultAsync(a => a.Id == id);
                return Ok(values);
        }
    }
}
