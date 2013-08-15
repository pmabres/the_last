using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
    public class ExecutionerAxeBehaviour : WeaponBehaviour 
	{
        float clawsTravelSpeed = 0;
        
		// Use this for initialization
		void Start () 
		{
            base.Start();						
            SetTravelSpeed(new Vector2(clawsTravelSpeed,0));
            SetRangeType(Constants.ATTACK_SHORT);
			SetMaxCollisions(1);
			SetTrajectoryType(Constants.TRAJECTORY_LINEAR);
			SetMaxCollisions(5);
			SetDuration(0.1f);
        }
                
        // Update is called once per frame
        void Update () 
		{
            base.Update();
        }
	}
}