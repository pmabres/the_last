using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class TowerBehaviour : PgtCharacter
	{		
		private bool Attacked = false;
		private int WeaponsPerAttack = 1;
		private bool AutoAttack = true;
		
		public void Awake() 
		{
			// Collection container
			SetCollection(Constants.COLLECTION_ALLIES);
			// Collection to attack
			SetCollectionToAttack(Constants.COLLECTION_ENEMIES);
			SetAttackDelayTotalTime(5);
			SetAttackRangeLong(70);
			SetAttackRangeMedium(0); // From here the tower can't attack
			SetHittable(false); // Can't be hittable
			SetAutoAttack(true);
		}
		
		// Update is called once per frame
		public void Update () 
		{
			base.Update();
			Attacked = false;
			// Can attack until we get the animation of the tower
			if (GetAttackTypeToExecute() == Constants.ATTACK_LONG && CanTowerAttack())
			{
				for (int i = 1; i <= GetWeaponsPerAttack(); i++) {
					//AttackIncreaseAngle((i*2));
					DoAttack(( (i-1) * 2), new Vector2( (i-1) * 2, 0));
				}
				CanAttack = false;
				Attacked = true;
			}
		}
		
		/*
		public void AttackIncreaseAngle(float Increaser)
		{
			if (GetWeapon() != null)
			{
				
				GameObject weaponInstance = Instantiate(GetWeapon(), this.gameObject.transform.position, GetWeapon().transform.rotation) as GameObject;
				weaponInstance.GetComponent<WeaponBehaviour>().SetCollection(GetCollection());
				weaponInstance.GetComponent<WeaponBehaviour>().SetOriginTag(gameObject.tag);
				weaponInstance.GetComponent<WeaponBehaviour>().SetOriginEvolutionStatus(GetEvolutionStatus());
				weaponInstance.GetComponent<WeaponBehaviour>().SetTargetDistance(new Vector2(GetNextTargetDistance().x + Increaser, GetNextTargetDistance().y));				
				weaponInstance.GetComponent<WeaponBehaviour>().SetShootingAngle(weaponInstance.GetComponent<WeaponBehaviour>().GetShootingAngle() + Increaser);
				weaponInstance.GetComponent<WeaponBehaviour>().Shoot();
			}
		}
		*/
		
		public bool CanTowerAttack()
		{
			return (CanAttack && this.GetAutoAttack());
		}
		
		public bool DidAttack()
		{
			return this.Attacked;
		}
		
		public void SetWeaponsPerAttack(int q)
		{
			this.WeaponsPerAttack = q;
		}
		
		public int GetWeaponsPerAttack()
		{
			return this.WeaponsPerAttack;
		}
		
		public void SetAutoAttack(bool AutoAttack)
		{
			this.AutoAttack = AutoAttack;
		}
		
		public bool GetAutoAttack()
		{
			return this.AutoAttack;
		}

	}
}