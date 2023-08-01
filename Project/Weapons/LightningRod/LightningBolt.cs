using Godot;
using StarSwarm.Project.Autoload;
using System;
using System.Collections.Generic;
using System.Linq;
using static Godot.Tween;

namespace StarSwarm.Project.Weapons.LightningRod
{
    public partial class LightningBolt : Node2D
    {
        [Signal]
        public delegate void BounceTriggeredEventHandler(LightningBolt bolt, PhysicsBody2D target);

        [Export]
        public Godot.Collections.Array<Texture2D> AnimationFrames { get; set; } = default!;

        public float Damage { get; set; }

        public AudioManager2D AudioManager { get; set; } = default!;
        public Node2D? Source;
        public Node2D Target = default!;
        public Vector2 SourcePosition { get; set; }
        public Vector2 TargetPosition { get; set; }
        public Events Events { get; set; } = default!;
        public AnimationPlayer AnimationPlayer { get; set; } = default!;
        public Line2D BoltLine { get; set; } = default!;
        public Area2D BounceArea { get; set; } = default!;
        public Timer BounceTimer { get; set; } = default!;
        public float BounceCount { get; set; } = 1f;
        public List<Node2D> ForbiddenTargets { get; set; } = new List<Node2D>();

        private const float _lifeTimeDuration = .75f;
        private List<Vector2> _points = new();
        private Boolean _damageApplied = false;

        public override void _Ready()
        {
            AudioManager = GetNode<AudioManager2D>("/root/AudioManager2D");
            Events = GetNode<Events>("/root/Events");
            AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            BoltLine = GetNode<Line2D>("BoltLine");
            BounceArea = GetNode<Area2D>("BounceArea");
            BounceTimer = GetNode<Timer>("BounceTimer");

            BounceTimer.Connect("timeout", new Callable(this, "OnBounceTimeout"));
            BounceTimer.Start();

            BoltLine.TextureMode = Line2D.LineTextureMode.Tile;
            BoltLine.Width = 10;

            AudioManager.Play(KnownAudioStream2Ds.LightningRod, Target.GlobalPosition);
            AnimationPlayer.Play("ChainLightning");

            var tween = CreateTween();
            tween.TweenProperty(BoltLine, "modulate", Colors.Transparent, _lifeTimeDuration)
                .SetEase(EaseType.Out)
                .SetTrans(TransitionType.Linear)
                .Connect("finished", new Callable(this, "OnTweenCompleted"));
            tween.Play();
        }

        public override void _Process(double delta)
        {
            UpdatePoints();
            BoltLine.Points = _points.ToArray();
            if(!_damageApplied)
                ApplyDamage();
        }

        public void SetTexture(int frame) => BoltLine.Texture = AnimationFrames[frame];

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

        private void OnTweenCompleted() => QueueFree();

        private void ApplyDamage()
        {
            _damageApplied = true;
            if(IsInstanceValid(Target))
                Events.EmitSignal("Damaged", Target, Damage, this);
        }
    }
}