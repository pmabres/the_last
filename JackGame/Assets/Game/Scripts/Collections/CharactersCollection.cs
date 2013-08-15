using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pragmatia;

namespace JackGame
{
	public class CharactersCollection 
	{
		public Dictionary<int, List<GameObject>> Instances = new Dictionary<int, List<GameObject>>();
		
		// Generate the instance of a character in a group
		public void GenerateInstance(int collection, string type, Vector3 position) 
		{
			if (!Instances.ContainsKey(collection))
			{			
			    Instances.Add(collection, new List<GameObject>());
			}
			GameObject character;
			character = MonoBehaviour.Instantiate(Resources.Load("Prefabs/"+type, typeof(GameObject)), position, Quaternion.identity) as GameObject;
			this.Instances[collection].Add(character);
			character.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_CHARACTERS).transform;
		}
		
		public GameObject GetObject(int collection, int position) 
		{
			return this.Instances[collection][position];
		}
		
		public void Remove(int collection, GameObject obj)
		{
			this.Instances[collection].Remove(obj);
		}
		
		public void Clear ()
		{
			foreach (var item in Instances)
			{
				foreach (GameObject go in item.Value)
				{
					GameObject.Destroy(go);	
				}
			}
			this.Instances.Clear();
		}
		
		public bool IsEmpty(int collection)
		{
			return !Instances.ContainsKey(collection);
		}			
		
		public bool IsListEmpty(int collection)
		{
			return Instances[collection].Count == 0;
		}
	}
}