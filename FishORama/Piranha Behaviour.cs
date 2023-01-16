using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishORama
{
    class Piranha_Behaviour
    {
        //DECLARTION OF VARIABLES

        float angle = 0f; // This is the angle for the fishes circular movement
        float radius = 10f; // This is the radius and distance from the point that the fish is swiming in

        float initYPos; // These are the inital coordinates for the fishes to be doing circles around
        float initXPos;

        float xPosition;
        float yPosition;

        public float circularMovement (float initPos, float angle, float radius)
        {
            float position = (float)(initPos + (Math.Cos(angle) * radius)); // Changes the xPosition to be on the angle using cosine rule of a triangle

            return position;
        }

    }
}
