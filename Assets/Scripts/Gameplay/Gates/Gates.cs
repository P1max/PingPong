using UnityEngine;

namespace Gates
{
    public class Gates : MonoBehaviour
    {
        [SerializeField] private Side _side;

        public Side Side => _side;
    }

    #region

    public enum Side
    {
        Left = 0,
        Right = 1,
    }

    #endregion
}