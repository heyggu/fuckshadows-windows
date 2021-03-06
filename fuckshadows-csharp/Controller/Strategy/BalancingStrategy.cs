﻿using System;
using System.Net;
using Fuckshadows.Model;

namespace Fuckshadows.Controller.Strategy
{
    class BalancingStrategy : IStrategy
    {
        FuckshadowsController _controller;
        Random _random;

        public BalancingStrategy(FuckshadowsController controller)
        {
            _controller = controller;
            _random = new Random();
        }

        public string Name
        {
            get { return I18N.GetString("Load Balance"); }
        }

        public string ID
        {
            get { return "com.shadowsocks.strategy.balancing"; }
        }

        public void ReloadServers()
        {
            // do nothing
        }

        public Server GetAServer(IStrategyCallerType type, IPEndPoint localIPEndPoint, EndPoint destEndPoint)
        {
            var configs = _controller.GetCurrentConfiguration().configs;
            int index;
            if (type == IStrategyCallerType.TCP)
            {
                index = _random.Next();
            }
            else
            {
                index = localIPEndPoint.GetHashCode();
            }
            return configs[index % configs.Count];
        }

        public void UpdateLatency(Model.Server server, TimeSpan latency)
        {
            // do nothing
        }

        public void UpdateLastRead(Model.Server server)
        {
            // do nothing
        }

        public void UpdateLastWrite(Model.Server server)
        {
            // do nothing
        }

        public void SetFailure(Model.Server server)
        {
            // do nothing
        }
    }
}
