using SharedGame;
using System;
using UnityEngine;

namespace PlayerVsGameSpace {

    public class VsGameView : MonoBehaviour, IGameView {
        public VsFighterView fighterPrefab;
        private VsFighterView[] fighterViews = Array.Empty<VsFighterView>();
        private GameManager gameManager => GameManager.Instance;

        private void ResetView(VsGame gs) {
            var fighterGss = gs._fighters;
            fighterViews = new VsFighterView[fighterGss.Length];

            for (int i = 0; i < fighterGss.Length; ++i) {
                fighterViews[i] = Instantiate(fighterPrefab, transform);
            }
        }

        public void UpdateGameView(IGameRunner runner) {
            var gs = (VsGame)runner.Game;
            var ngs = runner.GameInfo;

            var fighterGss = gs._fighters;
            if (fighterViews.Length != fighterGss.Length) {
                ResetView(gs);
            }
            for (int i = 0; i < fighterGss.Length; ++i) {
                fighterViews[i].Populate(fighterGss[i], ngs.players[i]);
            }
        }

        private void Update() {
            if (gameManager.IsRunning) {
                UpdateGameView(gameManager.Runner);
            }
        }
    }
}