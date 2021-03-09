/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;
using TowerDefense.Enemy.Scripts;
using TowerDefense.Map.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] 
	private WaypointManager _waypointManager;
	
	[SerializeField] 
	private Enemy _enemy;

	private int _waypointIndex = 0;
	
	#region Lifecycle

	private void Update() {
		if (this._waypointManager.GetWaypointsArrayLength().Equals(this._waypointIndex)) {
			return;
		}
		this._enemy.MoveEnemyTowardsWaypoint(this._waypointManager.GetWaypointAtIndex(this._waypointIndex));
		this.CheckDistanceBetweenEnemyAndWaypoint();
	}

	#endregion
	
	#region Private

	private void CheckDistanceBetweenEnemyAndWaypoint() {
		float distance = Vector3.Distance(this._enemy.transform.position, this._waypointManager.GetWaypointAtIndex(this._waypointIndex).position);

		if (distance <= 0.5f) {
			this._waypointIndex++;
		}
	}
	
	#endregion
}