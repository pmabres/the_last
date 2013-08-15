using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class ActionRainArrowsBehaviour :  DirectActionBehaviour
	{
		public void Awake()
		{
			SetChargingTime(5);
			SetColorButton(new Color(8, 0, 0));
		}
		
		public void OnMouseDown()
		{
			if (IsEnabled())
			{
				GameObject.FindGameObjectWithTag(Constants.TAG_RAINARROWS).GetComponent<RainArrowsBehaviour>().Rain();
				Activate();
			}
		}
	}
}
