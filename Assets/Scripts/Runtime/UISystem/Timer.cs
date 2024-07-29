using Runtime.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.UISystem
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro timerText;
        
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
             _signalBus.Subscribe<ResumeTimerSignal>(OnResumeTimer);
        }
        
        private void OnResumeTimer(ResumeTimerSignal signal)
        {
            int minutes = Mathf.FloorToInt(signal.WaveTime / 60);
            int seconds = Mathf.FloorToInt(signal.WaveTime % 60);
            
            timerText.text = $"{minutes:0}:{seconds:00}";
            InvokeRepeating(nameof(ReduceTime), 1, 1f);
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<ResumeTimerSignal>(OnResumeTimer);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void ReduceTime()
        {
            var time = timerText.text;
            var minute = int.Parse(time[0].ToString());
            var secondFirstIndex = int.Parse(time[2].ToString());
            var secondSecondIndex = int.Parse(time[3].ToString());
            
            if(secondFirstIndex == 0 && secondSecondIndex == 0)
            {
                if(minute == 0)
                {
                    CancelInvoke(nameof(ReduceTime));
                    // TODO: Game Over
                    return;
                }
                
                minute--;
                secondFirstIndex = 5;
                secondSecondIndex = 9;
            }
            else
            {
                if(secondSecondIndex == 0)
                {
                    secondFirstIndex--;
                    secondSecondIndex = 9;
                }
                else
                {
                    secondSecondIndex--;
                }
            }
            
            timerText.text = minute + "." + secondFirstIndex + secondSecondIndex;
        }
    }
}