using System;
using CrudDatastore;

namespace Persistence
{
    public interface ITodoContextFactory
    {
        DataContextBase CreateDataContext();
    }
}
