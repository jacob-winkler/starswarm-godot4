using StarSwarm.Project.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Ships.Player
{
    public class PlayerState : State
    {
        public PlayerShip Ship;

        public override async void _Ready()
        {
            base._Ready();
            
            await ToSignal(Owner, "ready");
            Ship = (PlayerShip)Owner;
        }
    }
}
