using Godot;
using System;

public class PlayerSpawner : Node2D
{
    public PlayerShip PlayerShip;


    public override void _Ready()
    {
        PlayerShip = GetNode<PlayerShip>("PlayerShip");
    }

    public void SpawnPlayer(RandomNumberGenerator rng)
    {
        var station := Station.instance()
        add_child(station)
        player_ship.global_position = (
            station.global_position
            + (Vector2.UP.rotated(rng.randf_range(0, PI * 2)) * radius_player_near_station)
        )
        Events.emit_signal("station_spawned", station, player_ship)
    }

    func get_player() -> PlayerShip:
        return player_ship
}
