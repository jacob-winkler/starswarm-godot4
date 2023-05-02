using Godot;

namespace StarSwarm.Project.Planets
{
    public class TweenAura : Tween
    {
        [Export]
        public Vector2 ScaleHidden { get; set; } = Vector2.Zero;
        [Export]
        public Vector2 ScaleFinal { get; set; } = Vector2.One;
        [Export]
        public float DurationAppear { get; set; } = 1f;
        [Export]
        public float DurationDisappear { get; set; } = 0.5f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }

        public void MakeAppear(Sprite aura)
        {
            if (IsActive())
                return;

            if (aura.Visible)
                return;

            InterpolateProperty(
                aura, "scale", aura.Scale, ScaleFinal, DurationAppear, TransitionType.Elastic, EaseType.Out
            );

            aura.Visible = true;
            Start();
        }

        public async void MakeDisappear(Sprite aura)
        {
            if (IsActive())
                return;

            if (!aura.Visible)
                return;

            InterpolateProperty(
                aura, "scale", aura.Scale, ScaleHidden, DurationDisappear, TransitionType.Back, EaseType.In
            );

            Start();
            await ToSignal(this, "tween_completed");
            aura.Visible = false;
        }
    }
}
