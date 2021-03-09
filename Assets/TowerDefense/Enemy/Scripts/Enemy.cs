/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

using TowerDefense.Map.Scripts;

namespace TowerDefense.Enemy.Scripts {
	public class Enemy : MonoBehaviour {
		
		[SerializeField] 
		private float _speed;
		
		private WaypointManager _waypointManager => WaypointManager.Instance;
		private int _waypointIndex = 0;
		private Vector3 _originalPosition;
		
		#region Lifecycle

		private void Awake() {
			this._originalPosition = this.transform.position;
		}

		private void Update() {
			if (this._waypointManager.GetWaypointsArrayLength().Equals(this._waypointIndex)) {
				this._waypointIndex = 0;
				this.transform.position = this._originalPosition;

			}
			this.MoveEnemyTowardsWaypoint(this._waypointManager.GetWaypointAtIndex(this._waypointIndex));
			this.CheckDistanceBetweenEnemyAndWaypoint();
		}
		
		#endregion
		
		#region Private

		private void MoveEnemyTowardsWaypoint(Transform waypoint) {
			Vector3 distance = waypoint.position - this.transform.position;
			this.transform.Translate(this._speed * Time.deltaTime * distance.normalized, Space.World);
		}
		
		private void CheckDistanceBetweenEnemyAndWaypoint() {
			float distance = Vector3.Distance(this.transform.position, this._waypointManager.GetWaypointAtIndex(this._waypointIndex).position);

			if (distance <= 0.5f) {
				this._waypointIndex++;
			}
		}
		
		#endregion
	}
}
