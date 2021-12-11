using SharedGame;
using System.Collections.Generic;
using UnityGGPO;

namespace PlayerVsGameSpace {

    public class VsGameManager : GameManager {

        public override void StartLocalGame() {
            StartGame(new LocalRunner(new VsGame(2)));
        }

        public override void StartGGPOGame(IPerfUpdate perfPanel, IList<Connections> connections, int playerIndex) {
            var game = new GGPORunner("vsGame", new VsGame(connections.Count), perfPanel);
            game.Init(connections, playerIndex);
            StartGame(game);
        }
    }
}