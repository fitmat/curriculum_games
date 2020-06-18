using RPG.Control;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
