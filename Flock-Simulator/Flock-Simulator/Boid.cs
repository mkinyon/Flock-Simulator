using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlockTest
{
    class Boid
    {
        //Declarations 
        Texture2D texture;

        private Color tintColor = Color.White;
        private float rotation = 0.0f;

        protected Vector2 position = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;

        Random rand = new Random();
        public Rectangle sprite = new Rectangle(0, 0, 16, 16);
        public Vector2 origin = Vector2.Zero;


        /// <summary>
        /// Constructor for Boid
        /// </summary>
        /// <param name="position">Object's location on the screen</param>
        /// <param name="texture">Boid texture</param>
        /// <param name="velocity">Initial velocity</param>
        public Boid(Vector2 position, Texture2D texture, Vector2 velocity)
        {
            this.position = position;
            this.texture = texture;
            this.velocity = velocity;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// Updates the Boid's position based on velocity.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += (velocity * elapsed);
        }
 
        /// <summary>
        /// Adds the boid to the spriteBatch to be later drawn on screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sprite, tintColor, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
        }

    }
}
