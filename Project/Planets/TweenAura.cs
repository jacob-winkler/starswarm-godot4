using Godot;
using System;
using static Godot.Tween;

namespace StarSwarm.Project.Planets;

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
        if (AuraTween != null)
            AuraTween.Kill();

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
        if (AuraTween != null)
            AuraTween.Kill();

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

    public Boolean IsRunning() => AuraTween == null ? false : AuraTween.IsRunning();

    public void Pause() 
    {
        if (AuraTween != null)
            AuraTween.Kill();
    }
}
