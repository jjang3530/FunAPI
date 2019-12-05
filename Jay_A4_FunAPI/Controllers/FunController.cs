using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jay_A4_FunAPI.Data;
using Jay_A4_FunAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jay_A4_FunAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunController : ControllerBase
    {
        private readonly FunContext _context;

        public FunController(FunContext context)
        {
            _context = context;
        }

        // GET: api/Fun
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FunModel>>> GetFun()
        {
            return await _context.FunModel.ToListAsync();
        }

        // GET: api/Fun/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FunModel>> GetFun(int id)
        {
            var fun = await _context.FunModel.FirstOrDefaultAsync(i => i.FunId == id);

            if (fun == null)
            {
                return NotFound();
            }

            return fun;
        }

        // POST: api/Fun
        [HttpPost]
        public async Task<ActionResult<FunModel>> SaveFun(FunModel funModel)
        {
            _context.FunModel.Add(funModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFun", new { id = funModel.FunId }, funModel);
        }


        // PUT: api/Fun/5
        [HttpPut("{id}")]
        public async Task<ActionResult<FunModel>> UpdateFun(int id, FunModel funModel)
        {

            if (id != funModel.FunId)
            {
                return BadRequest();
            }

            _context.Entry(funModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FunExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FunModel>> DeleteFun(int id)
        {
            var fun = await _context.FunModel.
                FindAsync(id);
            if (fun == null)
            {
                return NotFound();
            }

            _context.FunModel.Remove(fun);
            await _context.SaveChangesAsync();

            return fun;
        }


        private bool FunExists(int id)
        {
            return _context.FunModel.Any(e => e.FunId == id);
        }
    }
}
