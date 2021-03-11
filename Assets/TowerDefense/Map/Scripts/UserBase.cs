/**
 * Created Date: 3/11/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

using TowerDefense.Enemy.Scripts;

namespace TowerDefense.Map.Scripts {
	public class UserBase : MonoBehaviour {
		[SerializeField] 
		private HealthBar _baseHealthBar;

		private float _health = 100f;

		#region Collision

		private void OnTriggerEnter(Collider other) {
			if (other.tag.Equals("Enemy")) {
				Enemy.Scripts.Enemy enemy = other.GetComponent<Enemy.Scripts.Enemy>();
				enemy.KillEnemy();
				this._baseHealthBar.SetHealth(this._health -= enemy.GetHealth()/4);

				if (this._health <= 0) {
					Debug.Log("GAME OVER");
					//raise game over event.
				}
			}
		}

		#endregion
	}
}