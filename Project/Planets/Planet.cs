using System;
using System.Collections.Generic;
using Godot;
using StarSwarm.Project.UI.PlayerHUD;

namespace StarSwarm.Project.Planets
{
    public class Planet : Node2D
    {
        public Sprite UpgradeIcon { get; set; } = default!;
        public Area2D ActivateResearchArea { get; set; } = default!;
        public ResearchBar ResearchBar { get; set; } = default!;
        public OffScreenMarker Marker { get; set; } = default!;
        public WeaponAttachment? Weapon { get; set; } = default!;
        public Control Sprite { get; set; } = default!;

        private float _researchTime = 5;
        private PlayerShip _playerShip { get; set; } = default!;
        private Boolean _activatable = false;
        private RandomNumberGenerator _rng = default!;

        private List<PackedScene> _weaponAttachments { get; set; } = default!;

        public override void _Ready()
        {
            UpgradeIcon = GetNode<Sprite>("UpgradeIcon");
            ActivateResearchArea = GetNode<Area2D>("ActivateResearchArea");
            ResearchBar = GetNode<ResearchBar>("ResearchBar");
            Marker = GetNode<OffScreenMarker>("OffScreenMarker");

            ActivateResearchArea.Connect("body_entered", this, "OnBodyEnteredActivationRange");
            ActivateResearchArea.Connect("body_exited", this, "OnBodyExitedActivationRange");
            ResearchBar.Connect("ResearchFinished", this, "OnResearchFinished");

            _weaponAttachments = new List<PackedScene>() {
                GD.Load("res://Project/Weapons/LightningRod/LightningRodAttachment.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Weapons/LaserBeam/LaserBeamAttachment.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Weapons/SpaceMine/SpaceMineAttachment.tscn") as PackedScene ?? throw new NullReferenceException(),
            };
        }

        public override void _Process(float delta)
        {
            Marker.TargetPosition = _playerShip.GlobalPosition;
        }

        public override void _UnhandledInput(InputEvent @event)
        { 
            if(_activatable)
            {
                if(@event.IsActionPressed("research"))
                {
                    ((CollisionShape2D)ActivateResearchArea.GetChild(0)).Disabled = true;
                    _activatable = false;
                    ResearchBar.BeginResearch(_researchTime);
                }
            }
        }

        public void Initialize(Control spriteInstance, PlayerShip playerShip, RandomNumberGenerator rng)
        {
            _rng = rng;
            Sprite = spriteInstance;
            spriteInstance.RectPosition = Position - GlobalPosition;

            _playerShip = playerShip;

            RefreshHousedUpgrade();
        }

        private void OnResearchFinished()
        {
            if(Weapon != null)
                _playerShip.AddChild(Weapon);

            RefreshHousedUpgrade();
            ((CollisionShape2D)ActivateResearchArea.GetChild(0)).Disabled = false;
            if(ActivateResearchArea.OverlapsBody(_playerShip))
                _activatable = true;
        }

        private void OnBodyEnteredActivationRange(PhysicsBody2D playerBody)
        {
            if (_playerShip == playerBody)
            {
                _activatable = true;
            }
        }

        private void OnBodyExitedActivationRange(PhysicsBody2D playerBody)
        {
            if (_playerShip == playerBody)
            {
                _activatable = false;
            }
        }

        private void RefreshHousedUpgrade()
        {
            Weapon = (WeaponAttachment)_weaponAttachments[_rng.RandiRange(0, _weaponAttachments.Count - 1)].Instance();
            if (Weapon?.SmallIcon != null)
            {
                Marker.SetIconTexture(Weapon.SmallIcon);
                UpgradeIcon.Texture = Weapon.SmallIcon;
            }
        }
    }
}