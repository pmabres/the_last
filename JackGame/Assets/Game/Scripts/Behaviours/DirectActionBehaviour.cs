using UnityEngine;
using System.Collections;
using Pragmatia;


namespace JackGame
{
	public class DirectActionBehaviour : PgtBehaviour
	{
		private float ChargingTime; 			// Time until the direct action can be performed
		private float ChargingTimeCurrent; 		// Current timer
		private Color ColorButton;
		private float InitialAlpha = 0.1f;
		
		public void Start()
		{
			SetColor(0, 0, 0);
		}
		
		// Update is called once per frame
		public void Update () 
		{
			// While is not enabled reload time
			if (!IsEnabled())
			{
				ChargingTimeCurrent += Time.deltaTime;
				if (IsEnabled())
				{
					SetColor(ColorButton);
				}
			}
		}
		
		// Utilize this direct action and restore the timer
		public void Activate()
		{
			if (IsEnabled())
			{
				ChargingTimeCurrent = 0;
				SetColor(0, 0, 0);
			}
		}
		
		public bool IsEnabled()
		{
			return (ChargingTimeCurrent >=  ChargingTime);
		}
		
		public void SetChargingTime(float ChargingTime)
		{
			this.ChargingTime = ChargingTime;
		}
		
		public float GetChargingTime()
		{
			return this.ChargingTime;
		}
		
		public float GetChargingTimeCurrent()
		{
			return this.ChargingTimeCurrent;
		}
		
		public void SetColorButton(Color ColorButton)
		{
			this.ColorButton = ColorButton;
		}
		
		public Color GetColorButton()
		{
			return this.ColorButton;
		}
	}
}