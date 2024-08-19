using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.UISystem
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timerText;
        
        private SignalBus _signalBus;
        
        private CancellationTokenSource _cancellationTokenSource;

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
        
        private void OnResumeTimer()
        {
            timerText.text = "1.30";
            _cancellationTokenSource = new CancellationTokenSource();
            StartTimerAsync(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid StartTimerAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                await UniTask.Delay(1000, cancellationToken: cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                ReduceTime();
            }
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
                    _cancellationTokenSource?.Cancel();
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
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<ResumeTimerSignal>(OnResumeTimer);
            _cancellationTokenSource?.Cancel();
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}
