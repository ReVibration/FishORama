using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FishORamaEngineLibrary;
using System.Collections.Generic;

namespace FishORama
{
    /// CLASS: Simulation class - the main class users code in to set up a FishORama simulation
    /// All tokens to be displayed in the scene are added here
    public class Simulation : IUpdate, ILoadContent
    {
        // CLASS VARIABLES
        // Variables store the information for the class
        private IKernel kernel;                 // Holds a reference to the game engine kernel which calls the draw method for every token you add to it
        private Screen screen;                  // Holds a reference to the screeen dimensions (width, height)
        private ITokenManager tokenManager;     // Holds a reference to the TokenManager - for access to ChickenLeg variable

        /// PROPERTIES
        public ITokenManager TokenManager      // Property to access chickenLeg variable
        {
            set { tokenManager = value; }
        }

        // *** ADD YOUR CLASS VARIABLES HERE ***
        // Variables to hold fish will be declared here

        List<Piranha> piranhas = new List<Piranha>(); // This creates the list for the piranhas

        int diffYPos = 150; // This is the position that the fish will start generate and also its how spaced the fish will be on the y axis
        int initXpos = 300; // This is the position that the fish will generate on the x axis

        Random rand = new Random(); // This creates a random object

        int randomChicken; // This is the variable that will generate the chicken leg at random

        /// CONSTRUCTOR - for the Simulation class - run once only when an object of the Simulation class is INSTANTIATED (created)
        /// Use constructors to set up the state of a class
        public Simulation(IKernel pKernel)
        {
            kernel = pKernel;                   // Stores the game engine kernel which is passed to the constructor when this class is created
            screen = kernel.Screen;             // Sets the screen variable in Simulation so the screen dimensions are accessible

            // *** ADD OTHER INITIALISATION (class setup) CODE HERE ***




        }

        /// METHOD: LoadContent - called once at start of program
        /// Create all token objects and 'insert' them into the FishORama engine
        public void LoadContent(IGetAsset pAssetManager)
        {
            // *** ADD YOUR NEW TOKEN CREATION CODE HERE ***
            // Code to create fish tokens and assign to thier variables goes here
            // Remember to insert each token into the kernel

            //This starts the y position for each piranha
            int ypos = diffYPos;
            
            //This loops through the first team and creates 3 piranhas and also stores them in the list
            for (int i = 0; i < 3; i++) 
            {
                int xpos = -(initXpos);
                Piranha tempFish = new Piranha("Piranha1", xpos, ypos, screen, tokenManager,i,1, rand);
                piranhas.Add(tempFish);
                kernel.InsertToken(tempFish);
                ypos = ypos - diffYPos;
            }

            //This resets the y position for the other team
            ypos = diffYPos;

            //This create through the second team and stores the piranhas in the list
            for(int i = 0; i < 3; i++)
            {
                int xpos = initXpos;
                Piranha tempFish = new Piranha("Piranha1", xpos, ypos, screen, tokenManager, i, 2, rand);
                piranhas.Add(tempFish);
                kernel.InsertToken(tempFish);
                ypos = ypos - diffYPos;
            }       



        }

        /// METHOD: Update - called 60 times a second by the FishORama engine when the program is running
        /// Add all tokens so Update is called on them regularly
        public void Update(GameTime gameTime)
        {

            // *** ADD YOUR UPDATE CODE HERE ***
            // Each fish object (sitting in a variable) must have Update() called on it here

            //This updates each piranha in the list
            foreach(Piranha piranha in piranhas)
            {
                piranha.Update();
            }

            //This generates the random number which controls the place leg method
            randomChicken = rand.Next(0, 100);

            //This checks whether the random number is 1 to run the PlaceLeg() method
            if(randomChicken == 1)
            {
                PlaceLeg();
            }

        }

        //This places a chicken leg in the center of the screen
        public void PlaceLeg()
        {
            // If a chicken leg is not currently placed and the left mouse button is pressed
            if (tokenManager.ChickenLeg == null)
            {
                // Place a new chicken leg at the position at the center of the screen
                ChickenLeg newChickenLeg = new ChickenLeg("ChickenLeg", 0, 0);
                tokenManager.SetChickenLeg(newChickenLeg);
                kernel.InsertToken(newChickenLeg);
            }

        }
    }
}
