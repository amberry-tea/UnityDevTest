using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoxGame
{
    public class PlayerManager : MonoBehaviour
    {
        #region Singleton

        public static PlayerManager instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        public GameObject player;

    }
}
