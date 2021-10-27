using UnityEngine;
using TMPro;

namespace Archer.Controlls.Other {
    public class UIFPS : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI text;
        private int avgFrameRate;

        public void Update() {
            avgFrameRate = (int)(Time.frameCount / Time.time);
            if (text.text != avgFrameRate.ToString()) {
                text.text = avgFrameRate.ToString();
            }
        }
    }
}
