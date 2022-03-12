using System;
using System.Linq.Expressions;
using CrudDatastore;

namespace Persistence.Specifications
{
    public class ActivitySpecs : Specification<Entities.Activity>
    {
        private ActivitySpecs(Expression<Func<Entities.Activity, bool>> predicate)
            : base(predicate)
        { }

        private ActivitySpecs(string command, params object[] parameters)
            : base(command, parameters)
        { }

        public static ActivitySpecs GetActivities()
        {
            return new ActivitySpecs(t => true);
        }
    }
}
