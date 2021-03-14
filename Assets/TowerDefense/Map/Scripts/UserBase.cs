/**
 * Created Date: 3/11/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

using TowerDefense.Enemy.Scripts;
using TowerDefense.Managers;

namespace TowerDefense.Map.Scripts {
	public class UserBase : MonoBehaviour {
		[SerializeField] 
		private HealthBar _baseHealthBar;

		private float _health = 200f;
		private float _originalHealth;
		
		#region Lifecycle
		
		private void Start() {
			this._originalHealth = this._health;
			this._baseHealthBar.SetMaxHealthAndUpdateHealthBar(this._health);
			
			//reset the base health when the user retries the game
			GameSceneManager.Instance.OnGameRetry += ()=> {
				this._health = this._originalHealth;
				this._baseHealthBar.SetMaxHealthAndUpdateHealthBar(this._health);
			};
		}
		
		#endregion

		#region Collision

		private void OnTriggerEnter(Collider other) {
			if (other.tag.Equals("Enemy")) {
				EnemyComponent enemyComponent = other.GetComponent<EnemyComponent>();
				this._baseHealthBar.UpdateHealth(this._health -= enemyComponent.GetHealth()/4);

				if (this._health <= 0.0f) {
					//execute game over event when base health is below or equal to 0
					GameSceneManager.Instance.ExecuteGameOver(false);
				}
				enemyComponent.KillEnemy();
			}
		}

		#endregion
	}
}