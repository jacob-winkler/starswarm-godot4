using Godot;
using StarSwarm.Project.GSAI_Framework;
using System;

namespace StarSwarm.Project.UI.PlayerHUD
{
    public class OffScreenMarker : Node2D
    {
        [Export]
        public Texture SpriteTexture { get; set; } = default!;
        [Export]
        public Texture IconTexture { get; set; } = default!;

        public Sprite Sprite { get; set; } = default!;
        public Sprite Icon { get; set; } = default!;
        public Vector2? TargetPosition { get; set; } = default!;

        public override void _Ready()
        {
            Sprite = GetNode<Sprite>("Sprite");
            Icon = GetNode<Sprite>("Sprite/Icon");

            if (SpriteTexture != null)
            {
                var textureSize = SpriteTexture.GetSize();
                Sprite.Offset = new Vector2(-textureSize.x / 2, Sprite.Offset.y);
                Sprite.Texture = SpriteTexture;
            }

            if (IconTexture != null)
            {
                SetIconTexture(IconTexture);
            }
        }

        public override void _Process(Single delta)
        {
            var canvas = GetCanvasTransform();
            var topLeft = -canvas.origin / canvas.Scale;
            var size = GetViewportRect().Size / canvas.Scale;

            SetMarkerPosition(new Rect2(topLeft, size));
            SetMarkerRotation();
        }

        public void SetMarkerPosition(Rect2 bounds)
        {
            if (TargetPosition == null)
            {
                Sprite.GlobalPosition = new Vector2(
                    Mathf.Clamp(GlobalPosition.x, bounds.Position.x, bounds.End.x),
                    Mathf.Clamp(GlobalPosition.y, bounds.Position.y, bounds.End.y));
            }
            else
            {
                var displacement = GlobalPosition - TargetPosition.Value;
                float length;

                var tl = (bounds.Position - TargetPosition.Value).Angle();
                var tr = (new Vector2(bounds.End.x, bounds.Position.y) - TargetPosition.Value).Angle();
                var bl = (new Vector2(bounds.Position.x, bounds.End.y) - TargetPosition.Value).Angle();
                var br = (bounds.End - TargetPosition.Value).Angle();

                if (displacement.Angle() > tl && displacement.Angle() < tr ||
                    displacement.Angle() < bl && displacement.Angle() > br)
                {
                    var yLen = Mathf.Clamp(displacement.y, bounds.Position.y - TargetPosition.Value.y,
                        bounds.End.y - TargetPosition.Value.y);
                    var angle = displacement.Angle() - Mathf.Pi / 2;
                    length = Mathf.Cos(angle) != 0 ? yLen / Mathf.Cos(angle) : yLen;
                }
                else
                {
                    var xLen = Mathf.Clamp(displacement.x, bounds.Position.x - TargetPosition.Value.x,
                        bounds.End.x - TargetPosition.Value.x);
                    var angle = displacement.Angle();
                    length = Mathf.Cos(angle) != 0 ? xLen / Mathf.Cos(angle) : xLen;
                }

                Sprite.GlobalPosition = Mathf.Polar2Cartesian(length, displacement.Angle()) + TargetPosition.Value;

            }

            if(bounds.HasPoint(GlobalPosition))
                Hide();
            else
                Show();
        }

        public void SetMarkerRotation()
        {
            var angle = (GlobalPosition - Sprite.GlobalPosition).Angle();
            Sprite.GlobalRotation = angle;
            Icon.GlobalRotation = 0;
        }

        public void SetIconTexture(Texture iconTexture)
        {
            Icon.Texture = iconTexture;
        }
    }
}
