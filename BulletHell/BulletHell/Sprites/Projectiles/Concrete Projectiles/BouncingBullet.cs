﻿namespace BulletHell.Sprites.Projectiles.Concrete_Projectiles
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using global::BulletHell.Sprites.Movement_Patterns;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal class BouncingBullet : Projectile
    {
        // max bounce number
        private int maxBounces = 5;

        public BouncingBullet(Texture2D texture, Color color, MovementPattern movement)
            : base(texture, color, movement)
        {
        }

        public override void Move()
        {
            // if you can still bounce
            if (this.maxBounces != 0)
            {
                // when bullet is out of bounds reverse its velocity and reduce maxBounces
                if (this.OutOfBounds())
                {
                    this.Movement.Velocity = -this.Movement.Velocity;
                    this.maxBounces--;
                }
            }

            // if you can no longer bounce remove the bullet when it is out of bounds
            else
            {
                if (this.OutOfBounds())
                {
                    this.IsRemoved = true;
                }
            }

            this.Movement.Move();
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            base.Update(gameTime, sprites);
        }
    }
}