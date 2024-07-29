using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runtime.UISystem
{
    public class RetryButton : MonoBehaviour
    {
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }
        
        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _button.onClick.AddListener(OnRetryButtonClicked);
        }
        
        private void OnRetryButtonClicked()
        {
            SceneManager.LoadScene(0);
        }
        
        private void UnsubscribeEvents()
        {
            _button.onClick.RemoveListener(OnRetryButtonClicked);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}