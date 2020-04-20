using System;
using System.Collections.Generic;
using System.Linq;
using CrudDatastore;

namespace Persistence
{
    public class MessageContextFactory : IMessageContextFactory
    {
        public DataContextBase CreateDataContext()
        {
            return new GenericDataContext(new MessageUnitOfWorkInMemory());
        }
    }

    internal class MessageUnitOfWorkInMemory : UnitOfWorkBase
    {
        private static readonly IList<Entities.Activity> _activities = new List<Entities.Activity>();

        private IDataStore<Entities.Activity> Activities()
        {
            return new DataStore<Entities.Activity>(
                new DelegateCrudAdapter<Entities.Activity>(
                    /* create */
                    (e) =>
                    {
                        e.Id = (_activities.Any() ? _activities.Max(i => i.Id) : 0) + 1;
                        _activities.Add(new Entities.Activity
                        {
                            Id = e.Id,
                            Type = e.Type,
                            Payload = e.Payload
                        });
                    },

                    /* update */
                    (e) =>
                    {
                    },

                    /* delete */
                    (e) =>
                    {
                    },

                    /* read */
                    (predicate) =>
                    {
                        return _activities.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        public MessageUnitOfWorkInMemory()
        {
            this.Register(Activities());
        }
    }
}
