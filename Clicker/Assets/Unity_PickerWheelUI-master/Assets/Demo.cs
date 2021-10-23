using UnityEngine ;
using EasyUI.PickerWheelUI ;
using UnityEngine.UI ;
using DailyRewardSystem;

public class Demo : MonoBehaviour {
   [SerializeField] private Button uiSpinButton ;
   [SerializeField] private Text uiSpinButtonText ;

   [SerializeField] private PickerWheel pickerWheel ;
    private CoinsManager coinsManager;
    private DailyRewards dailyReward;


    private void Start () {
        coinsManager = FindObjectOfType<CoinsManager>();
        dailyReward = FindObjectOfType<DailyRewards>();

        uiSpinButton.onClick.AddListener (() => {

         uiSpinButton.interactable = false ;
         uiSpinButtonText.text = "Spinning" ;

         pickerWheel.OnSpinEnd (wheelPiece => {
            Debug.Log (
               @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Label
               + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
            ) ;

            uiSpinButton.interactable = false;
            uiSpinButtonText.text = "Spin" ;
             switch (wheelPiece.Index)
             {
                 case 0:
                     coinsManager.AddCoins(new Vector3(0, 1, 0), 10);
                     break;
                 case 1:
                     dailyReward.ActivateReward();
                     break;
                 case 3:
                     coinsManager.AddCoins(new Vector3(0, 1, 0), 40);
                     break;
                 case 4:
                     coinsManager.AddCoins(new Vector3(0, 1, 0), 100);
                     break;
             }
         }) ;

         pickerWheel.Spin () ;

      }) ;

   }

}
