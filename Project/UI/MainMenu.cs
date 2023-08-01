using Godot;
using StarSwarm.Project.UI;

public partial class MainMenu : Control
{
    private const float FADE_IN_TIME = 0.2f;
    private const float FADE_OUT_TIME = 2.5f;

    public ScreenFader? ScreenFader;

    public override void _Ready()
    {
        ScreenFader = GetNode<ScreenFader>("UIBehaviors/ScreenFader");
        ScreenFader.FadeIn();
    }

    public override async void _UnhandledInput(InputEvent @event)
    {
        if (ScreenFader!.IsPlaying)
            return;
        if (@event is InputEventKey || @event.IsActionPressed("thrust_forwards"))
        {
            ScreenFader.FadeOut();
            await ToSignal(ScreenFader, "AnimationFinished");
            GetTree().ChangeSceneToFile("res://Project/Main/Game.tscn");
        }
    }
}