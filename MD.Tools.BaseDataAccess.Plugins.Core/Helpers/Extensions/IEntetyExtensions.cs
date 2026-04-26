using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Helpers.Extensions
{
    public static class IEntetyExtensions
    {
        public static IMethod<M, P> GetMethod<M, P>(this IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IMethod<M, P>> entity, Method inputMethod)
            where P : IMethodProperty
        {
            if (!typeof(M).IsEnum)
            {
                throw new ArgumentException("M must be an enumerated type");
            }
            IMethod<M, P> method = entity.Methods.FirstOrDefault(m => m.MethodInt.Equals(inputMethod.Id));
            return method;
        }

        public static IEventHandlerMethod<M, P> GetMethod<M, P>(this IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<M, P>> entity, Method inputMethod)
            where P : IMethodProperty
        {
            if (!typeof(M).IsEnum)
            {
                throw new ArgumentException("M must be an enumerated type");
            }
            IEventHandlerMethod<M, P> method = entity.Methods.FirstOrDefault(m => m.MethodInt.Equals(inputMethod.Id));
            IMethodStatus status = inputMethod;
            method.BindTaskStatus(ref status);
            return method;
        }

        public static IEventHandlerMethod<M, P> BindEventHandlers<M, P>(this IEventHandlerMethod<M, P> method, IEventHandlerEntity entity, Method inputMethod)
            where P : IMethodProperty
        {
            if (method != null && entity != null)
            {
                switch (inputMethod.MethodType)
                {
                    case Mapping.MethodTypes.Create:
                        if (entity.HasOnBeforeInsert)
                        {
                            method.OnBeforeExecute += entity.OnBeforeInsert;
                        }
                        if (entity.HasOnAfterInsert)
                        {
                            method.OnAfterExecute += entity.OnAfterInsert;
                        }
                        break;
                    case Mapping.MethodTypes.Update:
                        if (entity.HasOnBeforeUpdate)
                        {
                            method.OnBeforeExecute += entity.OnBeforeUpdate;
                        }
                        if (entity.HasOnAfterUpdate)
                        {
                            method.OnAfterExecute += entity.OnAfterUpdate;
                        }
                        break;
                    case Mapping.MethodTypes.Delete:
                        if (entity.HasOnBeforeDelete)
                        {
                            method.OnBeforeExecute += entity.OnBeforeDelete;
                        }
                        if (entity.HasOnAfterDelete)
                        {
                            method.OnAfterExecute += entity.OnAfterDelete;
                        }
                        break;
                    default:
                        if (entity.HasOnBeforeSelect)
                        {
                            method.OnBeforeExecute += entity.OnBeforeSelect;
                        }
                        if (entity.HasOnAfterSelect)
                        {
                            method.OnAfterExecute += entity.OnAfterSelect;
                        }
                        break;
                }
            }
            return method;
        }

        
        public static IEventHandlerMethod<M, P> BindEventHandlers<M, P>(this IEventHandlerMethod<M, P> method, IBaseDataAccessPlugin plugin, Method inputMethod)
            where P : IMethodProperty
        {
            return method.BindEventHandlers(plugin.EventHandlers.FirstOrDefault(e => e.Key == inputMethod.Entity).Value, inputMethod);
        }
    }
}
