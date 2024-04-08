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
        return switchState != null ? Ok(switchState) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SetSwitch([FromBody] SwitchState state)
    {
        if (state.State != "true" && state.State != "false")
        {
            return BadRequest("Invalid state. State can be either 'true' or 'false'");
        }

        var switchState = await db.SwitchStates.FirstOrDefaultAsync();
        
        if (switchState != null)
        {
            if (state.Id != switchState.Id)
            {
                return BadRequest($"Invalid ID: {state.Id}");
            }
            
            if (switchState.State == state.State)
            {
                return BadRequest($"State {state.Id} is already set to {state.State}");
            }
            
            switchState.State = state.State;
        }
        else
        {
            db.SwitchStates.Add(new SwitchState { Id = state.Id, State = state.State });
        }

        await db.SaveChangesAsync();
        return Ok(state);
    }
}