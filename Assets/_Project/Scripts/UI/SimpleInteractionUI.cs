using UnityEngine;
using TMPro;

namespace TheCommunityFestival.UI
{
    /// <summary>
    /// Simple UI to show interaction prompts and messages
    /// </summary>
    public class SimpleInteractionUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _promptText;
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private CanvasGroup _promptGroup;
        [SerializeField] private CanvasGroup _messageGroup;

        [Header("Settings")]
        [SerializeField] private float _messageDuration = 3f;

        private float _messageTimer;

        private void Start()
        {
            HidePrompt();
            HideMessage();
        }

        private void Update()
        {
            // Update message timer
            if (_messageTimer > 0)
            {
                _messageTimer -= Time.deltaTime;
                if (_messageTimer <= 0)
                {
                    HideMessage();
                }
            }
        }

        #region Prompt

        public void ShowPrompt(string text)
        {
            if (_promptText != null)
            {
                _promptText.text = text;
            }
            
            if (_promptGroup != null)
            {
                _promptGroup.alpha = 1f;
            }
        }

        public void HidePrompt()
        {
            if (_promptGroup != null)
            {
                _promptGroup.alpha = 0f;
            }
        }

        #endregion

        #region Message

        public void ShowMessage(string text, float duration = -1f)
        {
            if (_messageText != null)
            {
                _messageText.text = text;
            }
            
            if (_messageGroup != null)
            {
                _messageGroup.alpha = 1f;
            }

            _messageTimer = duration > 0 ? duration : _messageDuration;
        }

        public void HideMessage()
        {
            if (_messageGroup != null)
            {
                _messageGroup.alpha = 0f;
            }
        }

        #endregion

        #region Singleton Access

        private static SimpleInteractionUI _instance;
        public static SimpleInteractionUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SimpleInteractionUI>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}

