using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class WeaponBehaviour : MonoBehaviour {
		private int rangeType;
		private string originTag;
		private int originEvolutionStatus;
		private Vector2 travelSpeed;
		private int maxCollisions = 0;
		private int currentCollisions = 0;
		private int collection = 0;
		private float duration = 0;
		private float durationTimer = 0;
		private int trajectoryType;
		private Vector2 targetDistance = new Vector2();
		private Vector2 initialPosition = new Vector2();
		private float shootingAngle;
		private int Damage;
		// Use this for initialization
		public void Awake () 
		{
			
		}
		public void Start () 
		{			
			Vector3 position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);			
			gameObject.transform.position = position;			
			//initialPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
			//this.SetOriginTag(gameObject.transform.parent.tag);
			//this.SetCollection(gameObject.transform.parent.GetComponent<PgtCharacter>().GetCollection());
			//gameObject.transform.parent = null;
		}
		
		// This method fire the projectile for weapons that have a parabolic trajectory giving a initial impulse
		public void Shoot ()			
		{					
			if (GetTrajectoryType() == Constants.TRAJECTORY_PARABOLIC)				
			{
				CalculateForce(GetTargetDistance());
				gameObject.rigidbody.AddForce(new Vector3(GetTravelSpeed().x,GetTravelSpeed().y,0),ForceMode.Impulse);
			}
		}
		public void CalculateForce(Vector2 targetDistance)
		{			
			float a = GetShootingAngle();
			float x = targetDistance.x;
			float g = Physics.gravity.magnitude;
			float y = targetDistance.y - gameObject.transform.position.y;
			
			//float distance = Mathf.Sqrt( Mathf.Pow(targetDistance.x, 2) + Mathf.Pow(targetDistance.y - gameObject.transform.position.y, 2) );
			//check how this works
			float velX = Mathf.Sqrt(x * g / Mathf.Sin(2 * Mathf.Deg2Rad * a));
			//float velX = (Mathf.Sqrt(g) * Mathf.Sqrt(x) * Mathf.Sqrt((Mathf.Tan(a)*Mathf.Tan(a))+1)) / Mathf.Sqrt(2 * Mathf.Tan(a) - (2 * g * y) / x); // velocity
			/*
			float velX = (1 / Mathf.Cos(a * Mathf.Deg2Rad)) * 
								Mathf.Sqrt((0.5f * g * 
											Mathf.Pow(displacement,2)) / 
										( displacement * Mathf.Tan (Mathf.Deg2Rad * a) + initialPosition.y));
			
			*/
			float velY = ((velX / Mathf.Cos (a * Mathf.Deg2Rad)) * Mathf.Sin(a * Mathf.Deg2Rad)) + y;
			float forceX = velX * gameObject.rigidbody.mass;
			float forceY = velY * gameObject.rigidbody.mass;
			SetTravelSpeed(new Vector2(forceX,forceY));			
		}
		// Update is called once per frame
		public void Update ()
		{			
			switch (GetTrajectoryType())
			{
				case Constants.TRAJECTORY_LINEAR:
					gameObject.transform.Translate(GetTravelSpeed().x, 0, 0);
					break;
				case Constants.TRAJECTORY_PARABOLIC:
					float angle = Mathf.Atan2(gameObject.rigidbody.velocity.y,gameObject.rigidbody.velocity.x) * Mathf.Rad2Deg;			
					gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
					break;
			}			
			// Todo Find another method to kill or destroy the instance if the projectile leaves the screen
			// Destroy when timer reaches the duration. Don't apply when weapon doesnt have duration setted
			durationTimer += Time.deltaTime;
			if ((durationTimer >= duration && duration > 0) || !CanCollide())  // !gameObject.renderer.isVisible
			{
				Destroy(gameObject);			
			}
		}

		public void OnTriggerEnter(Collider c)
		{
			/*
			// Care about collitions between weapon and a character
			if (c.gameObject.GetComponent<PgtCharacter>() != null)
			{
				// Get the collection of the collider character
				int colliderCollection = c.gameObject.GetComponent<PgtCharacter>().GetCollection();
				
				// Get the collection of the character who used this weapon
				//int weaponCollection = this.gameObject.transform.parent.gameObject.GetComponent<PgtCharacter>().GetCollection();
				int weaponCollection = this.GetCollection();
				
				// Destroy when touch a character of another collection
				if (colliderCollection != weaponCollection)
				{
					{
						Destroy(gameObject); // this will destroy the arrow on collision
					}
				}
			}
			*/
		}

		public void SetTrajectoryType(int trajectoryType)
		{
			this.trajectoryType = trajectoryType;
		}
		public int GetTrajectoryType()
		{
			return this.trajectoryType;
		}
		public void SetTravelSpeed(Vector2 travelSpeed)
		{
			this.travelSpeed = travelSpeed;
		}
		public Vector2 GetTravelSpeed()
		{
			return this.travelSpeed;
		}
		public void SetRangeType(int rangeType)
		{
			this.rangeType = rangeType;
		}
		public int GetRangeType()
		{
			return this.rangeType;
		}
		public void SetMaxCollisions(int maxCollisions)
		{
			this.maxCollisions = maxCollisions;
		}
		public int GetMaxCollisions()
		{
			return this.maxCollisions;
		}
		public void SetCurrentCollisions(int currentCollisions)
		{
			this.currentCollisions = currentCollisions;
		}
		public int GetCurrentCollisions()
		{
			return this.currentCollisions;
		}
		public void SetDuration(float Duration)
		{
			this.duration = Duration;
		}
		public float GetDuration()
		{
			return this.duration;
		}
		public void SetOriginTag(string OriginTag)
		{
			this.originTag = OriginTag;
		}
		public string GetOriginTag()
		{
			return this.originTag;
		}
		public void SetOriginEvolutionStatus(int OriginEvolutionStatus)
		{
			this.originEvolutionStatus = OriginEvolutionStatus;
		}
		public int GetOriginEvolutionStatus()
		{
			return this.originEvolutionStatus;
		}
		public void SetCollection(int Collection)
		{
			this.collection = Collection;
		}
		public int GetCollection()
		{
			return this.collection;
		}
		public bool CanDamage()
		{			
			bool r = false;
			if (CanCollide())
			{
				r = true;
				SetCurrentCollisions(GetCurrentCollisions() + 1);
			}
			return r;
		}		
		public bool CanCollide()
		{
			return (GetCurrentCollisions() < GetMaxCollisions() || GetMaxCollisions() == 0);
		}
		public void SetTargetDistance(Vector2 targetDistance)
		{
			this.targetDistance = targetDistance;
		}
		public Vector2 GetTargetDistance()
		{
			return this.targetDistance;
		}
		public void SetShootingAngle(float shootingAngle)
		{
			this.shootingAngle = shootingAngle;
		}
		public float GetShootingAngle()
		{
			return this.shootingAngle;
		}
		public void SetDamage(int Damage)
		{
			this.Damage = Damage;
		}
		public int GetDamage()
		{
			return this.Damage;
		}
	}
}