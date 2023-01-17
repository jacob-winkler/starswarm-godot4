using Godot;
using StarSwarm.Project.Autoload;
using StarSwarm.Project.GSAI_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Ships.Guns.Projectiles
{
    public class Projectile : KinematicBody2D
    {
        private readonly List<Resource> _AUDIO_SAMPLES = new List<Resource>() {
            ResourceLoader.Load("Weapons_Plasma_Shot_01.wav"),
            ResourceLoader.Load("Weapons_Plasma_Shot_02.wav")
        };

        [Export]
        public float Speed = 1650.0F;
        [Export]
        public float Damage = 10.0F;

        public bool IsActive { get { return IsActive; } 
            set {
                IsActive = value;
                Collider.Disabled = !IsActive;
                SetPhysicsProcess(IsActive);
            }
        }
        public Vector2 Direction = Vector2.Zero;
        public Node Shooter;

        public Events Events;

        public Tween Tween;
        public Sprite Sprite;
        public AnimationPlayer Player;
        public RemoteTransform2D RemoteTransform;
        public VisibilityNotifier2D VisibilityNotifier;
        public CollisionShape2D Collider;
        public AudioStreamPlayer2D Audio;

        public override void _Ready()
        {
            Events = GetNode<Events>("/root/Events");
            Tween = GetNode<Tween>("Tween");
            Sprite = GetNode<Sprite>("Sprite");
            Player = GetNode<AnimationPlayer>("AnimationPlayer");
            RemoteTransform = GetNode<RemoteTransform2D>("DistortionTransform");
            VisibilityNotifier = GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
            Collider = GetNode<CollisionShape2D>("CollisionShape2D");
            Audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

            Direction = GSAIUtils.AngleToVector2(Rotation);
            VisibilityNotifier.Connect("screen_exited", this, "QueueFree");

            Sprite.Material = (Material)Sprite.Material.Duplicate();
            Player.Play("Flicker");

            Appear();
        }

        public override void _PhysicsProcess(float delta)
        {
            var collision = MoveAndCollide(Direction * Speed * delta);
            if (collision != null)
            {
                Events.EmitSignal("damaged", collision.Collider, Damage, Shooter);
                Die();
            }
        }

        private void Appear()
        {
            IsActive = true;
            Tween.InterpolateMethod(this, "Fade", 0.0, 1.0, 0.05f, Tween.TransitionType.Linear, Tween.EaseType.Out);
            Tween.InterpolateProperty(this, "Scale", Scale / 5.0f, Scale, 0.05f, Tween.TransitionType.Linear, Tween.EaseType.Out);
            Tween.Start();
            Audio.Stream = (AudioStream)_AUDIO_SAMPLES[(int)(GD.Randi() % _AUDIO_SAMPLES.Count())];
            Audio.Play();
        }

        private async void Die()
        {
            IsActive = false;
            Tween.InterpolateMethod(this, "Fade", 1.0, 0.0, 0.15f, Tween.TransitionType.Linear, Tween.EaseType.Out);
            Tween.Start();
            await ToSignal(Tween, "tween_all_completed");
            QueueFree();
        }

        public void Fade(float value)
        {
            ((ShaderMaterial)Sprite.Material).SetShaderParam("fade_amount", value);
        }
    }
}
