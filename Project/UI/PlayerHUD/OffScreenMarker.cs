using Godot;

namespace StarSwarm.Project.UI.PlayerHUD;

public partial class OffScreenMarker : Node2D
{
    [Export]
    public Texture2D SpriteTexture { get; set; } = default!;

    [Export]
    public Texture2D IconTexture { get; set; } = default!;

    public Sprite2D Sprite2D { get; set; } = default!;
    public Sprite2D Icon { get; set; } = default!;
    public Vector2? TargetPosition { get; set; } = default!;

    public override void _Ready()
    {
        Sprite2D = GetNode<Sprite2D>("Sprite2D");
        Icon = GetNode<Sprite2D>("Sprite2D/Icon");

        if (SpriteTexture != null)
        {
            var textureSize = SpriteTexture.GetSize();
            Sprite2D.Offset = new Vector2(-textureSize.X / 2, Sprite2D.Offset.Y);
            Sprite2D.Texture = SpriteTexture;
        }

        if (IconTexture != null)
        {
            SetIconTexture(IconTexture);
        }
    }

    public override void _Process(double delta)
    {
        var canvas = GetCanvasTransform();
        var topLeft = -canvas.Origin / canvas.Scale;
        var size = GetViewportRect().Size / canvas.Scale;

        SetMarkerPosition(new Rect2(topLeft, size));
        SetMarkerRotation();
    }

    public void SetMarkerPosition(Rect2 bounds)
    {
        if (TargetPosition == null)
        {
            Sprite2D.GlobalPosition = new Vector2(
                Mathf.Clamp(GlobalPosition.X, bounds.Position.X, bounds.End.X),
                Mathf.Clamp(GlobalPosition.Y, bounds.Position.Y, bounds.End.Y));
        }
        else
        {
            var displacement = GlobalPosition - TargetPosition.Value;
            float length;

            var tl = (bounds.Position - TargetPosition.Value).Angle();
            var tr = (new Vector2(bounds.End.X, bounds.Position.Y) - TargetPosition.Value).Angle();
            var bl = (new Vector2(bounds.Position.X, bounds.End.Y) - TargetPosition.Value).Angle();
            var br = (bounds.End - TargetPosition.Value).Angle();

            if (displacement.Angle() > tl && displacement.Angle() < tr ||
                displacement.Angle() < bl && displacement.Angle() > br)
            {
                var yLen = Mathf.Clamp(displacement.Y, bounds.Position.Y - TargetPosition.Value.Y,
                    bounds.End.Y - TargetPosition.Value.Y);
                var angle = displacement.Angle() - Mathf.Pi / 2;
                length = Mathf.Cos(angle) != 0 ? yLen / Mathf.Cos(angle) : yLen;
            }
            else
            {
                var xLen = Mathf.Clamp(displacement.X, bounds.Position.X - TargetPosition.Value.X,
                    bounds.End.X - TargetPosition.Value.X);
                var angle = displacement.Angle();
                length = Mathf.Cos(angle) != 0 ? xLen / Mathf.Cos(angle) : xLen;
            }

            Sprite2D.GlobalPosition = length * Vector2.FromAngle(displacement.Angle()) + TargetPosition.Value;
        }

        if (bounds.HasPoint(GlobalPosition))
            Hide();
        else
            Show();
    }

    public void SetMarkerRotation()
    {
        var angle = (GlobalPosition - Sprite2D.GlobalPosition).Angle();
        Sprite2D.GlobalRotation = angle;
        Icon.GlobalRotation = 0;
    }

    public void SetIconTexture(Texture2D iconTexture) => Icon.Texture = iconTexture;
}