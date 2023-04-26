using Godot;
using StarSwarm.Project.UI.PlayerHUD;

namespace StarSwarm.Project.Planets
{
    public class Planet : Node2D
    {
        public OffScreenMarker Marker { get; set; } = default!;
        public WeaponAttachment? Weapon { get; set; } = default!;
        public Control Sprite { get; set; } = default!;

        private PlayerShip _playerShip { get; set; } = default!;

        public override void _Ready()
        {
            Marker = GetNode<OffScreenMarker>("OffScreenMarker");
        }

        public override void _Process(float delta)
        {
            Marker.TargetPosition = _playerShip.GlobalPosition;
        }

        public void Initialize(Control spriteInstance, PlayerShip playerShip, WeaponAttachment? weapon = null)
        {
            Weapon = weapon;
            Sprite = spriteInstance;
            spriteInstance.RectPosition = Position - GlobalPosition;

            _playerShip = playerShip;

            if(Weapon?.SmallIcon != null)
                Marker.SetIconTexture(Weapon.SmallIcon);
        }
    }
}