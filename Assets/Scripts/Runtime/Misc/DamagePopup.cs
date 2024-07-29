using System;
using DG.Tweening;
using Runtime.Core.Pool;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.Misc
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro damageText;
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            SubscribeEvents();
            StartSequence();
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<SetDamagePopupTextSignal>(SetDamagePopupText);
        }

        private void StartSequence()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(0.1f, 1f));
            sequence.Join(transform.DOMoveY(transform.position.y + 0.5f, 0.5f));
            sequence.Insert(0.35f, damageText.DOFade(0, 0.65f));
            sequence.OnComplete(() =>
            {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            });
        }
        
        private void SetDamagePopupText(SetDamagePopupTextSignal signal)
        {
            damageText.text = signal.Damage.ToString();
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<SetDamagePopupTextSignal>(SetDamagePopupText);
        }

        private void Reset()
        {
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1);
            damageText.transform.localScale = new Vector3(0.05f, 0.05f, 1);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
            Reset();
        }
    }
}