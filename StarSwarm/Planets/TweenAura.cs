using System;
using Godot;
using static Godot.Tween;

namespace StarSwarm.Planets;

public partial class TweenAura : Node
{
    [Export]
    public Vector2 ScaleHidden { get; set; } = Vector2.Zero;
    [Export]
    public Vector2 ScaleFinal { get; set; } = Vector2.One;
    [Export]
    public float DurationAppear { get; set; } = 1f;
    [Export]
    public float DurationDisappear { get; set; } = 0.5f;

    private Tween? AuraTween { get; set; }

    public void MakeAppear(Sprite2D aura)
    {
        AuraTween?.Kill();

        if (aura.Visible)
            return;

        AuraTween = CreateTween();

        AuraTween.TweenProperty(aura, "scale", ScaleFinal, DurationAppear)
            .SetTrans(TransitionType.Elastic)
            .SetEase(EaseType.Out);

        aura.Visible = true;
        AuraTween.Play();
    }

    public async void MakeDisappear(Sprite2D aura)
    {
        AuraTween?.Kill();

        if (!aura.Visible)
            return;

        AuraTween = CreateTween();

        AuraTween.TweenProperty(aura, "scale", ScaleHidden, DurationDisappear)
            .SetTrans(TransitionType.Back)
            .SetEase(EaseType.In);

        AuraTween.Play();
        await ToSignal(AuraTween, "finished");
        aura.Visible = false;
    }

    public bool IsRunning() { return AuraTween  != null && AuraTween.IsRunning();}

    public void Pause() 
    {
        AuraTween?.Kill();
    }
}
