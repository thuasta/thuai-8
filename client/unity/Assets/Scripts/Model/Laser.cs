using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleCity
{
    public class Laser
    {
        public Position startPos { get; set; }
        public Position endPos { get; set; }
               
        public Laser(Position start, Position end)
        {
            startPos = start;
            endPos = end;
            
        }     
               

        public void RemoveTrap()
        {
           //TODO
        }

        
    }
}
