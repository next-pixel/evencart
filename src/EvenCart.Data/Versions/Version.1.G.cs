﻿using DotEntity;
using DotEntity.Versioning;
using EvenCart.Data.Entity.MediaEntities;
using Db = DotEntity.DotEntity.Database;
namespace EvenCart.Data.Versions
{
    public class Version1G : IDatabaseVersion
    {
        public void Upgrade(IDotEntityTransaction transaction)
        {
            Db.AddColumn<Media, string>(nameof(Media.MetaData), null, transaction);
        }

        public void Downgrade(IDotEntityTransaction transaction)
        {
            Db.DropColumn<Media>(nameof(Media.MetaData), transaction);
        }

        public string VersionKey => "EvenCart.Version.1G";

        
    }
}

