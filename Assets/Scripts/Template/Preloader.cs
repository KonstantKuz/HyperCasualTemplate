using Template.LevelManagement;
using UnityEngine;

namespace Template
{
    public class Preloader : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        private void Start()
        {
            levelManager.LoadLastLevel();
        }
    }
}
