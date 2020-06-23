using RPG.Control;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.CanvasUI
{
    public class CanvasTest : MonoBehaviour
    {
        // required variables
        private PlayerAIController playerAI;
        private PowerBar pb;
        private CreateMover cm;

        private void Start()
        {
            playerAI = FindObjectOfType<PlayerAIController>();
            pb = FindObjectOfType<PowerBar>();
            cm = FindObjectOfType<CreateMover>();
        }

        public void QuitB ()
        {
            Application.Quit();
        }

        public void RunDown()
        {
            playerAI.runToggle = true;
        }
        public void RunUp()
        {
            playerAI.runToggle = false;
        }

        public void PowerDown()
        {
            pb.powerAttack = true;
        }
        public void PowerUp()
        {
            pb.powerAttack = false;
        }

        public void jumpB()
        {
            cm.JumpClick();
        }

        public void EnvironmentOne()
        {
            SceneManager.LoadScene(1);
        }

        public void EnvironmentTwo()
        {
            SceneManager.LoadScene(2);
        }

        public void EnvironmentThree()
        {
            SceneManager.LoadScene(3);
        }

        public void EnvironmentFour()
        {
            SceneManager.LoadScene(4);
        }

        public void EnvironmentFive()
        {
            SceneManager.LoadScene(5);
        }

        public void EnvironmentSix()
        {
            SceneManager.LoadScene(6);
        }

        public void EnvironmentSeven()
        {
            SceneManager.LoadScene(7);
        }

        public void EnvironmentEight()
        {
            SceneManager.LoadScene(8);
        }

        public void EnvironmentNine()
        {
            SceneManager.LoadScene(9);
        }

        public void EnvironmentTen()
        {
            SceneManager.LoadScene(10);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
