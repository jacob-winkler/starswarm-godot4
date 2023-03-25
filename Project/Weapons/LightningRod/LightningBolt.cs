using Godot;
using System;
using System.Collections.Generic;

namespace StarSwarm.Project.Weapons.LightningRod
{
    public class LightningBolt : Node2D
    {
        [Export]
        public List<Texture> AnimationFrames { get; set; } = default!;

        public Node2D Source = default!;
        public Node2D Target = default!;
        public AnimationPlayer AnimationPlayer { get; set; } = default!;
        public Line2D BoltLine { get; set; } = default!;
        public Tween Tween { get; set; } = default!;
        public float LifeTimeDuration { get; set; } = .75f;

        private Vector2 _sourcePosition;
        private Vector2 _targetPosition;
        private List<Vector2> _points = new List<Vector2>();

        public override void _Ready()
        {
            AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            BoltLine = GetNode<Line2D>("BoltLine");
            Tween = GetNode<Tween>("Tween");

            BoltLine.TextureMode = Line2D.LineTextureMode.Tile;
            BoltLine.Width = 10;

            AnimationPlayer.Play("ChainLightning");

            Tween.Connect("tween_completed", this, "OnTweenCompleted");
            Tween.InterpolateProperty(
                BoltLine,
                "modulate",
                BoltLine.Modulate,
                Colors.Transparent,
                LifeTimeDuration,
                Tween.TransitionType.Linear,
                Tween.EaseType.Out);
            Tween.Start();
        }

        public override void _Process(Single delta)
        {
            UpdatePoints();
            BoltLine.Points = _points.ToArray();
        }

        public void SetTexture(int frame)
        {
            BoltLine.Texture = AnimationFrames[frame];
        }

        private void OnTweenCompleted(Godot.Object incomingObject, NodePath key)
        {
            QueueFree();
        }

        private void UpdatePoints()
        {
            _sourcePosition = IsInstanceValid(Source) ? Source.GlobalPosition - GlobalPosition : _sourcePosition;
            _targetPosition = IsInstanceValid(Target) ? Target.GlobalPosition - GlobalPosition : _targetPosition;
            _points = new List<Vector2>
            {
                _sourcePosition,
                _targetPosition
            };
        }
    }
}