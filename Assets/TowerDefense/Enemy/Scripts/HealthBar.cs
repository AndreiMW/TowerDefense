/**
 * Created Date: 3/11/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.Enemy.Scripts {
	public class HealthBar : MonoBehaviour {
		[SerializeField] 
		private Image _healthProgressImage;
		
		[Header("Colors")]
		[SerializeField] 
		private Color _fullHpColor = Color.green;
		
		[SerializeField] 
		private Color _halfHpColor = Color.yellow;
		
		[SerializeField] 
		private Color _lowHpColor = Color.red;

		private float _maxHealth;
		
		#region Public

		/// <summary>
		/// Sets the maximum health value for the health bar and updates it.
		/// </summary>
		/// <param name="maxHealth">The maximum health it can display.</param>
		public void SetMaxHealthAndUpdateHealthBar(float maxHealth) {
			this._maxHealth = maxHealth;
			this.UpdateHealth(maxHealth);
		}

		/// <summary>
		/// Updates the health.
		/// </summary>
		/// <param name="health">The health value to calculate.</param>
		public void UpdateHealth(float health) {
			this._healthProgressImage.fillAmount = this.CalculateHealthPercentage(health);
			this.ChangeHealthBarColor(health);
		}
		
		#endregion
		
		#region Private

		/// <summary>
		/// Changes the healthbar color depending on the amount on health remaining.
		/// </summary>
		/// <param name="health">The current health of the healthbar.</param>
		private void ChangeHealthBarColor(float health) {
			float currentHealthValue = this.CalculateHealthPercentage(health);
			if (currentHealthValue <= 0.25) {
				this._healthProgressImage.color = this._lowHpColor;
			} else {
				if (currentHealthValue > 0.25 && currentHealthValue <= 0.75) {
					this._healthProgressImage.color = this._halfHpColor;	
				} else {
					this._healthProgressImage.color = this._fullHpColor;
				}
			}
		}

		/// <summary>
		/// Calculates the health percentage of the healthbar using the rule of 3.
		/// maxhealth.....1
		/// health....x
		/// => x = health * 1 / maxHealth
		/// </summary>
		/// <param name="health"></param>
		/// <returns></returns>
		private float CalculateHealthPercentage(float health) {
			return health / this._maxHealth;
		}
		
		#endregion
	}
}