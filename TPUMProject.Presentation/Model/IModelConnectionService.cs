﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Presentation.Model
{
    public interface IModelConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectionStateChanged;

        public event Action<string>? OnMessage;
        public event Action? OnError;

        public Task Connect(Uri uri);
        public Task Disconnect();

        public bool IsConnected();
    }
}
