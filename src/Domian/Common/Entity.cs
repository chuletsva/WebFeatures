﻿using MediatR;
using System;
using System.Collections.Generic;

namespace Domian.Common
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        public ICollection<INotification> Events { get; private set; }

        protected Entity()
        {
            Events = new HashSet<INotification>();
        }
    }
}
