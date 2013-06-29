using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlockTest
{
    class BoidManager
    {
        private int screenWidth = 800;
        private int screenHeight = 600;
        private int screenPadding = 16;

        private Texture2D texture;

        public List<Boid> Boids = new List<Boid>();
        private int minSpeed = 60;
        private int maxSpeed = 120;

        private Random rand = new Random();

        public void AddBoid()
        {
            Boid newBoid = new Boid(new Vector2(0, 0), texture, Vector2.Zero);

            newBoid.Position = randomLocation();
            newBoid.Velocity = randomVelocity();
            //newBoid.Velocity = new Vector2(5, 0);

            //newBoid.Rotation = MathHelper.ToRadians((float)rand.Next(0, 360));
            newBoid.Rotation = 0.0f;
            
            Boids.Add(newBoid);
        }

        public void Clear()
        {
            Boids.Clear();
        }

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

        private Vector2 randomLocation()
        {
            Vector2 location = Vector2.Zero;

            location.X = rand.Next(50, screenWidth - 50);
            location.Y = rand.Next(50, screenHeight - 50);

            return location;
        }

        private Vector2 randomVelocity()
        {
            Vector2 velocity = new Vector2(
            rand.Next(0, 101) - 50,
            rand.Next(0, 101) - 50);
            velocity.Normalize();
            velocity *= rand.Next(minSpeed, maxSpeed);
            return velocity;
        }

        private bool isOnScreen(Boid boid)
        {
            if (boid.sprite.Intersects(new Rectangle(0, 0, screenWidth, screenHeight)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Converts a Vector2 to a rotation 
        public float V2ToAngle(Vector2 direction)
        {
            //1.57079633 radians = 90 degrees
            return (float)Math.Atan2(direction.Y, direction.X) + 1.57079633f;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Boid boid in Boids)
            {
                //Rotation is based on velocity
                boid.Rotation = V2ToAngle(boid.Velocity);
                
                //If the sprite collides with the border, reverse velocity.
                if (boid.Position.X > screenWidth)
                {
                    Vector2 vel = boid.Velocity;
                    vel.X *= - 1;
                    boid.Velocity = vel;
                }
                if (boid.Position.X < 0)
                {
                    Vector2 vel = boid.Velocity;
                    vel.X *= -1;
                    boid.Velocity = vel;
                }
                if (boid.Position.Y > screenHeight)
                {
                    Vector2 vel = boid.Velocity;
                    vel.Y *= - 1;
                    boid.Velocity = vel;
                }
                if (boid.Position.Y < 0)
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
