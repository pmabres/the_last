using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class TowerArrowsBehaviour : TowerBehaviour
	{		
		private int QuantityToRain; // Arrows needed to be shooted until the rain
		private int QuantityPerRain; // Arrow quantity per rain
		private int QuantityInRainCounter; // Arrows shooted in the rain
		private int Counter = 0;
		
		public void Awake() 
		{
			base.Awake();
			SetQuantityToRain(3);
			SetQuantityPerRain(10);
			SetWeaponsPerAttack(1);
			SetAutoAttack(true);
			SetAttackRangeMedium(10); // From here the tower can't attack
		}
		
		// Update is called once per frame
		void Update () 
		{
			base.Update();
			
			// If the tower performed an attack previously increase the counter
			if (DidAttack())
			{
				Counter++;
			}
			
			// Shoot a rain of arrows after a time
			if (CanRain())
			{
				//StartCoroutine("RainAttack");
				RainAttack();
				Counter = 0;
			}
				
		}
		/*
		IEnumerator RainAttack()
		{
			while (true)
			{
				for (int i=1; i <= GetQuantityPerRain(); i++)
				{			
					float randomRowTime = Random.Range(0.1f, 0.5f);
					yield return new WaitForSeconds(randomRowTime);
					DoAttackInAngle();
				}
				yield break;
				yield return 0;
			}
		}
		*/	
		private void RainAttack()
		{
			for (int i=0; i <= GetQuantityPerRain(); i++)
			{			
				//AttackIncreaseAngle( (i*2) );
				DoAttack(0, new Vector2(i*0.01f, 0));
			}
		}
		
		public bool CanRain()
		{
			return (GetQuantityToRain() > 0 && GetQuantityPerRain() > 0 && Counter == GetQuantityToRain());
		}
		
		public void SetQuantityToRain(int QuantityToRain)
		{
			this.QuantityToRain = QuantityToRain;
		}
		public int GetQuantityToRain()
		{
			return this.QuantityToRain;
		}
		
		public void SetQuantityPerRain(int QuantityPerRain)
		{
			this.QuantityPerRain = QuantityPerRain;
		}
		public int GetQuantityPerRain()
		{
			return this.QuantityPerRain;
		}
		
		

	}
}