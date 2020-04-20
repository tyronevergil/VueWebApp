using System;
using CrudDatastore;

namespace Persistence
{
    public interface IMessageContextFactory
    {
        DataContextBase CreateDataContext();
    }
}
