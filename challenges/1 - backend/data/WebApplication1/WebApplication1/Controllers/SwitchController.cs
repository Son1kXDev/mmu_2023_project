using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities;

namespace WebApplication1.Controllers;

[Route("[controller]")]
[ApiController]
public class SwitchController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetSwitch()
    {
        var switchState = await db.SwitchStates.FirstOrDefaultAsync();
        return switchState != null ? Ok(switchState.State) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SetSwitch([FromBody] string state)
    {
        if (state != "true" && state != "false")
        {
            return BadRequest("Invalid state. State can be either 'true' or 'false'");
        }

        var switchState = await db.SwitchStates.FirstOrDefaultAsync();
        
        if (switchState != null)
        {
            if (switchState.State == state)
            {
                return BadRequest($"State is already set to {state}");
            }
            
            switchState.State = state;
        }
        else
        {
            db.SwitchStates.Add(new SwitchState { State = state });
        }

        await db.SaveChangesAsync();
        return Ok();
    }
}