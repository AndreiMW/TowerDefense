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
		
		[SerializeField] 
		private Color _fullHpColor = Color.green;
		
		[SerializeField] 
		private Color _halfHpColor = Color.yellow;
		
		[SerializeField] 
		private Color _lowHpColor = Color.red;

		private float _maxHealth;
		
		#region Public

		public void SetMaxHealthAndUpdateHealthBar(float maxHealth) {
			this._maxHealth = maxHealth;
			this.UpdateHealth(maxHealth);
		}

		public void UpdateHealth(float health) {
			this._healthProgressImage.fillAmount = this.CalculateHealthPercentage(health);
			this.ChangeHealthBarColor(health);
		}
		
		#endregion
		
		#region Private

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

		private float CalculateHealthPercentage(float health) {
			return health / this._maxHealth;
		}
		
		#endregion
	}
}