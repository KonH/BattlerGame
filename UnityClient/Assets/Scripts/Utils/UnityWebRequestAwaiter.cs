using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityClient.Utils {
    public sealed class UnityWebRequestAwaiter : INotifyCompletion {
        UnityWebRequestAsyncOperation _asyncOp;
        Action                        _continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp) {
            _asyncOp = asyncOp;
            _asyncOp.completed += OnRequestCompleted;
        }

        public bool IsCompleted {
            get { return _asyncOp.isDone; }
        }

        public void GetResult() {}

        public void OnCompleted(Action continuation) {
            _continuation = continuation;
        }

        void OnRequestCompleted(AsyncOperation obj) {
            _continuation();
        }
    }

    public static class UnityWebRequestAwaiterExtension {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp) {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }
}
