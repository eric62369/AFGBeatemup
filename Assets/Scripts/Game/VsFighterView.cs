using SharedGame;
using UnityEngine;
using UnityEngine.UI;
using UnityGGPO;

namespace PlayerVsGameSpace {

    public class VsFighterView : MonoBehaviour {
        // public Text txtStatus;
        // public Image imgProgress;
        public Transform model;

        public void Populate(Fighter fighter, PlayerConnectionInfo info) {
            transform.position = fighter.position;
            model.rotation = Quaternion.Euler(0, 0, fighter.heading);
            string status = "";
            int progress = -1;
            switch (info.state) {
                case PlayerConnectState.Connecting:
                    status = (info.type == GGPOPlayerType.GGPO_PLAYERTYPE_LOCAL) ? "Local Player" : "Connecting...";
                    break;

                case PlayerConnectState.Synchronizing:
                    progress = info.connect_progress;
                    status = (info.type == GGPOPlayerType.GGPO_PLAYERTYPE_LOCAL) ? "Local Player" : "Synchronizing...";
                    break;

                case PlayerConnectState.Disconnected:
                    status = "Disconnected";
                    break;

                case PlayerConnectState.Disconnecting:
                    status = "Waiting for player...";
                    progress = (Utils.TimeGetTime() - info.disconnect_start) * 100 / info.disconnect_timeout;
                    break;
            }
        }
    }
}