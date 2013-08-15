using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pragmatia;

namespace JackGame
{
	public class MainBehaviour : MonoBehaviour 
	{
		
		private bool DeadScreenActive = false;
		private float Track = 0;
		private float TrackMaximum = 5;
		private float TrackIncreaser = 0.1f;
		private float TimeInterval = 1;
		private float TimeCount = 0;
		
		private List<GameObject> DirectActionsButtons = new List<GameObject>();
		
		void Awake()
		{
			PgtNavigator.Start();
			PgtNavigator.LoadLevel();
			SpawnObjects();
		}
		void OnDestroy()
		{
			
		}
		
		// Use this for initialization
		void Start () 
		{
			
		}
		
		void SpawnObjects()
		{
			if (PgtNavigator.IsUserPlaying)
			{
				SpawnBarricade();
				SpawnDog();
				SpawnMainCharacter();
				SpawnTrap();
				SpawnTower();
				SpawnRainArrows();
				CreateControls();
			}
		}
		IEnumerator Spawn(Spawner spawner)
		{
			while (true)
			{
				for (int i = 0; i < spawner.getQuantity(); i++)
				{			
					//if (Track >= TrackMaximum) Track = 0; // Reset track counter
					float randomSpawnTime = Random.Range(Constants.MIN_SPAWN_WAIT_TIME, Constants.MAX_SPAWN_WAIT_TIME);
					yield return new WaitForSeconds(randomSpawnTime);
					Statics.GenerateInstance(Constants.COLLECTION_ENEMIES, spawner.getTag(), PgtNavigator.EnemiesSpawnPosition + new Vector3(0, 0, Track));
					//Track += TrackIncreaser;
				}
				yield break;
				yield return 0;
			}
		}
		
		
		// Update is called once per frame
		void Update () 
		{
			if (PgtNavigator.IsUserPlaying)
			{			
				int timeSinceLevelLoad = (int) Time.timeSinceLevelLoad;
				Spawner spawner = Statics.spawners.Get(Constants.LEVEL_1, timeSinceLevelLoad);
				if (spawner != null)
				{
					Statics.spawners.Remove(spawner);
					StartCoroutine("Spawn", spawner);
				}
				TimeCount += Time.fixedDeltaTime;
				//Enter this block each amount X of time
				if (TimeCount >= TimeInterval)
				{
					if (IsWinner()) PgtNavigator.NextLevel();
					TimeCount = 0;
				}
				
				// Check if gameover
				if (IsDefeat() && !DeadScreenActive)
				{
					PgtNavigator.GameOver();					
				}
			}
		}	
		public static void SpawnBarricade() 
		{
			// Spawn BARRICADE (Home)
			Statics.GenerateInstance(Constants.COLLECTION_ALLIES, Constants.TAG_BARRICADE,PgtNavigator.BarricadeSpawnPosition);
			//Statics.allies.GenerateInstance(AlliesCollection.BARRICADE, BarricadeSpawnPosition);
		}
		public static void SpawnDog() 
		{
			// Spawn DOG (pet)
			Statics.GenerateInstance(Constants.COLLECTION_ALLIES, Constants.TAG_DOG,PgtNavigator.DogSpawnPosition);
			//Statics.allies.GenerateInstance(AlliesCollection.BARRICADE, BarricadeSpawnPosition);
		}
		public static void SpawnTower() 
		{
			// Spawn Tower
			Statics.GenerateInstance(Constants.COLLECTION_NONE, Constants.TAG_TOWER_STRUCTURE, PgtNavigator.TowerSpawnPosition);
			//Statics.allies.GenerateInstance(AlliesCollection.BARRICADE, BarricadeSpawnPosition);
		}
		public static void SpawnMainCharacter()
		{
			// Spawn JACK
			Statics.GenerateInstance(Constants.COLLECTION_ALLIES, Constants.TAG_JACK,PgtNavigator.JackSpawnPosition); //JackSpawnPosition
			//Statics.allies.GenerateInstance(AlliesCollection.MAIN_CHARACTER, JackSpawnPosition);
		}
		public static void SpawnTrap() 
		{
			// Spwan TRAP
			Statics.GenerateInstance(Constants.COLLECTION_NONE, Constants.TAG_TRAP, PgtNavigator.TrapSpawnPosition);
		}
		public static void SpawnRainArrows()
		{
			// Spawn RAIN ARROWS
			Statics.GenerateInstance(Constants.COLLECTION_NONE, Constants.TAG_RAINARROWS, new Vector3(0,0,0));
			
			// Insert it in the main camera
			GameObject rainArrow = GameObject.FindGameObjectWithTag(Constants.TAG_RAINARROWS);			
			rainArrow.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_JACK).transform;			
			rainArrow.transform.localPosition = new Vector3 (-1000,1000,0);
			
		}
		
		public void CreateControls()
		{
			// DIRECT ACTIONS
			
			// Smashing hit
			AddDirectActionButton(Constants.ACTION_SMASHING_HIT, Constants.ACTION_SCRIPT_SMASHING_HIT, new Vector2(-11, -9.5f));
			
			// Rain arrows
			AddDirectActionButton(Constants.ACTION_RAIN_ARROWS, Constants.ACTION_SCRIPT_RAIN_ARROWS, new Vector2(-8, -9.5f));
			
			// Call dog
			AddDirectActionButton(Constants.ACTION_CALL_DOG, Constants.ACTION_SCRIPT_CALL_DOG, new Vector2(-5, -9.5f));
			
			// Mutate
			AddDirectActionButton(Constants.ACTION_MUTATE, Constants.ACTION_SCRIPT_MUTATE, new Vector2(8, -9.5f));
						
			// Bullet Time
			AddDirectActionButton(Constants.ACTION_BULLET_TIME, Constants.ACTION_SCRIPT_BULLET_TIME, new Vector2(11, -9.5f));
		}
		
		bool IsDefeat()
		{
			// Players loses when Jack or the barricade dies
			return (IsJackDead() || Statics.HomeLife <= 0);
		}
		
		bool IsWinner()
		{
			if (!Statics.characters.IsEmpty(Constants.COLLECTION_ENEMIES))
			{
				return (Statics.spawners.LevelCount(PgtNavigator.CurrentLevel) == 0 && Statics.characters.IsListEmpty(Constants.COLLECTION_ENEMIES));
			}				
			return false;
		}
		
		bool IsJackDead() 
		{
			GameObject character = GameObject.FindWithTag(Constants.TAG_JACK);
			if (character != null)
			{
				return (character.GetComponent<PgtCharacter>().IsDead());
			}
			return true;
		}
		
		bool IsBarricadeDestroyed()
		{
			GameObject character = GameObject.FindWithTag(Constants.TAG_BARRICADE);
			if (character != null)
			{
				return (character.GetComponent<PgtCharacter>().IsDead());
			}
			return true;
		}
		
		private GameObject CreateButton(string Tag, string Script, Vector2 Position)
		{
			GameObject tmp;
			tmp = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_EMPTY), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			tmp.gameObject.tag = Tag;
			tmp.gameObject.name = Tag;
			tmp.gameObject.AddComponent(Script);
			tmp.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUDCAMERA).transform;
			tmp.transform.localPosition = new Vector3(Position.x,Position.y,1);			
			return tmp;
		}
		
		private void AddDirectActionButton(string Tag, string Script, Vector2 Position)
		{
			GameObject tmp = CreateButton(Tag, Script, Position);
			this.DirectActionsButtons.Add(tmp);
		}
	}
}
