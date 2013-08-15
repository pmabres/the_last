using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class UndeadExploderBehaviour : EnemyBehaviour 
	{
		public float SPEED = 0.9f;
		public const int HEALTH = 20;
		private bool exploding = false;
		private bool collidedAlly = false;
		
		public UndeadExploderBehaviour() 
		{
			//SPEED = Random.Range(0.02f,0.06f);
			SetSpeed(SPEED);
			SetTotalHealth(HEALTH);
			SetCurrentHealth(HEALTH);		
			SetShortAttackAnimation(Constants.TAG_EXPLOSION);
			SetWalkLeftAnimation("Walk");
			SetAttackDelayTotalTime(0);
			SetAttackRangeShort(1);
			SetHopeRelease(6, 7);
			SetHomeLifeDrain(10);
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (IsDead()) Invoke("DestroyCharacter", 1);
			if (!exploding) {
				base.Update();
				
				int attackType = GetAttackTypeToExecute();
				
				if (attackType == Constants.ATTACK_SHORT)
				{	
					exploding = true;
					// This should be called by the animation
					DoAttack();
					Invoke("DestroyCharacter", 1);
				} 
				else 
				{
					if (!collidedAlly) base.Move();
				}
			}
		}
		
		// This character can stop when collides Jack or Barricade
		public void OnTriggerEnter(Collider c) 
		{
			base.OnTriggerEnter(c);

			if ((c.gameObject.tag == Constants.TAG_JACK || c.gameObject.tag == Constants.TAG_BARRICADE) && !c.gameObject.GetComponent<PgtCharacter>().IsDead())
			{
				this.collidedAlly = false;
			}
		}
	}
}