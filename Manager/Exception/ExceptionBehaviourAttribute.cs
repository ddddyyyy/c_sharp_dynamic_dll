﻿using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Manager.Handler
{
    /// <summary>
    /// 异常处理注解
    /// </summary>
    public class ExceptionBehaviourAttribute : Attribute, IServiceBehavior
    {
        private readonly Type _errorHandlerType;

        public ExceptionBehaviourAttribute(Type errorHandlerType)
        {
            _errorHandlerType = errorHandlerType;
        }

        #region IServiceBehavior Members

        public void Validate(ServiceDescription description,
        ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription description,
        ServiceHostBase serviceHostBase,
        Collection<ServiceEndpoint> endpoints,
        BindingParameterCollection parameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription description,
        ServiceHostBase serviceHostBase)
        {
            var handler =
            (IErrorHandler)Activator.CreateInstance(_errorHandlerType);

            foreach (ChannelDispatcherBase dispatcherBase in
            serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = dispatcherBase as ChannelDispatcher;
                if (channelDispatcher != null)
                    channelDispatcher.ErrorHandlers.Add(handler);
            }
        }

        #endregion
    }
}
