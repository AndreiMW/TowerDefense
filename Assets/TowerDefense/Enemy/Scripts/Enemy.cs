/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;
using UnityEngine;

using TowerDefense.Map.Scripts;

namespace TowerDefense.Enemy.Scripts {
	public class Enemy : MonoBehaviour {
		
		[SerializeField] 
		private float _speed;
		
		private WaypointManager _waypointManager => WaypointManager.Instance;
		private int _waypointIndex = 0;
		private Vector3 _originalPosition;

		private bool _shouldStartMoving = false;

		public event Action OnDeath;
		
		#region Lifecycle

		private void Awake() {
			this._originalPosition = this.transform.position;
		}

		private void Update() {
			if (!this._shouldStartMoving) {
				return;
			}
			if (this._waypointManager.GetWaypointsArrayLength().Equals(this._waypointIndex)) {
				this.OnDeath?.Invoke();
				
				this._shouldStartMoving = false;
				this._waypointIndex = 0;
				this.transform.position = this._originalPosition;
			}
			this.MoveEnemyTowardsWaypoint(this._waypointManager.GetWaypointAtIndex(this._waypointIndex));
			this.CheckDistanceBetweenEnemyAndWaypoint();
		}

		private void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag.Equals("Bullet")) {
				this._shouldStartMoving = false;
				this._waypointIndex = 0;
				this.transform.position = this._originalPosition;
				this.OnDeath?.Invoke();
			}
		}

		#endregion
		
		#region Public

		public void StartEnemyMovement() {
			this._shouldStartMoving = true;
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
