using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Archer.Controlls.UI.ShootingControlls {
    public class Level : MonoBehaviour {
        [SerializeField] private Sprite fillMark;
        [SerializeField] private Sprite unfillMark;
        [SerializeField] private Sprite levelCompleBackground;
        [SerializeField] private Sprite levelAvalibleBackground;
        //[SerializeField] private Sprite levelUnavalibleBackground;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI idText;
        [SerializeField] private Image background;
        [SerializeField] private List<Image> markList;

        public void Setup(LevelStatDescriptor descriptor) {
            idText.text = descriptor.Id.ToString();
            var score = descriptor.Score;
            var index = 0;
            var isComplete = false;
            var enumerator = descriptor.ScoreLowerBounds.GetEnumerator();
            foreach (var image  in markList) {
                if (enumerator.MoveNext()) {
                    var lowBoundScore = enumerator.Current;
                    var isFill = score >= lowBoundScore;
                    if (isFill) {
                        image.sprite = fillMark;
                        isComplete = true;
                    } else {
                        image.sprite = unfillMark;
                    }

                } else {
                    image.sprite = unfillMark;
                }
                index++;
            }
            if (isComplete)
                background.sprite = levelCompleBackground;
            else
                background.sprite = levelAvalibleBackground;

        }
    }
}

