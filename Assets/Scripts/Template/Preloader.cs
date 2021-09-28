using UnityEngine;

namespace Template
{
    public class Preloader : MonoBehaviour
    {
        [SerializeField] private LevelManager.LevelManager levelManager;
        private void Start()
        {
            levelManager.LoadLastLevel();
        }
    }
}
