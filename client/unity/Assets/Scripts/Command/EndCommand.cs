using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCity
{
    public class EndCommand : AbstractCommand
    {

        public EndCommand()
        {
        }

        protected override void OnExecute()
        {
            SceneManager.LoadScene("End");
        }
        
    }
}