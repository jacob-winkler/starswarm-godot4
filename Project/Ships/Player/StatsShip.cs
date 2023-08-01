using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Ships.Player
{
    public partial class StatsShip : Stats
    {
        [Signal]
        public delegate void HealthDepletedEventHandler();
        [Signal]
        public delegate void MaxHealthUpdatedEventHandler();

        [Export]
        private float _maxHealth = 10000F;
        [Export]
        private float _accelerationMax = 15.0F;
        [Export]
        private float _linearSpeedMax = 350.0F;
        [Export]
        private float _angularSpeedMax = 120.0F;
        [Export]
        private float _angularAccelerationMax = 45.0F;
        [Export]
        private float _health;

        public String TestProperty { get; set; }

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

        public float GetMaxHealth() => GetStat("maxHealth");

        public float GetAccelerationMax() => GetStat("accelerationMax");

        public float GetLinearSpeedMax() => GetStat("linearSpeedMax");

        public float GetAngularSpeedMax() => GetStat("angularSpeedMax");

        public float GetAngularAccelerationMax() => GetStat("angularAccelerationMax");
    }
}
