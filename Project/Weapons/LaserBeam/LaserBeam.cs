using Godot;
using System;

namespace StarSwarm.Project.Weapons.LaserBeam
{
    public partial class LaserBeam : RayCast2D
    {
        public float DamagePerSecond { get; set; } = 4000;

        public LaserAudioPlayer AudioPlayer { get; set; } = default!;
        public Line2D FillLine { get; set; } = default!;
        public GpuParticles2D CastingParticles { get; set; } = default!;
        public GpuParticles2D CollisionParticles { get; set; } = default!;
        public GpuParticles2D BeamParticles { get; set; } = default!;
        public Events Events { get; set; } = default!;
        public Tween? Tween { get; set; }


        private Boolean _isCasting;
        private float _lineWidth;

        public Boolean IsCasting {
            get { return _isCasting; }
            set
            {
                _isCasting = value;

                if (_isCasting)
                {
                    TargetPosition = Vector2.Zero;
                    FillLine.Points[1] = TargetPosition;
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
            AudioPlayer = GetNode<LaserAudioPlayer>("LaserAudioPlayer");
            FillLine = GetNode<Line2D>("FillLine");
            CastingParticles = GetNode<GpuParticles2D>("CastingParticles");
            CollisionParticles = GetNode<GpuParticles2D>("CollisionParticles");
            BeamParticles = GetNode<GpuParticles2D>("BeamParticles");
            Events = GetNode<Events>("/root/Events");

            SetPhysicsProcess(false);
            FillLine.SetPointPosition(1, Vector2.Zero);
            _lineWidth = FillLine.Width;
        }

        public override void _PhysicsProcess(double delta)
        {
            TargetPosition = (TargetPosition + (Vector2.Right * 7000 * (float)delta)).LimitLength(400);
            CastBeam(delta);
        }

        public void CastBeam(double delta)
        {
            var castPoint = TargetPosition;

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
            ParticleProcessMaterial material = (ParticleProcessMaterial)BeamParticles.ProcessMaterial;
            material.EmissionBoxExtents = new Vector3()
            {
                X = castPoint.Length() * 0.5f,
                Y = material.EmissionBoxExtents.Y,
                Z = material.EmissionBoxExtents.Z
            };
        }
        
        private void Appear()
        {
            if(Tween != null)
                Tween.Kill();
            Tween = CreateTween();

            Tween.TweenProperty(FillLine, "width", _lineWidth, 0.2f);
            Tween.Play();
            AudioPlayer.Start();
        }

        private void Disappear()
        {
            if (Tween != null)
                Tween.Kill();
            Tween = CreateTween();

            Tween.TweenProperty(FillLine, "width", 0f, 0.1f);
            Tween.Play();
            AudioPlayer.End();
        }

        private void ApplyDamage(double delta)
        {
            var collider = GetCollider();
            Events.EmitSignal("Damaged", collider, DamagePerSecond * delta, this);
        }
    }
}