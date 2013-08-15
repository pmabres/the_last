using UnityEngine;
using System.Collections;
using Pragmatia;
using System.Collections.Generic;

namespace JackGame
{
	public class DamagesCollection 
	{
		private List<Damage> damages = new List<Damage>();	
		public void Add(Damage damage)
		{
			this.damages.Add(damage);
		}
		public int Get(string Tag, int EvolutionStatus)
		{
			for (int i=0;i<this.damages.Count;i++)
			{			
		
				if (this.damages[i].getTag() == Tag && this.damages[i].getEvolutionStatus() == EvolutionStatus )
				{
					return this.damages[i].getAmount();
				}
			}
			Debug.Log("Damage not found");
			return 0;
		}
				
		public void Clear ()
		{
			this.damages.Clear();
		}
		
	}
}