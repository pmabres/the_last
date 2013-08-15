using UnityEngine;
using System.Collections;
using JackGame;

namespace Pragmatia
{
	public static class PgtNavigator 
	{
		public static int CurrentLevel = 0;
		public static bool IsUserPlaying = false;
		public static Vector3 EnemiesSpawnPosition = new Vector3();
		public static Vector3 BarricadeSpawnPosition = new Vector3();
		public static Vector3 JackSpawnPosition = new Vector3();
		public static Vector3 DogSpawnPosition = new Vector3();
		public static Vector3 TrapSpawnPosition = new Vector3();
		public static Vector3 BossSpawnPosition = new Vector3();
		public static Vector3 TowerSpawnPosition = new Vector3();
		
		public static void Start()
		{			
			if (!IsUserPlaying) NextLevel();
		}
		public static void NextLevel()
		{					
			Application.LoadLevel("loading");
			CurrentLevel += 1;
			if(Application.GetStreamProgressForLevel("level" + CurrentLevel.ToString()) == 1)
			{				
    			Application.LoadLevel("level" + CurrentLevel.ToString());
			}				
			IsUserPlaying = true;
		}
		public static void LoadLevel()
		{
			CleanLevel();
			LoadLevelStructure();
		}
		public static void GameOver()
		{
			Application.LoadLevel("gameover");
		}
		
		public static void CleanLevel()
		{
			Statics.characters.Clear();
			Statics.damages.Clear();
			Statics.spawners.Clear();
			Statics.weapons.Clear();
		}
		
		public static void LoadLevelStructure()
		{			
			InitialSetup();
			// Here will be loaded the current level settings, amount of monsters, boss, etc.
			switch (PgtNavigator.CurrentLevel)
			{
				case 1:
					// Default positions (restore if changed it; update if modified)
					BarricadeSpawnPosition.Set(35,Constants.FLOOR_Y_POSITION,0);	// (35,Constants.FLOOR_Y_POSITION,0)
					DogSpawnPosition.Set(20,Constants.FLOOR_Y_POSITION,0);			// (20,Constants.FLOOR_Y_POSITION,0)
					JackSpawnPosition.Set(50,Constants.FLOOR_Y_POSITION,0);			// (50,Constants.FLOOR_Y_POSITION,0)
					TrapSpawnPosition.Set(220,Constants.FLOOR_Y_POSITION,0);		// (220,Constants.FLOOR_Y_POSITION,0)
					BossSpawnPosition.Set(400,Constants.FLOOR_Y_POSITION,0);		// (400,Constants.FLOOR_Y_POSITION,0)
					EnemiesSpawnPosition.Set(400,Constants.FLOOR_Y_POSITION,0);		// (400,Constants.FLOOR_Y_POSITION,0)
					TowerSpawnPosition.Set(150,Constants.FLOOR_Y_POSITION + 5,0); 	// (150,Constants.FLOOR_Y_POSITION + 5,0)
					
					// Spawners
					// Level, Character, Seconds, Quantity
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_UNDEADCOMMON, 1, 15));
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_SKELETONCOMMON, 15, 15));	
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_UNDEADCOMMON, 40, 25));					
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_SKELETONCOMMON, 65, 25));
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_UNDEADCOMMON, 80, 40));
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_UNDEADEXPLODER, 80, 2));	
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_SKELETONCOMMON, 85, 40));
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_EXECUTIONER, 150, 1));					
					
					break;
				case 2:					
					BarricadeSpawnPosition.Set(20,0,0);
					JackSpawnPosition.Set(10.3f,0.5f,0);
					TrapSpawnPosition.Set(120,-1f,0);
					BossSpawnPosition.Set(Constants.SCREEN_WIDTH_METERS + Constants.SCREEN_RIGHT_MARGIN_METERS,0,0);
					EnemiesSpawnPosition.Set(45,0,0);	//140
					TowerSpawnPosition.Set(25,8,0); // 50
					
				// Spawners
					// Level, Character, Seconds, Quantity
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_UNDEADCOMMON, 1, 10));
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_EXECUTIONER, 3, 2));
					Statics.spawners.Add(new Spawner(Constants.LEVEL_1, Constants.TAG_UNDEADEXPLODER, 8, 5));
					break;
			}
		}
		public static void InitialSetup()
		{			
			// Jack attacks
			Statics.weapons.Add(Constants.TAG_JACK, Constants.TAG_ARROW, Constants.ATTACK_LONG);
			Statics.weapons.Add(Constants.TAG_JACK, Constants.TAG_SHOTGUN, Constants.ATTACK_MEDIUM);
			Statics.weapons.Add(Constants.TAG_JACK, Constants.TAG_AXES, Constants.ATTACK_SHORT);	
			
			// Dog attacks
			Statics.weapons.Add(Constants.TAG_DOG, Constants.TAG_BITE, Constants.ATTACK_SHORT);	
			
			// Undead common attacks
			Statics.weapons.Add(Constants.TAG_UNDEADCOMMON, Constants.TAG_CLAWS, Constants.ATTACK_SHORT);		
			
			// Skeleton common attacks
			Statics.weapons.Add(Constants.TAG_SKELETONCOMMON, Constants.TAG_CLAWS, Constants.ATTACK_SHORT);		
			
			// Executioner boss attacks
			Statics.weapons.Add(Constants.TAG_EXECUTIONER, Constants.TAG_EXECUTIONER_AXE, Constants.ATTACK_SHORT);		
			
			// Undead exploder attacks
			Statics.weapons.Add(Constants.TAG_UNDEADEXPLODER, Constants.TAG_EXPLOSION, Constants.ATTACK_SHORT);		
			
			// Trap Weapon
			Statics.weapons.Add(Constants.TAG_TRAP, Constants.TAG_TRAP_WEAPON, Constants.ATTACK_SHORT);		
			
			// Tower arrows Weapon
			Statics.weapons.Add(Constants.TAG_TOWER_ARROWS, Constants.TAG_ARROW_TOWER, Constants.ATTACK_LONG);		
			
			// Tower rocks Weapon
			Statics.weapons.Add(Constants.TAG_TOWER_ROCKS, Constants.TAG_ROCK, Constants.ATTACK_LONG);		
			
			// Tower bodies Weapon
			Statics.weapons.Add(Constants.TAG_TOWER_BODIES, Constants.TAG_BODY_PROJECTILE, Constants.ATTACK_LONG);		
			
			// Body weapon
			Statics.weapons.Add(Constants.TAG_BODY, Constants.TAG_BODY_POISON, Constants.ATTACK_SHORT);		
			
			// Direct Action Rain Arrows weapon
			Statics.weapons.Add(Constants.TAG_RAINARROWS, Constants.TAG_ARROW_TOWER, Constants.ATTACK_LONG);
			
			// DAMAGES 
			
			// Normal
			Statics.damages.Add (new Damage(Constants.TAG_ARROW,MainCharacter.EVOLUTION_STATUS_NORMAL,5));
			Statics.damages.Add (new Damage(Constants.TAG_SHOTGUN,MainCharacter.EVOLUTION_STATUS_NORMAL,10));
			Statics.damages.Add (new Damage(Constants.TAG_AXES,MainCharacter.EVOLUTION_STATUS_NORMAL,20));
			Statics.damages.Add (new Damage(Constants.TAG_SMASHING_HIT,MainCharacter.EVOLUTION_STATUS_NORMAL, 175));
			
			Statics.damages.Add (new Damage(Constants.TAG_EXECUTIONER_AXE,MainCharacter.EVOLUTION_STATUS_NORMAL,100));
			Statics.damages.Add (new Damage(Constants.TAG_EXPLOSION,MainCharacter.EVOLUTION_STATUS_NORMAL,50));
			Statics.damages.Add (new Damage(Constants.TAG_ROCK,MainCharacter.EVOLUTION_STATUS_NORMAL, 1000));
			Statics.damages.Add (new Damage(Constants.TAG_BODY_PROJECTILE,MainCharacter.EVOLUTION_STATUS_NORMAL, 0));
			Statics.damages.Add (new Damage(Constants.TAG_BODY_POISON,MainCharacter.EVOLUTION_STATUS_NORMAL, 15));
			Statics.damages.Add (new Damage(Constants.TAG_BITE,MainCharacter.EVOLUTION_STATUS_NORMAL, 3));
			
			// Evolution level 1
			Statics.damages.Add (new Damage(Constants.TAG_ARROW, 1, 15));
			Statics.damages.Add (new Damage(Constants.TAG_SHOTGUN, 1, 25));
			Statics.damages.Add (new Damage(Constants.TAG_AXES, 1, 35));
			Statics.damages.Add (new Damage(Constants.TAG_CLAWS, 1, 20));
			Statics.damages.Add (new Damage(Constants.TAG_SMASHING_HIT, 1, 200));
			
			// Tower arrows
			Statics.damages.Add (new Damage(Constants.TAG_ARROW_TOWER, 0, 5));
			Statics.damages.Add (new Damage(Constants.TAG_ARROW_TOWER, 1, 10));
			Statics.damages.Add (new Damage(Constants.TAG_ARROW_TOWER, 2, 15));
			Statics.damages.Add (new Damage(Constants.TAG_ARROW_TOWER, 3, 25));
			Statics.damages.Add (new Damage(Constants.TAG_ARROW_TOWER, 4, 35));

			
			// Trap damages
			Statics.damages.Add (new Damage(Constants.TAG_TRAP_WEAPON, Constants.TRAP_ATTACK_LEVEL1, 1));
			Statics.damages.Add (new Damage(Constants.TAG_TRAP_WEAPON, Constants.TRAP_ATTACK_LEVEL2, 5));
			Statics.damages.Add (new Damage(Constants.TAG_TRAP_WEAPON, Constants.TRAP_ATTACK_LEVEL3, 10));
			
			// RESISTANCES
			
			// Monsters Resistances for weapons
			//Statics.resistances.Add (new Resistance(Constants.TAG_UNDEADCOMMON,Constants.TAG_ARROW, 0.5f));
			
			GameObject characters = new GameObject();
			characters.name = Constants.TAG_CHARACTERS;
			characters.tag = Constants.TAG_CHARACTERS;
			characters.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_MAIN).transform;
			GameObject hud = new GameObject();
			hud.name = Constants.TAG_HUD;
			hud.tag = Constants.TAG_HUD;
			hud.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_MAIN).transform;		
		}
		public static void RestartLevel()
		{
			
		}
		
		public static void Exit()
		{
			Application.Quit();
		}		
	}
}