using UnityEngine;
using System.Collections;
using Pragmatia;
using System.Collections.Generic;

namespace JackGame
{
	public class ResistancesCollection
	{
		private List<Resistance> resistances = new List<Resistance>();	
		public void Add(Resistance resistance)
		{
			this.resistances.Add(resistance);
		}
		public float Get(string ObjectiveTag, string WeaponTag)
		{
			for (int i=0;i<this.resistances.Count;i++)
			{			
		
				if (
					this.resistances[i].GetObjectiveTag() == ObjectiveTag &&
					this.resistances[i].GetWeaponTag() == WeaponTag
					)
				{
					return this.resistances[i].GetPercentageAmount();
				}
			}
			return 1;
		}
				
		public void Clear ()
		{
			this.resistances.Clear();
		}
		
	}
}