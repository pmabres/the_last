using UnityEngine;
using System.Collections;
using JackGame;

namespace Pragmatia 
{ 
	public class PgtBehaviour : MonoBehaviour 
	{
		public void Move(Vector3 vector) 
		{
			this.gameObject.transform.Translate(vector);
		}
		
		public void Rotate(Vector3 vector)
		{
			this.gameObject.transform.Rotate(vector);
		}
		
		public void SetColor(float r, float g, float b)
		{
			SetColor(new Color(r, g, b));
		}
		
		public void SetColor(Color NewColor)
		{
			this.gameObject.renderer.material.color = NewColor;
		}
	}
}