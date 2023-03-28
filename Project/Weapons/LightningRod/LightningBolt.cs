using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarSwarm.Project.Weapons.LightningRod
{
    public class LightningBolt : Node2D
    {
        [Signal]
        public delegate void BounceTriggered(LightningBolt bolt, PhysicsBody2D target);

        [Export]
        public List<Texture> AnimationFrames { get; set; } = default!;

        public float Damage { get; set; }

        public Node2D? Source;
        public Node2D Target = default!;
        public Vector2 SourcePosition { get; set; }
        public Vector2 TargetPosition { get; set; }
        public Events Events { get; set; } = default!;
        public AnimationPlayer AnimationPlayer { get; set; } = default!;
        public Line2D BoltLine { get; set; } = default!;
        public Tween Tween { get; set; } = default!;
        public Area2D BounceArea { get; set; } = default!;
        public Timer BounceTimer { get; set; } = default!;
        public float BounceCount { get; set; } = 1f;
        public List<Node2D> ForbiddenTargets { get; set; } = new List<Node2D>();

        private const float _lifeTimeDuration = .75f;
        private List<Vector2> _points = new List<Vector2>();
        private Boolean _damageApplied = false;

        public override void _Ready()
        {
            Events = GetNode<Events>("/root/Events");
            AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            BoltLine = GetNode<Line2D>("BoltLine");
            Tween = GetNode<Tween>("Tween");
            BounceArea = GetNode<Area2D>("BounceArea");
            BounceTimer = GetNode<Timer>("BounceTimer");

            BounceTimer.Connect("timeout", this, "OnBounceTimeout");
            BounceTimer.Start();

            BoltLine.TextureMode = Line2D.LineTextureMode.Tile;
            BoltLine.Width = 10;

            AnimationPlayer.Play("ChainLightning");

            Tween.Connect("tween_completed", this, "OnTweenCompleted");
            Tween.InterpolateProperty(
                BoltLine,
                "modulate",
                BoltLine.Modulate,
                Colors.Transparent,
                _lifeTimeDuration,
                Tween.TransitionType.Linear,
                Tween.EaseType.Out);
            Tween.Start();
        }

        public override void _Process(Single delta)
        {
            UpdatePoints();
            BoltLine.Points = _points.ToArray();
            if(!_damageApplied)
                ApplyDamage();
        }

        public void SetTexture(int frame)
        {
            BoltLine.Texture = AnimationFrames[frame];
        }

        private Node2D? GetNextTarget()
        {
            BounceArea.Position = TargetPosition;
            var bodiesInRange = BounceArea.GetOverlappingBodies().Cast<Node2D>().Except(ForbiddenTargets).ToList();
            if(!bodiesInRange.Any())
                return null;

            bodiesInRange.Remove(Target);

            return bodiesInRange.Find(x => x.GlobalPosition.DistanceSquaredTo(
                IsInstanceValid(Target) ? Target.GlobalPosition : TargetPosition + GlobalPosition
            ) == bodiesInRange.Min(x => x.GlobalPosition.DistanceSquaredTo(
                IsInstanceValid(Target) ? Target.GlobalPosition : TargetPosition + GlobalPosition)));
        }

        private void UpdatePoints()
        {
            SourcePosition = IsInstanceValid(Source) && Source != null ? Source.GlobalPosition - GlobalPosition : SourcePosition;
            TargetPosition = IsInstanceValid(Target) ? Target.GlobalPosition - GlobalPosition : TargetPosition;
            _points = new List<Vector2>
            {
                SourcePosition,
                TargetPosition
            };
            BounceArea.Position = TargetPosition;
        }

        private void OnBounceTimeout()
        {
            var target = GetNextTarget();
            if(target != null)
                EmitSignal("BounceTriggered", this, target);
        }

        private void OnTweenCompleted(Godot.Object incomingObject, NodePath key)
        {
            QueueFree();
        }

        private void ApplyDamage()
        {
            _damageApplied = true;
            if(IsInstanceValid(Target))
                Events.EmitSignal("Damaged", Target, Damage, this);
        }
    }
}