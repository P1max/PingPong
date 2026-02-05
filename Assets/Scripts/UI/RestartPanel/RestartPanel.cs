using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RestartPanel
{
    public class RestartPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _winText;
        [SerializeField] private TextMeshProUGUI _loseText;
        [SerializeField] private RectTransform _body;
        [SerializeField] private Button _replayButton;

        private Sequence _sequenceOpen;
        private Sequence _sequenceClose;
        private Vector2 _targetBodyPosition;
        private Vector2 _startShift;

        public void Init(Action callback)
        {
            _targetBodyPosition = _body.anchoredPosition;
            _startShift = new Vector2(_targetBodyPosition.x, -Screen.height / 2);

            _sequenceOpen = DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1, 1f).From(0))
                .Join(_body.DOAnchorPos(_targetBodyPosition, 1f).From(_startShift))
                .Append(_replayButton.transform.DOScale(1, 0.5f).From(0).SetEase(Ease.OutBounce))
                .SetAutoKill(false)
                .Pause();

            _sequenceClose = DOTween.Sequence()
                .Append(_replayButton.transform.DOScale(0,0.5f).SetEase(Ease.InBounce))
                .Append(_canvasGroup.DOFade(0, 1f))
                .Join(_body.DOAnchorPos(_startShift, 1f))
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);

                    callback?.Invoke();
                })
                .SetAutoKill(false)
                .Pause();
            
            _replayButton.onClick.AddListener(Hide);
        }

        public void Show(bool isWin)
        {
            Debug.Log($"Включаем");
            _sequenceClose.Pause();

            gameObject.SetActive(true);

            if (isWin)
            {
                _winText.gameObject.SetActive(true);
                _loseText.gameObject.SetActive(false);
            }
            else
            {
                _winText.gameObject.SetActive(false);
                _loseText.gameObject.SetActive(true);
            }

            _sequenceOpen.Restart();
        }

        public void Hide()
        {
            _sequenceOpen.Pause();
            _sequenceClose.Restart();
            
            Debug.Log($"Выключили");
        }
    }
}