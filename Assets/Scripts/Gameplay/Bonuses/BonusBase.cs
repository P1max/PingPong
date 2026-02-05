using UnityEngine;

namespace Bonuses
{
    public class BonusBase : MonoBehaviour
    {
        [SerializeField] private BonusType _bonusType;
        
        public BonusType BonusType => _bonusType;

        public void PickUpAnim()
        {
            
        }

        public void DestroyAnim()
        {
            
        }
    }
}