/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

namespace TowerDefense.Enemy.Scripts {
	public class Enemy : MonoBehaviour {
		
		[SerializeField] 
		private float _speed;
		
		#region Lifecycle
		
		#endregion
		
		#region Public

		public void MoveEnemyTowardsWaypoint(Transform waypoint) {
			Vector3 distance = waypoint.position - this.transform.position;
			this.transform.Translate(this._speed * Time.deltaTime * distance.normalized, Space.World);
		}
		
		#endregion
	}
}
