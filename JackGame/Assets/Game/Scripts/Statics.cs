using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class Statics 
	{
		public static int Hope;
		public static int HomeLife = Constants.HOME_LIFE;
		
		public static CharactersCollection characters = new CharactersCollection();	
		public static DamagesCollection damages = new DamagesCollection();
		public static SpawnersCollection spawners = new SpawnersCollection();
		public static WeaponsCollection weapons = new WeaponsCollection();
		public static ResistancesCollection resistances = new ResistancesCollection();
		
		// Generate the instance of the enemy		
		public static void GenerateInstance(int collection, string type, int spawnPositionX, int spawnPositionY) 
		{
			characters.GenerateInstance(collection, type, new Vector3(spawnPositionX, spawnPositionY, 0));
		}

		public static void GenerateInstance(int collection, string type, Vector3 vector) 
		{
			characters.GenerateInstance(collection, type, vector);
		}
		
	}
}