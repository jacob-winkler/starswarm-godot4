using Godot;

namespace StarSwarm.Project.UI;

public partial class ScreenFader : TextureRect
{
    [Signal]
    public delegate void AnimationFinishedEventHandler();

    [Export]
    public float DurationFadeIn = 0.5f;

    [Export]
    public float DurationFadeOut = 2.5f;

    public bool IsPlaying;

    /// <summary>
    /// Animate from the current modulate color until the node is fully transparent.
    /// </summary>
    public void FadeIn()
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "modulate", Colors.Transparent, DurationFadeIn)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Linear)
            .Connect("finished", new Callable(this, "OnTweenCompleted"));
        Show();
        tween.Play();
        IsPlaying = true;
    }

    /// <summary>
    /// Animate from the current modulate color until the node is fully black.
    /// </summary>
    public void FadeOut(bool isDelayed = false)
    {
        var tween = CreateTween();
        tween.TweenInterval(isDelayed ? DurationFadeOut : 0.0f);
        tween.TweenProperty(this, "modulate", Colors.White, DurationFadeOut)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Linear)
            .Connect("finished", new Callable(this, "OnTweenCompleted"));
        Show();
        tween.Play();
        IsPlaying = true;
    }

    public void OnTweenCompleted()
    {
        EmitSignal("AnimationFinished");
        if (Modulate == Colors.Transparent)
            Hide();
        IsPlaying = false;
    }
}