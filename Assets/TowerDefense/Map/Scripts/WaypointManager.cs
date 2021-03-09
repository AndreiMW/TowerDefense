/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

namespace TowerDefense.Map.Scripts {
	public class WaypointManager : MonoBehaviour {
		private static WaypointManager s_instance;
		public static WaypointManager Instance {
			get {
				if (s_instance == null) {
					s_instance = FindObjectOfType<WaypointManager>();
				}
				return s_instance;
			}
		}

		[SerializeField] 
		private Transform[] _waypoints;
		
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