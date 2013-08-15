using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pragmatia;

namespace JackGame
{
	public class WeaponsCollection
	{
		private Dictionary<string, List<WeaponRanges>> Instances = new Dictionary<string, List<WeaponRanges>>();
		public void Add(string tag, string weaponTag, int attackRange)
		{
			if (!Instances.ContainsKey(tag))
			{			
			    Instances.Add(tag, new List<WeaponRanges>());
			}
			WeaponRanges weaponRange = new WeaponRanges(weaponTag, attackRange);
			Instances[tag].Add(weaponRange);
		}
		//This Method finds which weapon is associated to which range for determinated GameObject Tag
		public string GetWeapon(string tag, int attackRange)
		{
			string weaponFound = null;
			if (Instances.ContainsKey(tag))
			{
				foreach (WeaponRanges weaponRanges in Instances[tag])
				{
					if (weaponRanges.getAttackRange() == attackRange)
					{
						weaponFound = weaponRanges.getWeapon();
					}
				}
			}				
			return weaponFound;		
		}
				
		public void Clear ()
		{
			this.Instances.Clear();
		}
	}
}