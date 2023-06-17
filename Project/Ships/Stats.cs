using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarSwarm.Project.Ships
{
	/// <summary>
	/// Virtual base class for stats (health, speed...) that support upgrades.
	/// 
	/// You must call `initialize()` to initialize the stats' values. This ensures that they are in sync
	/// with the values modified in Godot's inspector.
	/// 
	/// Each stat should be a floating point value, and we recommend to make them private properties, as
	/// they should be read-only. To get a stat's calculated value, with modifiers, see `get_stat()`.
	/// </summary>
	public partial class Stats : Resource
	{
		[Signal]
		public delegate void StatChangedEventHandler(String statName, float oldValue, float newValue);

		// Stores a cached array of property names that are stats as strings, that we use to find and
		// calculate the stats with upgrades from the base stats.
		protected Dictionary<string, string> _statsList;

		// Modifiers has a list of modifiers for each property in `_stats_list`. A modifier is a dict that
		// requires a key named `value`. The value of a modifier can be positive or negative.
		protected Dictionary<string, List<float>> _modifiers = new Dictionary<string, List<float>>();

		// Stores the cached values for the computed stats
		protected Dictionary<string, float> _cache = new Dictionary<string, float>();


		// Initializes the keys in the modifiers dict, ensuring they all exist, without going through the
		// property's setter.
		public Stats()
		{
			_statsList = GetStatsList();
			foreach (var stat in _statsList)
			{
				_modifiers[stat.Key] = new List<float>();
				_cache[stat.Key] = 0.0F;

			}
		}

		public void Initialize()
        {
			UpdateAll();
        }

		public float GetStat(string statName = "")
        {
			Debug.Assert(_cache.TryGetValue(statName, out var statValue), $"Failed to retrieve stat: {statName}");
			return statValue;
        }

		public int AddModifier(string statName, float modifier)
        {
            Debug.Assert(_statsList.TryGetValue(statName, out var statValue), $"Failed to add modifier. Couldn't find stat:{statName}");
            _modifiers[statName].Add(modifier);
			Update(statName);
			return _modifiers.Count();
        }

		public void RemoveModifier(string statName, int id)
        {
			Debug.Assert(_statsList.TryGetValue(statName, out var statValue), $"Failed to remove modifier. Couldn't find stat");
			_modifiers[statName].RemoveAt(id);
			Update(statName);
        }

		public void PopModifier(string statName)
        {
			Debug.Assert(_statsList.TryGetValue(statName, out var statValue), $"Failed to pop modifier. Couldn't find stat");
			_modifiers[statName].RemoveAt(_modifiers.Count - 1);
			Update(statName);
		}

		public void Reset()
        {
			_modifiers = new Dictionary<string, List<float>>();
			UpdateAll();
		}

		public void Update(String statName = "")
        {
            float valueStart = Get(_statsList[statName]).AsSingle();
			var value = valueStart;
			foreach (var modifier in _modifiers[statName])
				value += modifier;

			_cache[statName] = value;
			EmitSignal("StatChanged", statName, valueStart, value);
        }

		public void UpdateAll()
        {
			foreach (var statName in _statsList.Keys)
				Update(statName);
        }

        public Dictionary<string, string> GetStatsList()
        {
			var ignore = new List<string> {
				"resource_local_to_scene",
				"resource_name",
				"resource_path",
				"script",
				"_statsList",
				"_modifiers",
				"_cache"
			};

			var stats = new Dictionary<string, string>();
			foreach(Godot.Collections.Dictionary property in GetPropertyList())
			{
				if (property["name"].ToString()[0].ToString().Capitalize() == property["name"].ToString()[0].ToString())
					continue;
				if (ignore.Contains(property["name"].ToString()))
					continue;

				stats[property["name"].ToString().TrimStart('_')] = property["name"].ToString();
			}

			return stats;
        }
	}
}
	

