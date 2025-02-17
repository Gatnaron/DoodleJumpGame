﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleJumpGame.Classes
{
    public class Physics
    {
        public Transform transform;
        public float gravity;
        float a;

        public float dx;
        public bool usedBonus = false;

        public Physics(PointF position, Size size)
        {
            transform = new Transform(position, size);
            gravity = 0;
            a = 0.4f;
            dx = 0;
        }

        public void ApplyPhysics()
        {
            CalculatePhysics();
        }
        public void CalculatePhysics()
        {
            if(dx != 0)
            {
                transform.position.X += dx;
            }
            if(transform.position.Y < 700)
            {
                transform.position.Y += gravity;
                gravity += a;

                if (gravity > -25 && usedBonus)
                {
                    PlatformController.GenerateStartSequence();
                    PlatformController.GenerateRandomPlatform();
                    PlatformController.startPlatformPosY = -340;
                    PlatformController.GenerateStartSequence();
                    PlatformController.startPlatformPosY = 0;
                    usedBonus = false;
                }
                Collide();
            }
        }

        public bool StandartCollidePlayerWithObjects(bool forEnemies, bool forBonuses)
        {
            if (forEnemies)
            {
                for (int i = 0; i < PlatformController.enemies.Count; i++)
                {
                    var enemy = PlatformController.enemies[i];
                    PointF delta = new PointF();
                    delta.X = (transform.position.X + transform.size.Width / 2) - (enemy.physics.transform.position.X + enemy.physics.transform.size.Width / 2);
                    delta.Y = (transform.position.Y + transform.size.Height / 2) - (enemy.physics.transform.position.Y + enemy.physics.transform.size.Height / 2);
                    if (Math.Abs(delta.X) <= transform.size.Height / 2 + enemy.physics.transform.size.Height / 2)
                    {
                        if (Math.Abs(delta.Y) <= transform.size.Height / 2 + enemy.physics.transform.size.Height / 2)
                        {
                            if (!usedBonus)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            if (forBonuses)
            {
                for (int i = 0; i < PlatformController.bonuses.Count; i++)
                {
                    var bonus = PlatformController.bonuses[i];
                    PointF delta = new PointF();
                    delta.X = (transform.position.X + transform.size.Width / 2) - (bonus.physics.transform.position.X + bonus.physics.transform.size.Width / 2);
                    delta.Y = (transform.position.Y + transform.size.Height / 2) - (bonus.physics.transform.position.Y + bonus.physics.transform.size.Height / 2);
                    if (Math.Abs(delta.X) <= transform.size.Height / 2 + bonus.physics.transform.size.Height / 2)
                    {
                        if (Math.Abs(delta.Y) <= transform.size.Height / 2 + bonus.physics.transform.size.Height / 2)
                        {
                            if(bonus.type == 1 && !usedBonus)
                            {
                                usedBonus = true;
                                PlatformController.score += 50;
                                AddForce(-30);
                            }
                            if (bonus.type == 2 && !usedBonus)
                            {
                                usedBonus = true;
                                PlatformController.score += 100;
                                AddForce(-60);
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool StandartCollide()
        {
            for(int i = 0; i < PlatformController.bullets.Count; i++)
            {
                var bullet = PlatformController.bullets[i];
                PointF delta = new PointF();
                delta.X = (transform.position.X + transform.size.Width/2) - (bullet.physics.transform.position.X + bullet.physics.transform.size.Width/2);
                delta.Y = (transform.position.Y + transform.size.Height/2) - (bullet.physics.transform.position.Y + bullet.physics.transform.size.Height/2);
                if (Math.Abs(delta.X) <= transform.size.Height / 2 + bullet.physics.transform.size.Height / 2)
                {
                    if (Math.Abs(delta.Y) <= transform.size.Height/2 + bullet.physics.transform.size.Height/2)
                    {
                        PlatformController.RemoveBullet(i);
                        return true;
                    }
                }
            }
            return false;
        }

        public void Collide()
        {
            for(int i = 0; i < PlatformController.platforms.Count; i++)
            {
                var platform = PlatformController.platforms[i];
                if(transform.position.X+transform.size.Width/2 >= platform.transform.position.X && transform.position.X + transform.size.Width/2 <= platform.transform.position.X + platform.transform.size.Width)
                {
                    if(transform.position.Y+transform.size.Height >= platform.transform.position.Y && transform.position.Y + transform.size.Height <= platform.transform.position.Y + platform.transform.size.Height)
                    {
                        if(gravity > 0)
                        {
                            AddForce();
                            if (!platform.isTouchedByPlayer)
                            {
                                PlatformController.score += 20;
                                PlatformController.GenerateRandomPlatform();
                                platform.isTouchedByPlayer = true;
                            }
                        }
                    }
                }
            }
        }

        public void AddForce(int force = -10)
        {
            gravity = force;
        }
    }
}
