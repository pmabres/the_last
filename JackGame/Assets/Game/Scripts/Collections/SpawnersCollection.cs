using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pragmatia;

namespace JackGame
{
	public class Spawner 
	{
		private int Level;
		private string Tag; // the enemy tag
		private int Second;
		private int Quantity;
	
		
		public Spawner(int Level, string Tag, int Second, int Quantity) 
		{
			this.Level = Level;
			this.Second = Second;
			this.Tag = Tag;
			this.Quantity = Quantity;
		}
		
		public int getLevel()
		{
			return this.Level;
		}
		public int getSecond()
		{
			return this.Second;
		}
		public string getTag()
		{
			return this.Tag;
		}
		public int getQuantity()
		{
			return this.Quantity;
		}
	}
	
	
	public class SpawnersCollection {
	
		private List<Spawner> spawners = new List<Spawner>();	
	
		public void Add(Spawner spawner)
		{
			this.spawners.Add(spawner);
		}
		
		public Spawner Get(int Level, int Second)
		{
			for (int i=0; i < this.spawners.Count; i++)
			{			
				if (this.spawners[i].getLevel() == Level && this.spawners[i].getSecond() == Second)
				{
					return this.spawners[i];
				}
			}
			return null;
		}
		
		public int LevelCount(int Level)
		{
			
			int b = 0;			
			for (int i = 0; i < this.spawners.Count; i++)
			{
				if (this.spawners[i].getLevel() == Level)
				{
					b++;
				}
			}
			
			return b;
		}
		
		public void Remove(Spawner spawner)
		{
			this.spawners.Remove(spawner);
		}
				
		public void Clear ()
		{
			this.spawners.Clear();
		}
		/*
		public void Spawn(int Level, int Second, int SpawnPosition)
		{
			Spawner spawner = this.Get(Level, Second);
			if (spawner != null)
			{
				this.spawners.Remove(spawner);
				for (int i = 0; i < spawner.getQuantity(); i++)
				{
			        //float randomSpawnPos = Random.Range(Constants.MIN_SPAWN_WAIT_TIME, Constants.MAX_SPAWN_WAIT_TIME);
					//MonoBehaviour.StartCoroutine(Spawn(spawner));
					//WaitForSeconds(randomSpawnTime);
					//Debug.Log(randomSpawnPos);
					//yield return new WaitForSeconds(randomSpawnTime);
					//Statics.GenerateInstance(Constants.COLLECTION_ENEMIES, spawner.getTag(), new Vector3(SpawnPosition + randomSpawnPos, 0, 0));
				}
			}
		}
		*/

		
		/*
	    public IEnumerator Spawn(Spawner spawner) {
			//WaitForSeconds(randomSpawnTime);
			float randomSpawnPos = Random.Range(Constants.MIN_SPAWN_WAIT_TIME, Constants.MAX_SPAWN_WAIT_TIME);
			yield return new WaitForSeconds(randomSpawnPos);
			Statics.GenerateInstance(Constants.COLLECTION_ENEMIES, spawner.getTag(), randomSpawnPos);
	    }
		*/
	}
}