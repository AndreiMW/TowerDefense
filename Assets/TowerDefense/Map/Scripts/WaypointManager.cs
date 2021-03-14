/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

namespace TowerDefense.Map.Scripts {
	public class WaypointManager : MonoBehaviour {
		public static WaypointManager Instance;

		[SerializeField] 
		private Transform[] _waypoints;

		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;	
			}
		}

		#endregion
		
		#region Public
		
		/// <summary>
		/// Get a waypoint from the list, with the asked index.
		/// </summary>
		/// <param name="index"> The index of the asked waypoint.</param>
		/// <returns>The waypoint with the specified index.</returns>
		public Transform GetWaypointAtIndex(int index) {
			return this._waypoints[index];
		}

		#endregion
	}
}