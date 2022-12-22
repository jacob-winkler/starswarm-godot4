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

        [Export]
        private float _maxHealth = 100.0F;
        [Export]
        private float _accelerationMax = 15.0F;
        [Export]
        private float _linearSpeedMax = 350.0F;
        [Export]
        private float _angularSpeedMax = 120.0F;
        [Export]
        private float _angularAccelerationMax = 45.0F;

        public float Health { get { return Health; }  set {
                Health = Mathf.Clamp(value, 0.0F, _maxHealth);
                if (Mathf.IsEqualApprox(Health, 0.0F))
                    EmitSignal("HealthDepleted");
                Update("health");
            }
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
