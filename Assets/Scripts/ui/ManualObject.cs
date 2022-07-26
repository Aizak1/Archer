using UnityEngine;
using UnityEngine.UI;

namespace ui
{
    public class ManualObject : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        public Button CloseButton => _closeButton;
    }
}