using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Logging;
using RoomByRoom.Web.RequestService;
using RoomByRoom.Web.Sender;
using UnityEngine;
using Zenject;

namespace RoomByRoom.Web.Processor
{
    public class WebRequestProcessor : IWebRequestProcessor, IInitializable
    {
        private readonly IWebRequestService _webSvc;
        private readonly ICoroutineRunner _runner;
        private readonly IPersonalLogger _logger;
        private readonly List<IWebRequestSender> _requests = new List<IWebRequestSender>();
        private bool _hasConnection = true;

        public WebRequestProcessor(IWebRequestService webSvc, ICoroutineRunner runner, IPersonalLogger logger)
        {
            _webSvc = webSvc;
            _runner = runner;
            _logger = logger;
        }

        public void Initialize()
        {
            _runner.Run(RecoverConnectionRoutine());
        }

        public Coroutine Process(IWebRequestSender sender)
        {
            return _runner.Run(ProcessRequest(sender));
        }

        public async Task ProcessAsync(IWebRequestSender sender)
        {
            _requests.Add(sender);
            
            do
            {
                while (!_hasConnection)
                    await Task.Yield();

                await sender.SendRequestAsync();
                if (sender.IsConnectionError())
                {
                    RecoverConnection(true);
                }
            } while (sender.IsConnectionError());

            _requests.Remove(sender);
        }

        private IEnumerator ProcessRequest(IWebRequestSender sender)
        {
            _requests.Add(sender);
            
            do
            {
                yield return new WaitUntil(() => _hasConnection);
                yield return sender.SendRequest();
                if (sender.IsConnectionError())
                {
                    RecoverConnection(false);
                }
            } while (sender.IsConnectionError());

            _requests.Remove(sender);
        }

        private void RecoverConnection(bool async)
        {
            if (async)
                Task.Run(RecoverConnectionAsync);
            else
                _runner.Run(RecoverConnectionRoutine());
        }

        private IEnumerator RecoverConnectionRoutine()
        {
            _hasConnection = false;

            IWebRequestSender sender = _webSvc.CreateRequest(WebVerbs.HEAD, "alive");
            while (!sender.IsSuccess())
                yield return sender.SendRequest();

            _hasConnection = true;
        }

        private async Task RecoverConnectionAsync()
        {
            _hasConnection = false;
            
            IWebRequestSender sender = _webSvc.CreateRequest(WebVerbs.HEAD, "alive");
            while (!sender.IsSuccess())
                await sender.SendRequestAsync();

            _hasConnection = true;
        }
    }
}