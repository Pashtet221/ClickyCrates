using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace DailyRewardSystem {
	public enum RewardType {
		Metals,
		Coins,
		Gems
	}

	[Serializable] public struct Reward {
		public RewardType Type;
		public int Amount;
	}

	public class DailyRewards : MonoBehaviour {

		[Space]
		[Header ( "Reward UI" )]
		[SerializeField] GameObject rewardsCanvas;
		[SerializeField] Button openButton;
		[SerializeField] Button closeButton;

		[SerializeField] Button claimButton;
		[SerializeField] GameObject rewardsNotification;

        [Space]
        [Header("SpinButtonActive")]
        [SerializeField] Button spinButtonActive;

        [Space]
		[Header ( "Rewards Database" )]
		[SerializeField] RewardsDatabase rewardsDB;

		[Space]
		[Header ( "Timing" )]
		//wait 23 Hours to activate the next reward (it's better to use 23h instead of 24h)
		[SerializeField] double nextRewardDelay = 23f;
		//check if reward is available every 5 seconds
		[SerializeField] float checkForRewardDelay = 5f;


		private int nextRewardIndex;
		private bool isRewardReady = false;

		void Start ( ) {
			Initialize ( );

			StopAllCoroutines ( );
			StartCoroutine ( CheckForRewards ( ) );
		}

		void Initialize ( ) {
			nextRewardIndex = PlayerPrefs.GetInt ( "Next_Reward_Index", 0 );

			//Update Mainmenu UI (metals,coins,gems)
			UpdateMetalsTextUI ( );
			UpdateCoinsTextUI ( );
			UpdateGemsTextUI ( );

			//Add Click Events
			openButton.onClick.RemoveAllListeners ( );
			openButton.onClick.AddListener ( OnOpenButtonClick );

			closeButton.onClick.RemoveAllListeners ( );
			closeButton.onClick.AddListener ( OnCloseButtonClick );

			claimButton.onClick.RemoveAllListeners ( );
			claimButton.onClick.AddListener ( OnClaimButtonClick );

			//Check if the game is opened for the first time then set Reward_Claim_Datetime to the current datetime
			if ( string.IsNullOrEmpty ( PlayerPrefs.GetString ( "Reward_Claim_Datetime" ) ) )
				PlayerPrefs.SetString ( "Reward_Claim_Datetime", DateTime.Now.ToString ( ) );
		}

		IEnumerator CheckForRewards ( ) {
			while ( true ) {
				if ( !isRewardReady ) {
					DateTime currentDatetime = DateTime.Now;
					DateTime rewardClaimDatetime = DateTime.Parse ( PlayerPrefs.GetString ( "Reward_Claim_Datetime", currentDatetime.ToString ( ) ) );
                    spinButtonActive.interactable = false;
                    //get total Hours between this 2 dates
                    double elapsedHours = (currentDatetime - rewardClaimDatetime).TotalHours;

					if ( elapsedHours >= nextRewardDelay )
						ActivateReward ( );
					else
						DesactivateReward ( );
				}

				yield return new WaitForSeconds ( checkForRewardDelay );
			}
		}

		public void ActivateReward ( ) {
			isRewardReady = true;
            spinButtonActive.interactable = true;
			rewardsNotification.SetActive ( true );

			Reward reward = rewardsDB.GetReward ( nextRewardIndex );
		}

		void DesactivateReward ( ) {
			isRewardReady = false;
			rewardsNotification.SetActive ( false );
		}

		void OnClaimButtonClick ( ) {
			Reward reward = rewardsDB.GetReward ( nextRewardIndex );

			if ( reward.Type == RewardType.Metals ) {
				Debug.Log ( "<color=white>" + reward.Type.ToString ( ) + " Claimed : </color>+" + reward.Amount );
				GameData.Metals += reward.Amount;
				UpdateMetalsTextUI ( );

			} else if ( reward.Type == RewardType.Coins ) {
				Debug.Log ( "<color=yellow>" + reward.Type.ToString ( ) + " Claimed : </color>+" + reward.Amount );
				GameData.Coins += reward.Amount;
				UpdateCoinsTextUI ( );

			} else {//reward.Type == RewardType.Gems
				Debug.Log ( "<color=green>" + reward.Type.ToString ( ) + " Claimed : </color>+" + reward.Amount );
				GameData.Gems += reward.Amount;
				UpdateGemsTextUI ( );

				isRewardReady = false;
			}

			//Save next reward index
			nextRewardIndex++;
			if ( nextRewardIndex >= rewardsDB.rewardsCount )
				nextRewardIndex = 0;

			PlayerPrefs.SetInt ( "Next_Reward_Index", nextRewardIndex );

			//Save DateTime of the last Claim Click
			PlayerPrefs.SetString ( "Reward_Claim_Datetime", DateTime.Now.ToString ( ) );

			DesactivateReward ( );
		}

		//Update Mainmenu UI (metals,coins,gems)--------------------------------
		void UpdateMetalsTextUI ( ) {
			//metalsText.text = GameData.Metals.ToString ( );
		}

		void UpdateCoinsTextUI ( ) {
			//coinsText.text = GameData.Coins.ToString ( );
		}

		void UpdateGemsTextUI ( ) {
			//gemsText.text = GameData.Gems.ToString ( );
		}

		//Open | Close UI -------------------------------------------------------
		void OnOpenButtonClick ( ) {
			rewardsCanvas.SetActive ( true );
		}

		void OnCloseButtonClick ( ) {
			rewardsCanvas.SetActive ( false );
		}
	}

}

