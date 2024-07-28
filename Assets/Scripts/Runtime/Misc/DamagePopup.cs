using DG.Tweening;
using Runtime.Core.Pool;
using TMPro;
using UnityEngine;

namespace Runtime.Misc
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro damageText;
        
        public void SetDamage(int damage)
        {
            damageText.text = damage.ToString();
        }
        
        public void StartSequence()
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

        private void OnDisable()
        {
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1);
            damageText.transform.localScale = new Vector3(0.05f, 0.05f, 1);
        }
    }
}