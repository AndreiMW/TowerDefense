/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

namespace TowerDefense.Map.Scripts {
	public class WaypointManager : MonoBehaviour {
		public static WaypointManager Instance { get; private set; }

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

		public Transform GetWaypointAtIndex(int index) {
			return this._waypoints[index];
		}

		public int GetWaypointsArrayLength() {
			return this._waypoints.Length;
		}
		
		#endregion
	}
}