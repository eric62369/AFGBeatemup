using UnityEngine;
using UnityEngine.UI;
using BattleInput;

public class StickVisualizerController : MonoBehaviour
{
    private Text StickUI;

    // Start is called before the first frame update
    void Start()
    {
        StickUI = GetComponent<Text>();
    }

    public void UpdateStickUI(Numpad input)
    {
        StickUI.text = input.ToString();
    }
}
