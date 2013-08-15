using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public struct Damage 
	{
		private string Tag;
		private int EvolutionStatus;
		private int Amount; // The amount of damage 
		
		public Damage(string Tag,int EvolutionStatus, int Amount) 
		{
			this.Tag = Tag;
			this.EvolutionStatus = EvolutionStatus;
			this.Amount = Amount;
		}
		
		public string getTag()
		{
			return this.Tag;
		}
		public int getEvolutionStatus()
		{
			return this.EvolutionStatus;
		}
		public int getAmount()
		{
			return this.Amount;
		}
	}
	public struct Resistance
	{
		private string ObjectiveTag;
		private string WeaponTag;
		private float PercentageAmount;
		
		public Resistance ( string ObjectiveTag, string WeaponTag, float PercentageAmount)
		{
			this.ObjectiveTag = ObjectiveTag;
			this.WeaponTag = WeaponTag;
			this.PercentageAmount = PercentageAmount;
		}
		
	
		public float GetPercentageAmount()
		{
			return this.PercentageAmount;
		}
		public void SetPercentageAmount(float PercentageAmount)
		{
			this.PercentageAmount = PercentageAmount;
		}
		public string GetWeaponTag()
		{
			return this.WeaponTag;
		}
		public void SetWeaponTag(string WeaponTag)
		{
			this.WeaponTag = WeaponTag;
		}
		public string GetObjectiveTag()
		{
			return this.ObjectiveTag;
		}
		public void SetObjectiveTag(string ObjectiveTag)
		{
			this.ObjectiveTag = ObjectiveTag;
		}
		
	}
	public struct WeaponRanges
	{
		private string Weapon;
		private int AttackRange;
		public WeaponRanges(string Weapon,int AttackRange)
		{
			this.Weapon = Weapon;
			this.AttackRange = AttackRange;
		}
		public string getWeapon()
		{
			return this.Weapon;
		}
		public int getAttackRange()
		{
			return this.AttackRange;
		}
	}
	
	public class Constants 
	{
		// Levels
		public const int LEVEL_1 = 1;
		public const int LEVEL_2 = 2;
		public const int LEVEL_3 = 3;
		
		// Characters Tags
		public const string TAG_JACK = "Jack";	
		public const string TAG_DOG = "Dog";
		public const string TAG_ENEMY = "Enemy";	
		public const string TAG_UNDEADCOMMON = "UndeadCommon";
		public const string TAG_SKELETONCOMMON = "SkeletonCommon";
		public const string TAG_EXECUTIONER = "Executioner";
		public const string TAG_BARRICADE = "Barricade";
		public const string TAG_MAIN = "Main";	
		public const string TAG_TRAP = "Trap";
		public const string TAG_RAINARROWS = "RainArrows";
		public const string TAG_UNDEADEXPLODER = "UndeadExploder";
		public const string TAG_TOWER = "Tower";
		public const string TAG_TOWER_ARROWS = "TowerArrows";
		public const string TAG_TOWER_ROCKS = "TowerRocks";
		public const string TAG_TOWER_BODIES = "TowerBodies";
		public const string TAG_TOWER_STRUCTURE = "TowerStructure"; // The real body of the tower
		public const string TAG_BODY = "Body";
		
		
		// Weapons Tags
		public const string TAG_ARROW = "ArrowProjectile";
		public const string TAG_SHOTGUN = "Shotgun";
		public const string TAG_AXES = "Axes";
		public const string TAG_EXECUTIONER_AXE = "ExecutionerAxe";
		public const string TAG_EXPLOSION = "Explosion";
		public const string TAG_TRAP_WEAPON = "TrapWeapon";
		public const string TAG_ARROW_TOWER = "ArrowTowerProjectile";
		public const string TAG_ROCK = "Rock";
		public const string TAG_BODY_PROJECTILE = "BodyProjectile"; // Only a flying projectile
		public const string TAG_BODY_POISON = "BodyPoison"; // The real weapon
		public const string TAG_BITE = "Bite"; // Dog bite
		public const string TAG_SMASHING_HIT = "SmashingHit"; // Direct Action attack
		public const string TAG_CLAWS = "Claws";
		
		// Items
		public const string TAG_HOPE = "Hope";
		
		// CONTAINERS
		public const string TAG_CHARACTERS = "Characters";
		public const string TAG_HUD = "Hud";
		public const string TAG_ENVIRONMENT = "Environment";
		
		// Hud CAMERA
		public const string TAG_HUDCAMERA = "HudCamera";
		
		//Minimum time to wait before spawning a random object (in seconds)
		public const float MIN_SPAWN_WAIT_TIME = 2.0f;
		//max time to wait before spawning a random object (in seconds)
		public const float MAX_SPAWN_WAIT_TIME = 4.0f;
		
		// Directions
		public const int LEFT = -1;
		public const int RIGHT = 1;
		public const int MOVE_LEFT = LEFT;
		public const int MOVE_RIGHT = RIGHT;
		
		// Screen (pixels)
		public const int SCREEN_WIDTH = 20480;
		public const int SCREEN_HEIGHT = 1536;
		
		// Screen (meters)
		public const float SCREEN_WIDTH_METERS = 409.6f;
		public const float SCREEN_HEIGHT_METERS = 21.4f;
		public const float SCREEN_LEFT_MARGIN_METERS = 20f;
		public const float SCREEN_RIGHT_MARGIN_METERS = 60f;
		
		// Attacks types
		public const int ATTACK_NONE = 0;
		public const int ATTACK_SHORT = 1;
		public const int ATTACK_MEDIUM = 2;
		public const int ATTACK_LONG = 3;
		public const int ATTACK_SPECIAL_1 = 4;
		public const int ATTACK_SPECIAL_2 = 5;
		public const int ATTACK_SPECIAL_3 = 6;
		
		// Collections names
		public const int COLLECTION_NONE = 0;
		public const int COLLECTION_ALLIES = 1;
		public const int COLLECTION_ENEMIES = 2;
		
		public const string ATTACK_SHORT_NAME_JACK = "Axe";
		public const string ATTACK_MEDIUM_NAME_JACK = "Shotgun";
		public const string ATTACK_LONG_NAME_JACK = "Bow";
		
		public const float ATTACK_RANGE_LONG = 60;
		public const float ATTACK_RANGE_MEDIUM = 13;
		public const float ATTACK_RANGE_SHORT = 4;
	
		// Trap levels
		public const int TRAP_NONE = 0;
		public const int TRAP_ATTACK_LEVEL1 = 1;
		public const int TRAP_ATTACK_LEVEL2 = 2;
		public const int TRAP_ATTACK_LEVEL3 = 3;
		public const int TRAP_ATTACK_MAXLEVEL = 3;
		public const int TRAP_SPEED_LEVEL1 = 1;
		public const int TRAP_SPEED_LEVEL2 = 2;
		public const int TRAP_SPEED_LEVEL3 = 3;
		public const int TRAP_SPEED_MAXLEVEL = 3;
		
		// Tower types
		public const int TOWER_ARROWS = 1;
		public const int TOWER_ROCKS = 2;
		public const int TOWER_BODIES = 3;
		
		// Barricade Levels
		public const int BARRICADE_MAXLEVEL = 5;
		
		// Jack max evolution level
		public const int JACK_MAX_EVOLUTION_LEVEL = 1;
		
		// Jack animations (normal)
		public const string ATTACK_SHORT_ANIMATION_JACK = "AxeCombo";
		public const string ATTACK_MEDIUM_ANIMATION_JACK = "ShotgunCombo";
		public const string ATTACK_LONG_ANIMATION_JACK = "BowCombo";
		public const string ATTACK_SMASHINGHIT_ANIMATION_JACK = "SmashingHit";
		public const string WALK_RIGHT_ANIMATION_JACK = "Runs";
		public const string WALK_LEFT_ANIMATION_JACK = "WalksBack";
		public const string RUN_LEFT_ANIMATION_JACK = "RunsBack";
		public const string BRAKES_LEFT_ANIMATION_JACK = "BrakesTurns";
		public const string BRAKES_RIGHT_ANIMATION_JACK = "Brakes";
		
		// Jack animations (mutation level 1)
		public const string EVOLUTION_1_ATTACK_SHORT_ANIMATION_JACK = "AxeCombo";
		public const string EVOLUTION_1_ATTACK_MEDIUM_ANIMATION_JACK = "ShotgunCombo";
		public const string EVOLUTION_1_ATTACK_LONG_ANIMATION_JACK = "BowCombo";
		public const string EVOLUTION_1_ATTACK_SMASHINGHIT_ANIMATION_JACK = "SmashingHit";
		public const string EVOLUTION_1_WALK_RIGHT_ANIMATION_JACK = "Runs";
		public const string EVOLUTION_1_WALK_LEFT_ANIMATION_JACK = "WalksBack";
		public const string EVOLUTION_1_RUN_LEFT_ANIMATION_JACK = "RunsBack";
		public const string EVOLUTION_1_STANDBY_ANIMATION = "Standby";
		public const string EVOLUTION_1_STUN_ANIMATION = "Damage";
		public const string EVOLUTION_1_DIE_ANIMATION = "Dies";
		public const string EVOLUTION_1_BRAKES_LEFT_ANIMATION_JACK = "BrakesTurns";
		public const string EVOLUTION_1_BRAKES_RIGHT_ANIMATION_JACK = "Brakes";
		
		// Jack animations (mutation level 1)
		public const string EVOLUTION_2_ATTACK_SHORT_ANIMATION_JACK = "AxeCombo";
		public const string EVOLUTION_2_ATTACK_MEDIUM_ANIMATION_JACK = "ShotgunCombo";
		public const string EVOLUTION_2_ATTACK_LONG_ANIMATION_JACK = "BowCombo";
		public const string EVOLUTION_2_ATTACK_SMASHINGHIT_ANIMATION_JACK = "SmashingHit";
		public const string EVOLUTION_2_WALK_RIGHT_ANIMATION_JACK = "Runs";
		public const string EVOLUTION_2_WALK_LEFT_ANIMATION_JACK = "WalksBack";
		public const string EVOLUTION_2_RUN_LEFT_ANIMATION_JACK = "RunsBack";
		public const string EVOLUTION_2_STANDBY_ANIMATION = "Standby";
		public const string EVOLUTION_2_STUN_ANIMATION = "Damage";
		public const string EVOLUTION_2_DIE_ANIMATION = "Dies";
		public const string EVOLUTION_2_BRAKES_LEFT_ANIMATION_JACK = "BrakesTurns";
		public const string EVOLUTION_2_BRAKES_RIGHT_ANIMATION_JACK = "Brakes";
		
		// Dog animations
		public const string WALK_RIGHT_ANIMATION_DOG = "Walk";
		public const string ATTACK_SHORT_ANIMATION_DOG = "Attack";
		public const string STANDBY_ANIMATION_DOG = "StandBy";
		
		// Standard animations
		public const string STUN_ANIMATION = "Damage";
		public const string STANDBY_ANIMATION = "Standby";
		public const string DIE_ANIMATION = "Dies";
		
		// Sprite Fonts Legacy Text
		public const string DISPLAY_TEXT_HEALTH = "Osaka";		
		public const int DISPLAY_TEXT_HEALTH_TIME = 2;
		
		public const float GRAVITY_FORCE = 0.05f;
		
		// GUI elements
		public const string BUTTON_EMPTY = "ButtonEmpty";
		public const string BUTTON_TRAP_ATTACK_LEVEL = "ButtonTrapAttackLevel";
		public const string BUTTON_TRAP_SPEED_LEVEL = "ButtonTrapSpeedLevel";
		public const string BUTTON_BARRICADE_LEVEL = "ButtonBarricadeLevel";
		public const string BUTTON_RESTART = "ButtonRestart";
		public const string BUTTON_CREATE_TOWER_ARROWS = "ButtonCreateTowerArrows";
		public const string BUTTON_CREATE_TOWER_BODIES = "ButtonCreateTowerBodies";
		public const string BUTTON_CREATE_TOWER_ROCKS = "ButtonCreateTowerRocks";
		public const string BUTTON_REMOVE_TOWER_ARROWS = "ButtonRemoveTowerArrows";
		public const string BUTTON_REMOVE_TOWER_BODIES = "ButtonRemoveTowerBodies";
		public const string BUTTON_REMOVE_TOWER_ROCKS = "ButtonRemoveTowerRocks";
		public const string BUTTON_REMOVE_TOWER = "ButtonRemoveTower";
		
		// Direct Actions
		public const string ACTION_SMASHING_HIT = "ActionSmashingHit";
		public const string ACTION_RAIN_ARROWS = "ActionRainArrows";
		public const string ACTION_CALL_DOG = "ActionCallDog";
		public const string ACTION_MUTATE = "ActionMutate";
		public const string ACTION_BULLET_TIME = "ActionBulletTime";
		
		// Direct Actions Script
		public const string ACTION_SCRIPT_SMASHING_HIT = "ActionSmashingHitBehaviour";
		public const string ACTION_SCRIPT_RAIN_ARROWS = "ActionRainArrowsBehaviour";
		public const string ACTION_SCRIPT_CALL_DOG = "ActionCallDogBehaviour";
		public const string ACTION_SCRIPT_MUTATE = "ActionMutateBehaviour";
		public const string ACTION_SCRIPT_BULLET_TIME = "ActionBulletTimeBehaviour";
		
		// Weapon trajectory Type
		public const int TRAJECTORY_LINEAR = 0;
		public const int TRAJECTORY_PARABOLIC = 1;
		// Hope times
		public const float HOPE_DESTROY_TIME = 5f;
		public const float HOPE_ALERT_TIME = 3f;
		
		public const int HOME_LIFE = 100;
		
		public const int CAMERA_GUI_Z = 1;
		
		public const int CAMERA_STATUS_IDLE = 0;
		public const int CAMERA_STATUS_MOVING = 1;
		public const int CAMERA_STATUS_ZOOMING = 2;
		
		public const float FLOOR_Y_POSITION = 8;
	}
}