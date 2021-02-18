﻿namespace BulletHell.Sprites
{
    using System;
    using System.Collections.Generic;
    using global::BulletHell.Sprites.Entities;

    internal class Wave
    {
        int waveNumber;
        int waveDuration;
        TimeSpan startTime;
        List<EntityGroup> entityGroups = new List<EntityGroup>();

        public Wave(Dictionary<string, object> waveProperties)
        {
            this.waveNumber = (int)waveProperties["waveNumber"];
            this.waveDuration = (int)waveProperties["waveDuration"];

            foreach (Dictionary<string, object> entityGroupProperties in (List<Dictionary<string, object>>)waveProperties["entityGroups"])
            {
                this.entityGroups.Add(new EntityGroup(entityGroupProperties));
            }
        }

        public void CreateWave(List<Sprite> sprites)
        {
            foreach (EntityGroup entityGroup in this.entityGroups)
            {
                entityGroup.CreateEntities(sprites);
            }
        }
    }
}
