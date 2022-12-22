using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.UI
{
    public class ScreenFader : TextureRect
    {
        [Signal]
        public delegate void AnimationFinished();

        [Export]
        public float DurationFadeIn = 0.5f;
        [Export]
        public float DurationFadeOut = 2.5f;

        public bool IsPlaying = false;
        public Tween Tween;

        public override void _Ready()
        {
            Tween = GetNode<Tween>("Tween");
            Tween.Connect("tween_completed", this, "OnTweenCompleted");
        }

        /// <summary>
        /// Animate from the current modulate color until the node is fully transparent.
        /// </summary>
        public void FadeIn()
        {
            Tween.InterpolateProperty(
                this,
                "modulate",
                Modulate,
                Colors.Transparent,
                DurationFadeIn,
                Tween.TransitionType.Linear,
                Tween.EaseType.Out
            );
            Show();
            Tween.Start();
            IsPlaying = true;
        }

        /// <summary>
        /// Animate from the current modulate color until the node is fully black.
        /// </summary>
        public void FadeOut(bool isDelayed = false)
        {
            Tween.InterpolateProperty(
                this,
                "modulate",
                Modulate,
                Colors.White,
                DurationFadeOut,
                Tween.TransitionType.Linear,
                Tween.EaseType.Out,
                isDelayed ? DurationFadeOut : 0.0f
            );
            Show();
            Tween.Start();
            IsPlaying = true;
        }

        public void OnTweenCompleted(Godot.Object incomingObject, NodePath key)
        {
            EmitSignal("AnimationFinished");
            if (Modulate == Colors.Transparent)
                Hide();
            IsPlaying = false;
        }
    }
}
