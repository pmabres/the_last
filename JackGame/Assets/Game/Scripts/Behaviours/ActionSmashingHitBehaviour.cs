using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class ActionSmashingHitBehaviour :  DirectActionBehaviour
	{
		public void Awake()
		{
			SetChargingTime(1);
			SetColorButton(new Color(0, 0, 8));
		}
		
		public void OnMouseDown()
		{
			if (IsEnabled())
			{
				GameObject.FindGameObjectWithTag(Constants.TAG_JACK).GetComponent<MainCharacter>().SmashingHit();
				Activate();
			}
		}
	}
}
