﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlockTest
{
    class BoidManager
    {
        
        //Be sure to update the window's size when changine these parameters.
        private int screenWidth = 800;
        private int screenHeight = 600;
        private int screenPadding = 8;

        private Texture2D texture;

        //List to contain the boid objects
        public List<Boid> Boids = new List<Boid>();
        
        //Defines the minimum and maximum speeds for the boids
        private int minSpeed = 60;
        private int maxSpeed = 120;

        //Random number generator used for spawning boids
        private Random rand = new Random();

        int neighborRadius = 32;
        int maxNeighbors= 6;
        public List<Boid> Neighbors = new List<Boid>();

        /// <summary>
        /// Adds a boid to the screen.
        /// </summary>
        public void AddBoid()
        {
            Boid newBoid = new Boid(new Vector2(0, 0), texture, Vector2.Zero);

            newBoid.Position = randomLocation();
            //newBoid.Position = new Vector2(400, 300);
            newBoid.Velocity = randomVelocity();
            //newBoid.Velocity = new Vector2(5, 0);

            //newBoid.Rotation = MathHelper.ToRadians((float)rand.Next(0, 360));
            newBoid.Rotation = 0.0f;
            
            Boids.Add(newBoid);
        }

        /// <summary>
        /// Returns the total count of boids on the screen
        /// </summary>
        /// <returns></returns>
        public int BoidCount()
        {
            int count = 0;

            foreach (Boid boid in Boids)
            {
                count++;
            }

            return count;
        }
        /// <summary>
        /// Constructor for BoidManager
        /// </summary>
        /// <param name="boidCount">How many boids to spawn</param>
        /// <param name="texture">Boid texture to be passed to the boid class</param>
        /// <param name="screenWidth">Width of the simulation window</param>
        /// <param name="screenHeight">Height of the simulation window</param>
        public BoidManager(int boidCount, Texture2D texture, int screenWidth, int screenHeight)
        {
            this.texture = texture;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            for (int x = 0; x < boidCount; x++)
            {
                AddBoid();
            }
        }

        /// <summary>
        /// Generates a random initial location for the boids.
        /// </summary>
        /// <returns>location</returns>
        private Vector2 randomLocation()
        {
            Vector2 location = Vector2.Zero;

            //Sprits will only spawn inside the window.
            location.X = rand.Next(50, screenWidth - 50);
            location.Y = rand.Next(50, screenHeight - 50);

            return location;
        }

        /// <summary>
        /// Generates a random initial velocity for the boids.
        /// </summary>
        /// <returns>velocity</returns>
        private Vector2 randomVelocity()
        {
            Vector2 velocity = new Vector2(
            rand.Next(0, 101) - 50,
            rand.Next(0, 101) - 50);
            velocity.Normalize();
            velocity *= rand.Next(minSpeed, maxSpeed);
            return velocity;
        }

        //FindNeighbors: Get a list of neighbors within neighborRadius.
        public void FindNeighbors(Vector2 target)
        {
            int count = 0;
            
            foreach (Boid boid in Boids)
            {
                float dist;
                dist = Vector2.Distance(boid.Position, target);

                //If distannce is less the the neighborRadius, then add the boid to 
                // the Neighbors list.
                if (dist < neighborRadius)
                {
                    Neighbors.Add(boid);
                    count++;  
                }
                else if (count == maxNeighbors)
                {
                    return;
                }
            }
        }

        //Cohesion: Average the location of all neighbors within neighborRadius.
        public void Cohesion(Vector2 average)
        {
            Vector2 sum = new Vector2();
            int count = 0;
            
            foreach (Boid neighbors in Neighbors)
            {
                sum = sum + neighbors.Position;
                count++;
            }

            average = new Vector2(sum.X / count, sum.Y / count);
        }

        //Alignment: Average the velocitites of all neighbors with neighborRadius.
        public Vector2 Alignment(Vector2 velocity)
        {
            Vector2 sum = new Vector2();
            int count = 0;

            foreach (Boid neighbors in Neighbors)
            {
                sum = sum + neighbors.Velocity;
                count++;
            }

            //Return average velocity
            return velocity = new Vector2(sum.X / count, sum.Y / count);
        }

        //Separation: Check neighbors within neighborRadius to see if it is a safe
        // distance apart. If not, move away from nearest neighbor.
        public void Separation()
        {

        }

        public void CalculateFlock()
        {
            foreach (Boid boid in Boids)
            {
                //Look for neighbors
                FindNeighbors(boid.Position);

                //Calculate alignment
                Alignment(boid.Position);

                //Average alignment with boid's current velocity


                //Clear Neighbors list
                Neighbors.Clear();
            }
        }
        #region Screen Collisions
        public Rectangle LeftScreenBounds
        {
            get { return new Rectangle(-16, 0, 32, screenHeight); }
        }

        public Rectangle RightScreenBounds
        {
            get { return new Rectangle(screenWidth, 0, 16, screenHeight); }
        }

        public Rectangle TopScreenBounds
        {
            get { return new Rectangle(0, -16, screenWidth, 32); }
        }

        public Rectangle BottomScreenBounds
        {
            get { return new Rectangle(0, screenHeight, screenWidth, 16); }
        } 
        #endregion

        //Converts a Vector2 to an angle 
        public float V2ToAngle(Vector2 direction)
        {
            //1.57079633 radians = 90 degrees
            return (float)Math.Atan2(direction.Y, direction.X) + 1.57079633f;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Boid boid in Boids)
            {
                //Calculate flock
                FindNeighbors(boid.Position);
                boid.Velocity = Alignment(boid.Velocity);
                Neighbors.Clear();

                //Rotate boid based on velocity.
                boid.Rotation = V2ToAngle(boid.Velocity);
                
                //If the boid collides with the border, reverse velocity.
                if (boid.IsColliding(RightScreenBounds))
                {
                    Vector2 vel = boid.Velocity;
                    vel.X *= - 1;
                    boid.Velocity = vel;
                }
                if (boid.IsColliding(LeftScreenBounds))
                {
                    Vector2 vel = boid.Velocity;
                    vel.X *= -1;
                    boid.Velocity = vel;
                }
                if (boid.IsColliding(BottomScreenBounds))
                {
                    Vector2 vel = boid.Velocity;
                    vel.Y *= - 1;
                    boid.Velocity = vel;
                }
                if (boid.IsColliding(TopScreenBounds))
                {
                    Vector2 vel = boid.Velocity;
                    vel.Y *= - 1;
                    boid.Velocity = vel;
                }
                boid.Update(gameTime);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Boid boid in Boids)
            {
                boid.Draw(spriteBatch);
            }
        }
    }
}
