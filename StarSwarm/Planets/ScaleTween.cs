using System;
using Godot;
using static Godot.Tween;

namespace StarSwarm.Planets;

public partial class ScaleTween : Node
{
    [Export]
    public Vector2 ScaleHidden { get; set; } = Vector2.Zero;
    [Export]
    public Vector2 ScaleFinal { get; set; } = Vector2.One;
    [Export]
    public float DurationAppear { get; set; } = 1f;
    [Export]
    public float DurationDisappear { get; set; } = 0.5f;

    private Tween? Tween { get; set; }

    public void MakeAppear(Control control)
    {
        Tween?.Kill();

        if (control.Visible)
            return;

        Tween = CreateTween();

        Tween.TweenProperty(control, "scale", ScaleFinal, DurationAppear)
            .SetTrans(TransitionType.Elastic)
            .SetEase(EaseType.Out);

        control.Visible = true;
        Tween.Play();
    }

    public async void MakeDisappear(Control control)
    {
        Tween?.Kill();

        if (!control.Visible)
            return;

        Tween = CreateTween();

        Tween.TweenProperty(control, "scale", ScaleHidden, DurationDisappear)
            .SetTrans(TransitionType.Back)
            .SetEase(EaseType.In);

        Tween.Play();
        await ToSignal(Tween, "finished");
        control.Visible = false;
    }

    public void MakeAppear(Node2D node)
    {
        Tween?.Kill();

        if (node.Visible)
            return;

        Tween = CreateTween();

        Tween.TweenProperty(node, "scale", ScaleFinal, DurationAppear)
            .SetTrans(TransitionType.Elastic)
            .SetEase(EaseType.Out);

        node.Visible = true;
        Tween.Play();
    }

    public async void MakeDisappear(Node2D node)
    {
        Tween?.Kill();

        if (!node.Visible)
            return;

        Tween = CreateTween();

        Tween.TweenProperty(node, "scale", ScaleHidden, DurationDisappear)
            .SetTrans(TransitionType.Back)
            .SetEase(EaseType.In);

        Tween.Play();
        await ToSignal(Tween, "finished");
        node.Visible = false;
    }
}
