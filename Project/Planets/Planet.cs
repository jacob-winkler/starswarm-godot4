using Godot;

namespace StarSwarm.Project.Planets
{
    public class Planet : Node2D
    {
        public WeaponAttachment? Weapon { get; set; } = default!;
        public Control Sprite { get; set; } = default!;

        public void Initialize(Control spriteInstance, WeaponAttachment? weapon = null)
        {
            Weapon = weapon;
            Sprite = spriteInstance;
            spriteInstance.RectPosition = Position - GlobalPosition;
        }
    }
}