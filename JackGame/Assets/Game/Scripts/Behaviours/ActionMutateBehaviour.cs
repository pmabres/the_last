using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class ActionMutateBehaviour :  DirectActionBehaviour
	{
		private bool Mutated = false;
		private float CheckMutation = 0.5f;
		private float CheckMutationTimer;
		
		public void Awake()
		{
			SetChargingTime(10);
			SetColorButton(new Color(8, 2, 0));
		}
		
		public void Update()
		{
			// Start counting when Jack is not mutated anymore
			if (!Mutated)
			{
				base.Update();
			}
			else
			{
				CheckMutationTimer += Time.deltaTime;
				if (CheckMutationTimer >= CheckMutation && !GameObject.FindGameObjectWithTag(Constants.TAG_JACK).GetComponent<MainCharacter>().IsMutated())
				{
					Mutated = false;
					CheckMutationTimer = 0;
				}
			}
		}
		
		public void OnMouseDown()
		{
			if (IsEnabled())
			{
				GameObject.FindGameObjectWithTag(Constants.TAG_JACK).GetComponent<MainCharacter>().Evolve();
				Mutated = true;
				Activate();
			}
		}
	}
}
