using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pragmatia;

namespace JackGame
{
	public class TowerStructureBehaviour : PgtBehaviour 
	{	
		private Dictionary<string, GameObject> CreateControls = new Dictionary<string, GameObject>();	
		private Dictionary<string, GameObject> RemoveControls = new Dictionary<string, GameObject>();	
		private GameObject resetButton;
		
		void Start()
		{
			// Move behind at start
			gameObject.transform.position.Set(
				gameObject.transform.position.x,
				gameObject.transform.position.y,
				4
			);
			
			CreateControlsTowers();
			
		}
		
		public void CreateTower(string TowerType)
		{
			Vector3 Position = PgtNavigator.TowerSpawnPosition;
			
			// Rock towers at floor
			if (TowerType == Constants.TAG_TOWER_ROCKS) Position.y = Constants.FLOOR_Y_POSITION;
			// Arrow tower is on top of the tower.
			if (TowerType == Constants.TAG_TOWER_ARROWS) Position.y = Position.y + gameObject.renderer.bounds.size.y / 2;
			// Generate the instance
			Statics.GenerateInstance(Constants.COLLECTION_ALLIES, TowerType, Position); 
			
			// Hide the control of the tower
			this.CreateControls[TowerType].SetActive(false);
			
			// Show the control to remove the tower
			this.RemoveControls[TowerType].SetActive(true);
		}
		
		public void RemoveTower(string TowerType)
		{
			GameObject.FindWithTag(TowerType).GetComponent<PgtCharacter>().DestroyCharacter();
			
			// Show the control of the tower
			this.CreateControls[TowerType].SetActive(true);
			
			// Hide the remove button
			this.RemoveControls[TowerType].SetActive(false);
		}
		
		
		private void CreateControlsTowers()
		{
			// Instantiate controls of the barricade
			GameObject tmp;
			
			// CREATES
			
			// Tower arrows (RED)
			tmp = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_EMPTY), this.gameObject.transform.position + new Vector3(-4, -6, -0.6f), Quaternion.identity) as GameObject;			
			tmp.renderer.material.color = new Color(5,0,0);
			tmp.transform.localScale = new Vector3(2, 1, 1);
			tmp.gameObject.tag = Constants.BUTTON_CREATE_TOWER_ARROWS;
			tmp.gameObject.name = Constants.BUTTON_CREATE_TOWER_ARROWS;
			tmp.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUD).transform;
			CreateControls.Add(Constants.TAG_TOWER_ARROWS, tmp);
			
			// Tower bodies (GREEN)
			tmp = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_EMPTY), this.gameObject.transform.position + new Vector3(0, -6, -0.6f), Quaternion.identity) as GameObject;			
			tmp.renderer.material.color = new Color(0,5,0);
			tmp.transform.localScale = new Vector3(2, 1, 1);
			tmp.gameObject.tag = Constants.BUTTON_CREATE_TOWER_BODIES;
			tmp.gameObject.name = Constants.BUTTON_CREATE_TOWER_BODIES;
			tmp.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUD).transform;
			CreateControls.Add(Constants.TAG_TOWER_BODIES, tmp);
			
			// Tower rocks (BLUE)
			tmp = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_EMPTY), this.gameObject.transform.position + new Vector3(4, -6, -0.6f), Quaternion.identity) as GameObject;
			tmp.renderer.material.color = new Color(0,0,5);
			tmp.transform.localScale = new Vector3(2, 1, 1);
			tmp.gameObject.tag = Constants.BUTTON_CREATE_TOWER_ROCKS;
			tmp.gameObject.name = Constants.BUTTON_CREATE_TOWER_ROCKS;
			tmp.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUD).transform;
			CreateControls.Add(Constants.TAG_TOWER_ROCKS, tmp);
			
			// REMOVES (Inactives by default)
			
			// Tower arrows
			tmp = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_EMPTY), this.gameObject.transform.position + new Vector3(-4, -6, -0.6f), Quaternion.identity) as GameObject;
			tmp.renderer.material.color = new Color(5,5,5);
			tmp.transform.localScale = new Vector3(2, 1, 1);
			tmp.gameObject.tag = Constants.BUTTON_REMOVE_TOWER_ARROWS;
			tmp.gameObject.name = Constants.BUTTON_REMOVE_TOWER_ARROWS;
			tmp.SetActive(false);
			tmp.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUD).transform;
			RemoveControls.Add(Constants.TAG_TOWER_ARROWS, tmp);
			
			// Tower bodies
			tmp = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_EMPTY), this.gameObject.transform.position + new Vector3(0, -6, -0.6f), Quaternion.identity) as GameObject;			
			tmp.renderer.material.color = new Color(5,5,5);
			tmp.transform.localScale = new Vector3(2, 1, 1);
			tmp.gameObject.tag = Constants.BUTTON_REMOVE_TOWER_BODIES;
			tmp.gameObject.name = Constants.BUTTON_REMOVE_TOWER_BODIES;
			tmp.SetActive(false);
			tmp.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUD).transform;
			RemoveControls.Add(Constants.TAG_TOWER_BODIES, tmp);
			
			// Tower rocks!!
			tmp = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_EMPTY), this.gameObject.transform.position + new Vector3(4, -6, -0.6f), Quaternion.identity) as GameObject;			
			tmp.renderer.material.color = new Color(5,5,5);
			tmp.transform.localScale = new Vector3(2, 1, 1);
			tmp.gameObject.tag = Constants.BUTTON_REMOVE_TOWER_ROCKS;
			tmp.gameObject.name = Constants.BUTTON_REMOVE_TOWER_ROCKS;
			tmp.SetActive(false);
			tmp.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUD).transform;
			RemoveControls.Add(Constants.TAG_TOWER_ROCKS, tmp);
			
		}
		
	}
}