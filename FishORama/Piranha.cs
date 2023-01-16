using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishORamaEngineLibrary;

namespace FishORama
{
    /// CLASS: OrangeFish - this class is structured as a FishORama engine Token class
    /// It contains all the elements required to draw a token to screen and update it (for movement etc)
    class Piranha : IDraw
    {
        // CLASS VARIABLES
        // Variables hold the information for the class
        // NOTE - these variables must be present for the class to act as a TOKEN for the FishORama engine
        private string textureID;               // Holds a string to identify asset used for this token
        private float xPosition;                // Holds the X coordinate for token position on screen
        private float yPosition;                // Holds the Y coordinate for token position on screen
        private int xDirection;                 // Holds the direction the token is currently moving - X value should be either -1 (left) or 1 (right)
        private int yDirection;                 // Holds the direction the token is currently moving - Y value should be either -1 (down) or 1 (up)
        private Screen screen;                  // Holds a reference to the screen dimansions (width and height)
        private ITokenManager tokenManager;     // Holds a reference to the TokenManager - for access to ChickenLeg variable

        // *** ADD YOUR CLASS VARIABLES HERE ***

        Random rand; // This will be the random variable
        Random cRand = new Random(); // This will be the class random number

        Piranha_Behaviour piranhaBehaviour = new Piranha_Behaviour(); // This holds the object for the piranha behaviour

        int fishNumber;// This is the number of which the fish is
        int fishTeam; // This is the team that the fish is 

        float initYPos; // This is the inital y position that the fish spawns with
        float initXPos; // This is the inital x position that the fish spawns with

        float targetX; // This will be the location of the chicken leg
        float targetY;

        float speed; // This is the speed used for vector movement

        int chosenFish; // This is what fish was chosen to race

        int state = 3; // This will determine the state that the fish is in
                       // 1 = IDLE - The fish moves around in a circlue
                       // 2 = ATTACK - The fish moves to the chicken leg and eats it
                       // 3 = RETURN - The fish moves back to it space

        float angle;
        float radius;

        /// CONSTRUCTOR: OrangeFish Constructor
        /// The elements in the brackets are PARAMETERS, which will be covered later in the course
        public Piranha(string pTextureID, float pXpos, float pYpos, Screen pScreen, ITokenManager pTokenManager, int pFishNumber, int pFishTeam, Random pRand)
        {
            // State initialisation (setup) for the object
            textureID = pTextureID;
            xPosition = pXpos;
            yPosition = pYpos;
            xDirection = 1;
            yDirection = 1;
            screen = pScreen;
            tokenManager = pTokenManager;
            rand = pRand;

            // *** ADD OTHER INITIALISATION (class setup) CODE HERE ***
            
            //This gets the variables from the constructor and sets it locally
            fishNumber = pFishNumber; 
            fishTeam = pFishTeam; 


            //This changes the direction dependant on the team which makes the fishes face each other
            if(fishTeam == 1)
            {
                xDirection = 1;
            }
            else
            {
                xDirection = -1;
            }


            initYPos = yPosition; // This is the inital y position that the fish spawns with
            initXPos = xPosition; // This gets the inital x position that the fish spawns with

            speed = 5f; // This is the speed variable 

            chosenFish = cRand.Next(0,3); // This generates the first chosen fish to swim towards the chicken leg

            angle = 0f;
            radius = 10f;

        }

        /// METHOD: Update - will be called repeatedly by the Update loop in Simulation
        /// Write the movement control code here
        public void Update()
        {
            // *** ADD YOUR MOVEMENT/BEHAVIOUR CODE HERE ***
            if(tokenManager.ChickenLeg != null && fishNumber == chosenFish)
            {
                state = 2;
            }


            switch (state)
            {
                case 1:
                    xPosition = piranhaBehaviour.circularMovement(initXPos, angle, radius);
                    yPosition = piranhaBehaviour.circularMovement(initYPos, angle, radius);
                    angle = angle + 0.1f;
                    break;

                case 2:
                    if (tokenManager.ChickenLeg == null)
                    {
                        state = 3;
                    }
                    else
                    {
                        targetX = tokenManager.ChickenLeg.Position.X; // This sets the target coordinates to the chicken leg coordinates
                        targetY = tokenManager.ChickenLeg.Position.Y;

                        Vector2 startPos1 = new Vector2(xPosition, yPosition); // This creates a vector for the starting position 
                        Vector2 endPos1 = new Vector2(targetX, targetY); // This creates a vector for the chicken leg position

                        Vector2 vectorBetween1 = new Vector2(endPos1.X - startPos1.X, endPos1.Y - startPos1.Y); // This calculates the distance between the two points

                        float vectorLength1 = vectorBetween1.Length();

                        vectorBetween1 = Vector2.Normalize(vectorBetween1); // This normalizes the vector so that it only has a value between 0 and 1

                        vectorBetween1 = Vector2.Multiply(vectorBetween1, speed); // Multiply the vector between by the speed variable 

                        xPosition = xPosition + vectorBetween1.X; // Update the position with the vector value
                        yPosition = yPosition + vectorBetween1.Y;

                        if (vectorLength1 < 100) // If the piranha is in promixity of both axis
                        {
                            tokenManager.RemoveChickenLeg(); // Remove the chicken leg
                            state = 3; // Change the state to RETURN
                        }
                    }

                    break;

                case 3:

                    targetX = initXPos; // Change the target to be back to the inital position
                    targetY = initYPos;

                    Vector2 startPos = new Vector2(xPosition, yPosition); // create a new starting position
                    Vector2 endPos = new Vector2(targetX, targetY); // Create a new end position

                    Vector2 vectorBetween = new Vector2(endPos.X - startPos.X, endPos.Y - startPos.Y); // This calculates the distance between the two points

                    float vectorLength = vectorBetween.Length();

                    if (vectorLength < 10) // If the target is within 10
                    {
                        state = 1; // Revert back to idling
                    }

                    vectorBetween = Vector2.Normalize(vectorBetween); // This normalizes the vector so that it only has a value between 0 and 1

                    vectorBetween = Vector2.Multiply(vectorBetween, speed); // Multiply the vector between by the speed variable 

                    xPosition = xPosition + vectorBetween.X; // Update the position with the vector value
                    yPosition = yPosition + vectorBetween.Y;

                    break;

            }

        }



        /// METHOD: Draw - Called repeatedly by FishORama engine to draw token on screen
        /// DO NOT ALTER, and ensure this Draw method is in each token (fish) class
        /// Comments explain the code - read and try and understand each lines purpose
        public void Draw(IGetAsset pAssetManager, SpriteBatch pSpriteBatch)
        {
            Asset currentAsset = pAssetManager.GetAssetByID(textureID); // Get this token's asset from the AssetManager

            SpriteEffects horizontalDirection; // Stores whether the texture should be flipped horizontally

            if (xDirection < 0)
            {
                // If the token's horizontal direction is negative, draw it reversed
                horizontalDirection = SpriteEffects.FlipHorizontally;
            }
            else
            {
                // If the token's horizontal direction is positive, draw it regularly
                horizontalDirection = SpriteEffects.None;
            }

            // Draw an image centered at the token's position, using the associated texture / position
            pSpriteBatch.Draw(currentAsset.Texture,                                             // Texture
                              new Vector2(xPosition, yPosition * -1),                                // Position
                              null,                                                             // Source rectangle (null)
                              Color.White,                                                      // Background colour
                              0f,                                                               // Rotation (radians)
                              new Vector2(currentAsset.Size.X / 2, currentAsset.Size.Y / 2),    // Origin (places token position at centre of sprite)
                              new Vector2(1, 1),                                                // scale (resizes sprite)
                              horizontalDirection,                                              // Sprite effect (used to reverse image - see above)
                              1);                                                               // Layer depth
        }
    }
}
