using Godot;
using System;
using System.Collections.Generic;

namespace StarSwarm.Project.Weapons.LightningRod
{
    public class LightningBolt : Node2D
    {
        [Export]
        public float AngleVariation { get; set; } = 15;
        [Export]
        public Vector2 TargetPoint = new Vector2(100, 100);

        public Timer Timer { get; set; } = default!;
        public Line2D Line { get; set; } = default!;

        private bool emitting = true;
        private float minSegmentSize = 2;
        private float maxSegmentSize = 10;
        private Vector2 finalGoal;
        private List<Vector2> points = new List<Vector2>();

        public override void _Ready()
        {
            Line = GetNode<Line2D>("Line2D");
            finalGoal = TargetPoint - GlobalPosition;
            Timer = new Timer
            {
                OneShot = true
            };
            AddChild(Timer);
            Timer.Connect("timeout", this, "OnTimeout");
            Timer.Start((float)GD.RandRange(0.1f, 0.5f));
        }

        private void OnTimeout()
        {
            if (points.Count > 0)
            {
                points.RemoveAt(0);
                Line.Points = points.ToArray();
                Timer.Start((float)(0.002f + GD.RandRange(-0.001f, 0.001f)));
            }
            else if (emitting)
            {
                UpdatePoints();
                Line.Points = points.ToArray();
                Timer.Start((float)(0.1f + GD.RandRange(-0.02f, 0.1f)));
                emitting = false;
            }
            else
            {
                QueueFree();
            }
        }

        private void UpdatePoints()
        {
            finalGoal = TargetPoint - GlobalPosition;
            var currLineLen = 0f;
            points = new List<Vector2>() { new Vector2() };
            var startPoint = new Vector2();

            minSegmentSize = Mathf.Max(finalGoal.DistanceTo(Vector2.Zero) / 40, 1);
            maxSegmentSize = Mathf.Min(finalGoal.DistanceTo(Vector2.Zero) / 20, 10);

            while (currLineLen < finalGoal.Length())
            {
                var moveVector = startPoint.DirectionTo(finalGoal) * (float)GD.RandRange(minSegmentSize, maxSegmentSize);
                var newPoint = startPoint + moveVector;
                var newPointRotated = startPoint + moveVector.Rotated(Mathf.Deg2Rad((float)GD.RandRange(-AngleVariation, AngleVariation)));
                points.Add(newPointRotated);
                startPoint = newPoint;
                currLineLen = startPoint.Length();
            }

            points.Add(finalGoal);
        }

        public void SetLineWidth(float amount)
        {
            Line.Width = amount;
        }
    }
}