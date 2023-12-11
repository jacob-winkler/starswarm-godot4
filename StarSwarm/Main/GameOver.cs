using System;
using Godot;
using StarSwarm.UI;

namespace StarSwarm.Main;

public partial class GameOver : TextureRect
{
    public Label GameOverLabel { get; set; } = default!;
    public ScreenFader ScreenFader { get; set; } = default!;

    private bool _screenFinished { get; set; } = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GameOverLabel = GetNode<Label>("LabelLayer/GameOverLabel");
        ScreenFader = GetNode<ScreenFader>("FaderLayer/ScreenFader");
    }

    public async void Start()
    {
        ScreenFader.FadeOut();
			await ToSignal(ScreenFader, "AnimationFinished");
        GameOverLabel.Visible = true;
        _screenFinished = true;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!_screenFinished)
            return;

        if(@event is InputEventKey || @event.IsActionPressed("thrust_forwards"))
        {
            GetTree().Quit();
        }
    }
}