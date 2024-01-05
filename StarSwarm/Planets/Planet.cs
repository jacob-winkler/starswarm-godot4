using System;
using System.Collections.Generic;
using Godot;
using StarSwarm.Autoload;
using StarSwarm.UI.PlayerHUD;
using StarSwarm.Ships.Player;
using StarSwarm.Weapons;
using StarSwarm.World.Spawners;
using StarSwarm.GSAI_Framework;

namespace StarSwarm.Planets;

public partial class Planet : GSAICharacterBody2D
{
    [Export]
    public float HealthMax = 100f;

    public AudioManager AudioManager { get; set; } = default!;
    public AudioManager2D AudioManager2D { get; set; } = default!;
    public SwarmWaveSpawner SwarmWaveSpawner { get; set; } = default!;
    public TweenAura Tween { get; set; } = default!;
    public Sprite2D PlanetAura { get; set; } = default!;
    public Sprite2D UpgradeIcon { get; set; } = default!;
    public Area2D ActivateResearchArea { get; set; } = default!;
    public ResearchBar ResearchBar { get; set; } = default!;
    public OffScreenMarker Marker { get; set; } = default!;
    public WeaponAttachment? Weapon { get; set; } = default!;
    public Control Sprite2D { get; set; } = default!;

    private float _researchTime = 15;
    private PlayerShip _playerShip = default!;
    private bool _activatable = false;
    private RandomNumberGenerator _rng = default!;
    private List<PackedScene> _weaponAttachments = default!;
    private float _health;

    public Planet()
    {
        _health = HealthMax;
    }

    public override void _Ready()
    {
        AudioManager = GetNode<AudioManager>("/root/AudioManager");
        AudioManager2D = GetNode<AudioManager2D>("/root/AudioManager2D");

        SwarmWaveSpawner = GetNode<SwarmWaveSpawner>("SwarmWaveSpawner");
        Tween = GetNode<TweenAura>("TweenAura");
        PlanetAura = GetNode<Sprite2D>("PlanetAura");
        UpgradeIcon = GetNode<Sprite2D>("UpgradeIcon");
        ActivateResearchArea = GetNode<Area2D>("ActivateResearchArea");
        ResearchBar = GetNode<ResearchBar>("ResearchBar");
        Marker = GetNode<OffScreenMarker>("OffScreenMarker");

        ActivateResearchArea.Connect("body_entered", new Callable(this, "OnBodyEnteredActivationRange"));
        ActivateResearchArea.Connect("body_exited", new Callable(this, "OnBodyExitedActivationRange"));
        ResearchBar.Connect("ResearchFinished", new Callable(this, "OnResearchFinished"));

        _weaponAttachments = new List<PackedScene>() {
            GD.Load("res://StarSwarm/Weapons/LightningRod/LightningRodAttachment.tscn") as PackedScene ?? throw new NullReferenceException(),
            GD.Load("res://StarSwarm/Weapons/LaserBeam/LaserBeamAttachment.tscn") as PackedScene ?? throw new NullReferenceException(),
            GD.Load("res://StarSwarm/Weapons/SpaceMine/SpaceMineAttachment.tscn") as PackedScene ?? throw new NullReferenceException(),
        };
    }

    public override void _Process(double delta)
    {
        Marker.TargetPosition = _playerShip.GlobalPosition;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (_activatable)
        {
            if (@event.IsActionPressed("research"))
            {
                ((CollisionShape2D)ActivateResearchArea.GetChild(0)).Disabled = true;
                _activatable = false;
                AudioManager2D.Play(KnownAudioStream2Ds.StartResearch, GlobalPosition + new Vector2(50, 50));
                ResearchBar.BeginResearch(_researchTime);
                SwarmWaveSpawner.StartSwarmWave(1);
            }
        }
    }

    public void Initialize(Control spriteInstance, PlayerShip playerShip, RandomNumberGenerator rng)
    {
        _rng = rng;
        Sprite2D = spriteInstance;
        spriteInstance.Position = Position - GlobalPosition + new Vector2(-50, -50);

        _playerShip = playerShip;

        RefreshHousedUpgrade();
        SwarmWaveSpawner.Initialize(this);
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
        Agent.Position = GSAIUtils.ToVector3(GlobalPosition);
    }

    public void ApplyDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        QueueFree();
    }

    private void OnResearchFinished()
    {
        if(Weapon != null)
            _playerShip.AddChild(Weapon);

        RefreshHousedUpgrade();
        ((CollisionShape2D)ActivateResearchArea.GetChild(0)).Disabled = false;
        if(ActivateResearchArea.OverlapsBody(_playerShip))
            _activatable = true;

        AudioManager.Play(KnownAudioStreams.ResearchComplete);
        SwarmWaveSpawner.StopSwarmWave();
    }

    private void OnBodyEnteredActivationRange(PhysicsBody2D playerBody)
    {
        if (_playerShip == playerBody)
        {
            _activatable = true;

            if(Tween.IsRunning())
                Tween.Pause();
            Tween.MakeAppear(PlanetAura);
        }
    }

    private void OnBodyExitedActivationRange(PhysicsBody2D playerBody)
    {
        if (_playerShip == playerBody)
        {
            _activatable = false;

            if(Tween.IsRunning())
                Tween.Pause();
            Tween.MakeDisappear(PlanetAura);
        }
    }

    private void RefreshHousedUpgrade()
    {
        Weapon = (WeaponAttachment)_weaponAttachments[_rng.RandiRange(0, _weaponAttachments.Count - 1)].Instantiate();
        if (Weapon?.SmallIcon != null)
        {
            Marker.SetIconTexture(Weapon.SmallIcon);
            UpgradeIcon.Texture = Weapon.SmallIcon;
        }
    }
}