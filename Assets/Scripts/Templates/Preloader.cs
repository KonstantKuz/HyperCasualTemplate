using Templates.LevelManagement;
using UnityEngine;

namespace Templates
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
