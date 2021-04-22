﻿namespace BulletHell.Sprites
{
    using System;
    using System.Collections.Generic;
    using BulletHell.Sprites.Movement_Patterns;
    using BulletHell.Sprites.Projectiles;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal abstract class Attack : Sprite
    {
        public Projectile ProjectileToLaunch;
        public Sprite Attacker;
        protected double timer = 0;
        protected float projectileSpawnCooldown;

        public Attack(Projectile projectile, MovementPattern movement, float projectileSpawnCooldown)
            : base(null, Color.Transparent, movement)
        {
            this.ProjectileToLaunch = projectile;
            this.projectileSpawnCooldown = projectileSpawnCooldown;
        }

        protected virtual void CreateProjectile(List<Sprite> sprites)
        {
        }
    }
}
