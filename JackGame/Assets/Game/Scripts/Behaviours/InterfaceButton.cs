using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class InterfaceButton : PgtBehaviour 
	{
		public void OnMouseDown()
		{
			switch(gameObject.tag)
			{
			case Constants.BUTTON_TRAP_ATTACK_LEVEL:
				GameObject.FindWithTag(Constants.TAG_TRAP).GetComponent<TrapBehaviour>().AttackLevelUp(false);
				break;
			case Constants.BUTTON_TRAP_SPEED_LEVEL:
				GameObject.FindWithTag(Constants.TAG_TRAP).GetComponent<TrapBehaviour>().SpeedLevelUp(false);
				break;
			case Constants.BUTTON_CREATE_TOWER_ARROWS:
				GameObject.FindWithTag(Constants.TAG_TOWER_STRUCTURE).GetComponent<TowerStructureBehaviour>().CreateTower(Constants.TAG_TOWER_ARROWS);
				break;
			case Constants.BUTTON_CREATE_TOWER_BODIES:
				GameObject.FindWithTag(Constants.TAG_TOWER_STRUCTURE).GetComponent<TowerStructureBehaviour>().CreateTower(Constants.TAG_TOWER_BODIES);
				break;
			case Constants.BUTTON_CREATE_TOWER_ROCKS:
				GameObject.FindWithTag(Constants.TAG_TOWER_STRUCTURE).GetComponent<TowerStructureBehaviour>().CreateTower(Constants.TAG_TOWER_ROCKS);
				break;
			case Constants.BUTTON_REMOVE_TOWER_ARROWS:
				GameObject.FindWithTag(Constants.TAG_TOWER_STRUCTURE).GetComponent<TowerStructureBehaviour>().RemoveTower(Constants.TAG_TOWER_ARROWS);
				break;
			case Constants.BUTTON_REMOVE_TOWER_BODIES:
				GameObject.FindWithTag(Constants.TAG_TOWER_STRUCTURE).GetComponent<TowerStructureBehaviour>().RemoveTower(Constants.TAG_TOWER_BODIES);
				break;
			case Constants.BUTTON_REMOVE_TOWER_ROCKS:
				GameObject.FindWithTag(Constants.TAG_TOWER_STRUCTURE).GetComponent<TowerStructureBehaviour>().RemoveTower(Constants.TAG_TOWER_ROCKS);
				break;
			case Constants.BUTTON_RESTART:
				PgtNavigator.RestartLevel();
				//PgtNavigator.Exit();			
				break;
			case Constants.BUTTON_BARRICADE_LEVEL:
				GameObject.FindWithTag(Constants.TAG_BARRICADE).GetComponent<BarricadeBehaviour>().Upgrade();
				break;
			}
		}
		
	}
}