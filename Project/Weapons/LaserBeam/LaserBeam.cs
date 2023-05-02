using Godot;
using System;

namespace StarSwarm.Project.Weapons.LaserBeam
{
    public class LaserBeam : RayCast2D
    {
        public float DamagePerSecond { get; set; } = 4000;
        public Tween Tween { get; set; } = default!;
        public Line2D FillLine { get; set; } = default!;
        public Particles2D CastingParticles { get; set; } = default!;
        public Particles2D CollisionParticles { get; set; } = default!;
        public Particles2D BeamParticles { get; set; } = default!;
        public Events Events { get; set; } = default!;

        private Boolean _isCasting;
        private float _lineWidth;

        public Boolean IsCasting {
            get { return _isCasting; }
            set
            {
                _isCasting = value;

                if (_isCasting)
                {
                    CastTo = Vector2.Zero;
                    FillLine.Points[1] = CastTo;
                    Appear();
                }
                else
                {
                    CollisionParticles.Emitting = false;
                    Disappear();
                }

                BeamParticles.Emitting = _isCasting;
                CastingParticles.Emitting = _isCasting;
                SetPhysicsProcess(IsCasting);
            }
        }

        public override void _Ready()
        {
            Tween = GetNode<Tween>("Tween");
            FillLine = GetNode<Line2D>("FillLine");
            CastingParticles = GetNode<Particles2D>("CastingParticles");
            CollisionParticles = GetNode<Particles2D>("CollisionParticles");
            BeamParticles = GetNode<Particles2D>("BeamParticles");
            Events = GetNode<Events>("/root/Events");

            SetPhysicsProcess(false);
            FillLine.SetPointPosition(1, Vector2.Zero);
            _lineWidth = FillLine.Width;
        }

        public override void _PhysicsProcess(float delta)
        {
            CastTo = (CastTo + (Vector2.Right * 7000 * delta)).LimitLength(400);
            CastBeam(delta);
        }

        public void CastBeam(float delta)
        {
            var castPoint = CastTo;

            ForceRaycastUpdate();
            CollisionParticles.Emitting = IsColliding();

            if (IsColliding())
            {
                ApplyDamage(delta);
                castPoint = ToLocal(GetCollisionPoint());
                CollisionParticles.GlobalRotation = GetCollisionNormal().Angle();
                CollisionParticles.Position = castPoint;
            }

            FillLine.SetPointPosition(1, castPoint);
            BeamParticles.Position = castPoint * 0.5f;
            ParticlesMaterial material = (ParticlesMaterial)BeamParticles.ProcessMaterial;
            material.EmissionBoxExtents = new Vector3(material.EmissionBoxExtents)
            {
                x = castPoint.Length() * 0.5f
            };
        }
        
        private void Appear()
        {
            Tween.StopAll();
            Tween.InterpolateProperty(FillLine, "width", 0f, _lineWidth, 0.2f);
            Tween.Start();
        }

        private void Disappear()
        {
            Tween.StopAll();
            Tween.InterpolateProperty(FillLine, "width", FillLine.Width, 0f, 0.1f);
            Tween.Start();
        }

        private void ApplyDamage(float delta)
        {
            var collider = GetCollider();
            Events.EmitSignal("Damaged", collider, DamagePerSecond * delta, this);
        }
    }
}