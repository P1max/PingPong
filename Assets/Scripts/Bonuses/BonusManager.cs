using System;
using System.Collections.Generic;
using Gates;
using Paddles;

namespace Bonuses
{
    public class BonusManager
    {
        private readonly PlayerPaddle _playerPaddle;
        private readonly ComputerPaddle _computerPaddle;
        private readonly PlayerPaddleController _playerPaddleController;

        // Основные данные: Тип бонуса -> Оставшееся время
        private readonly Dictionary<Side, Dictionary<BonusType, float>> _activeBonuses = new();
        
        // Кэш ключей для итерации (чтобы не менять словарь во время перебора)
        private readonly Dictionary<Side, List<BonusType>> _activeKeys = new();

        private readonly Dictionary<BonusType, BonusType[]> _conflicts = new()
        {
            { BonusType.UpscaleBoard, new[] { BonusType.DownscaleBoard } },
            { BonusType.DownscaleBoard, new[] { BonusType.UpscaleBoard } }
        };

        private readonly Dictionary<BonusType, float> _bonusesDuration = new()
        {
            { BonusType.BallTwin, 0f },
            { BonusType.UpscaleBoard, 8f },
            { BonusType.DownscaleBoard, 8f },
            { BonusType.ControlInversion, 6f },
        };

        public Action<BallLogic.Ball> OnBallTwinRequested;

        public BonusManager(
            PlayerPaddle playerPaddle,
            ComputerPaddle computerPaddle,
            PlayerPaddleController playerPaddleController)
        {
            _playerPaddle = playerPaddle;
            _computerPaddle = computerPaddle;
            _playerPaddleController = playerPaddleController;

            InitSide(Side.Right);
            InitSide(Side.Left);
        }

        public void ApplyBonus(BonusType bonusType, BallLogic.Ball ballThatTouchedBonus)
        {
            if (!ballThatTouchedBonus.IsLastPlayerPaddleTouch.HasValue) return;

            if (bonusType == BonusType.BallTwin)
            {
                OnBallTwinRequested?.Invoke(ballThatTouchedBonus);
                return;
            }

            var side = ballThatTouchedBonus.IsLastPlayerPaddleTouch.Value ? Side.Right : Side.Left;
            var bonuses = _activeBonuses[side];
            var keys = _activeKeys[side];

            // 1) Удаляем конфликты (если были)
            if (_conflicts.TryGetValue(bonusType, out var conflicts))
            {
                foreach (var conflict in conflicts)
                {
                    if (bonuses.Remove(conflict))
                    {
                        keys.Remove(conflict);
                        RevertEffect(side, conflict);
                    }
                }
            }

            // 2) Если бонуса нет — активируем эффект и запоминаем ключ
            if (!bonuses.ContainsKey(bonusType))
            {
                ApplyEffect(side, bonusType);
                keys.Add(bonusType);
            }

            // 3) Обновляем время (для нового или существующего)
            bonuses[bonusType] = _bonusesDuration[bonusType];
        }

        public void Tick(float deltaTime)
        {
            foreach (var side in _activeBonuses.Keys)
            {
                var bonuses = _activeBonuses[side];
                var keys = _activeKeys[side];

                // Идем по списку ключей с конца (безопасно для удаления)
                for (var i = keys.Count - 1; i >= 0; i--)
                {
                    var type = keys[i];
                    bonuses[type] -= deltaTime;

                    if (bonuses[type] <= 0f)
                    {
                        RevertEffect(side, type);
                        
                        bonuses.Remove(type);
                        keys.RemoveAt(i); 
                    }
                }
            }
        }

        private BasePaddle GetPaddle(Side side) => side == Side.Right ? _playerPaddle : _computerPaddle;

        private void InitSide(Side side)
        {
            _activeBonuses[side] = new Dictionary<BonusType, float>();
            _activeKeys[side] = new List<BonusType>();
        }

        private void ApplyEffect(Side side, BonusType type)
        {
            switch (type)
            {
                case BonusType.UpscaleBoard:
                    GetPaddle(side).IncreaseSize();
                    break;
                case BonusType.DownscaleBoard:
                    GetPaddle(side).DecreaseSize();
                    break;
                case BonusType.ControlInversion:
                    if (side == Side.Right) _playerPaddleController.SetIsInverted(true);
                    break;
            }
        }

        private void RevertEffect(Side side, BonusType type)
        {
            switch (type)
            {
                case BonusType.UpscaleBoard:
                case BonusType.DownscaleBoard:
                    GetPaddle(side).RestoreSize();
                    break;
                case BonusType.ControlInversion:
                    if (side == Side.Right) _playerPaddleController.SetIsInverted(false);
                    break;
            }
        }
    }
}