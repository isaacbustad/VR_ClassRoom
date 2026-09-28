// Isaac Bustad
// 8/7/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BugFreeProductions.VRClassroom
{
    public class CanvasNav : MonoBehaviour
    {
        //  Vars
        protected Stack<GameObject> pannelStack = new Stack<GameObject>();

        // Methods
        public virtual void LoadNextScene(int aSceneIdx)
        {
            SceneManager.LoadScene(aSceneIdx);
        }

        public virtual void TurnPannelOn(GameObject aPannel)
        {
            aPannel.SetActive(true);

            pannelStack.Push(aPannel);
        }

        public virtual void TurnPannelOff()
        {
            pannelStack.Pop().SetActive(false);
        }

        // Accessors
    }
}
