using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class ActionCallDogBehaviour :  DirectActionBehaviour
	{
		public void Awake()
		{
			SetChargingTime(5);
			SetColorButton(new Color(0, 8, 0));
		}
		
		public void OnMouseDown()
		{
			if (IsEnabled())
			{
				GameObject.FindGameObjectWithTag(Constants.TAG_DOG).GetComponent<DogBehaviour>().SetCalled(true);
				Activate();
			}
		}
	}
}
