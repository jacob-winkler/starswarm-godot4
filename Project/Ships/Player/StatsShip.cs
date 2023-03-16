using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Ships.Player
{
    public class StatsShip : Stats
    {
        [Signal]
        public delegate void HealthDepleted();
        [Signal]
        public delegate void MaxHealthUpdated();

        [Export]
        private readonly float _maxHealth = 10000F;
        [Export]
        private readonly float _accelerationMax = 15.0F;
        [Export]
        private readonly float _linearSpeedMax = 350.0F;
        [Export]
        private readonly float _angularSpeedMax = 120.0F;
        [Export]
        private readonly float _angularAccelerationMax = 45.0F;

        private float _health;
        public float Health { get { return _health; }  set {
                _health = Mathf.Clamp(value, 0.0F, _maxHealth);
                if (Mathf.IsEqualApprox(Health, 0.0F))
                    EmitSignal("HealthDepleted");
                Update("health");
            }
        }

        public StatsShip()
        {
            _health = _maxHealth;
        }

        public float GetMaxHealth()
        {
            return GetStat("maxHealth");
        }

        public float GetAccelerationMax()
        {
            return GetStat("accelerationMax");
        }

        public float GetLinearSpeedMax()
        {
            return GetStat("linearSpeedMax");
        }

        public float GetAngularSpeedMax()
        {
            return GetStat("angularSpeedMax");
        }

        public float GetAngularAccelerationMax()
        {
            return GetStat("angularAccelerationMax");
        }
    }
}
