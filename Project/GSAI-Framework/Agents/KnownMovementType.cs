using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.GSAI_Framework.Agents
{
    // SLIDE uses `move_and_slide`
    // COLLIDE uses `move_and_collide`
    // POSITION changes the `global_position` directly
    public enum KnownMovementType { Slide, Collide, Position }
}
