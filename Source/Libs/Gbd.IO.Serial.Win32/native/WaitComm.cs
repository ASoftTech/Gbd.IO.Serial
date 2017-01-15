using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

// TODO Eventing

namespace Gbd.IO.Serial.Win32.native
{
    public class WaitComm
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern unsafe bool GetOverlappedResult(SafeFileHandle hFile, NativeOverlapped* lpOverlapped, ref int lpNumberOfBytesTransferred, bool bWait);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern unsafe bool WaitCommEvent(SafeFileHandle hFile, int* lpEvtMask, NativeOverlapped* lpOverlapped);

        public const int ERROR_ACCESS_DENIED = 5;
        public const int ERROR_BAD_COMMAND = 22;
        public const int ERROR_INVALID_PARAMETER = 87;
        public const int ERROR_IO_INCOMPLETE = 996;
        public const int ERROR_IO_PENDING = 997;

        public SafeFileHandle Handle;

        internal bool endEventLoop;
        IOCompletionCallback freeNativeOverlappedCallback;
        internal ManualResetEvent eventLoopEndedSignal = new ManualResetEvent(false);
        internal ManualResetEvent waitCommEventWaitHandle = new ManualResetEvent(false);
        private int eventsOccurred;

        /// <summary> Default Constructor. </summary>
        internal unsafe WaitComm() {
            endEventLoop = false;
            freeNativeOverlappedCallback = new IOCompletionCallback(FreeNativeOverlappedCallback);
        }

        internal unsafe void WaitForCommEvent() {
            int unused = 0;
            bool doCleanup = false;
            NativeOverlapped* intOverlapped = null;
            while (!endEventLoop) {
                SerialStreamAsyncResult asyncResult = null;

                asyncResult = new SerialStreamAsyncResult();
                asyncResult._userCallback = null;
                asyncResult._userStateObject = null;
                asyncResult._isWrite = false;

                // we're going to use _numBytes for something different in this loop.  In this case, both 
                // freeNativeOverlappedCallback and this thread will decrement that value.  Whichever one decrements it
                // to zero will be the one to free the native overlapped.  This guarantees the overlapped gets freed
                // after both the callback and GetOverlappedResult have had a chance to use it. 
                asyncResult._numBytes = 2;
                asyncResult._waitHandle = waitCommEventWaitHandle;

                waitCommEventWaitHandle.Reset();
                Overlapped overlapped = new Overlapped(0, 0, waitCommEventWaitHandle.SafeWaitHandle.DangerousGetHandle(), asyncResult);
                // Pack the Overlapped class, and store it in the async result
                intOverlapped = overlapped.Pack(freeNativeOverlappedCallback, null);
                

                fixed (int* eventsOccurredPtr = &eventsOccurred)
                {

                    if (WaitCommEvent(Handle, eventsOccurredPtr, intOverlapped) == false) {
                        int hr = Marshal.GetLastWin32Error();
                        // When a device is disconnected unexpectedly from a serial port, there appear to be
                        // at least two error codes Windows or drivers may return.
                        if (hr == ERROR_ACCESS_DENIED || hr == ERROR_BAD_COMMAND) {
                            doCleanup = true;
                            break;
                        }
                        if (hr == ERROR_IO_PENDING) {
                            int error;

                            // if we get IO pending, MSDN says we should wait on the WaitHandle, then call GetOverlappedResult
                            // to get the results of WaitCommEvent. 
                            bool success = waitCommEventWaitHandle.WaitOne();
                            Debug.Assert(success, "waitCommEventWaitHandle.WaitOne() returned error " + Marshal.GetLastWin32Error());

                            do {
                                // NOTE: GetOverlappedResult will modify the original pointer passed into WaitCommEvent.
                                success = GetOverlappedResult(Handle, intOverlapped, ref unused, false);
                                error = Marshal.GetLastWin32Error();
                            }
                            while (error == ERROR_IO_INCOMPLETE && !endEventLoop && !success);

                            if (!success) {
                                // Ignore ERROR_IO_INCOMPLETE and ERROR_INVALID_PARAMETER, because there's a chance we'll get
                                // one of those while shutting down 
                                if (!((error == ERROR_IO_INCOMPLETE || error == ERROR_INVALID_PARAMETER) && endEventLoop))
                                    Debug.Assert(false, "GetOverlappedResult returned error, we might leak intOverlapped memory" + error.ToString(CultureInfo.InvariantCulture));
                            }
                        }
                        else if (hr != ERROR_INVALID_PARAMETER) {
                            // ignore ERROR_INVALID_PARAMETER errors.  WaitCommError seems to return this
                            // when SetCommMask is changed while it's blocking (like we do in Dispose())
                            Debug.Assert(false, "WaitCommEvent returned error " + hr);
                        }
                    }
                }

                if (!endEventLoop)
                    CallEvents(eventsOccurred);

                if (Interlocked.Decrement(ref asyncResult._numBytes) == 0)
                    Overlapped.Free(intOverlapped);
              
            } // while (!ShutdownLoop)

            if (doCleanup) {
                // the rest will be handled in Dispose()
                endEventLoop = true;
                Overlapped.Free(intOverlapped);
            }
            eventLoopEndedSignal.Set();
        }

        private void CallEvents(int nativeEvents) {
            //// EV_ERR includes only CE_FRAME, CE_OVERRUN, and CE_RXPARITY
            //// To catch errors such as CE_RXOVER, we need to call CleanCommErrors bit more regularly. 
            //// EV_RXCHAR is perhaps too loose an event to look for overflow errors but a safe side to err...
            //if ((nativeEvents & (NativeMethods.EV_ERR | NativeMethods.EV_RXCHAR)) != 0) {
            //    int errors = 0;
            //    if (UnsafeNativeMethods.ClearCommError(handle, ref errors, IntPtr.Zero) == false) {

            //        //InternalResources.WinIOError();

            //        // We don't want to throw an exception from the background thread which is un-catchable and hence tear down the process.
            //        // At present we don't have a first class event that we can raise for this class of fatal errors. One possibility is 
            //        // to overload SeralErrors event to include another enum (perhaps CE_IOE) that we can use for this purpose. 
            //        // In the absene of that, it is better to eat this error silently than tearing down the process (lesser of the evil). 
            //        // This uncleared comm error will most likely ---- up when the device is accessed by other APIs (such as Read) on the 
            //        // main thread and hence become known. It is bit roundabout but acceptable.  
            //        //  
            //        // Shutdown the event runner loop (probably bit drastic but we did come across a fatal error). 
            //        // Defer actual dispose chores until finalization though. 
            //        endEventLoop = true;
            //        Thread.MemoryBarrier();
            //        return;
            //    }

            //    errors = errors & errorEvents;
            //    // 



            //    if (errors != 0) {
            //        ThreadPool.QueueUserWorkItem(callErrorEvents, errors);
            //    }
            //}

            //// now look for pin changed and received events.
            //if ((nativeEvents & pinChangedEvents) != 0) {
            //    ThreadPool.QueueUserWorkItem(callPinEvents, nativeEvents);
            //}

            //if ((nativeEvents & receivedEvents) != 0) {
            //    ThreadPool.QueueUserWorkItem(callReceiveEvents, nativeEvents);
            //}
        }

        private unsafe void FreeNativeOverlappedCallback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped) {
            // Unpack overlapped
            Overlapped overlapped = Overlapped.Unpack(pOverlapped);

            // Extract the async result from overlapped structure
            SerialStreamAsyncResult asyncResult =
                (SerialStreamAsyncResult)overlapped.AsyncResult;

            if (Interlocked.Decrement(ref asyncResult._numBytes) == 0)
                Overlapped.Free(pOverlapped);
        }

        // This is an internal object implementing IAsyncResult with fields
        // for all of the relevant data necessary to complete the IO operation.
        // This is used by AsyncFSCallback and all async methods.
        unsafe internal sealed class SerialStreamAsyncResult : IAsyncResult {
            // User code callback
            internal AsyncCallback _userCallback;

            internal Object _userStateObject;

            internal bool _isWrite;     // Whether this is a read or a write
            internal bool _isComplete;
            internal bool _completedSynchronously;  // Which thread called callback

            internal ManualResetEvent _waitHandle;
            internal int _EndXxxCalled;   // Whether we've called EndXxx already.
            internal int _numBytes;     // number of bytes read OR written
            internal int _errorCode;
            internal NativeOverlapped* _overlapped;

            public Object AsyncState {
                get { return _userStateObject; }
            }

            public bool IsCompleted {
                get { return _isComplete; }
            }

            public WaitHandle AsyncWaitHandle {
                get {
                    /*
                      // Consider uncommenting this someday soon - the EventHandle 
                      // in the Overlapped struct is really useless half of the 
                      // time today since the OS doesn't signal it.  If users call
                      // EndXxx after the OS call happened to complete, there's no
                      // reason to create a synchronization primitive here.  Fixing
                      // this will save us some perf, assuming we can correctly
                      // initialize the ManualResetEvent. 
                    if (_waitHandle == null) {
                        ManualResetEvent mre = new ManualResetEvent(false);
                        if (_overlapped != null && _overlapped->EventHandle != IntPtr.Zero)
                            mre.Handle = _overlapped->EventHandle;
                        if (_isComplete)
                            mre.Set();
                        _waitHandle = mre;
                    }
                    */
                    return _waitHandle;
                }
            }

            // Returns true iff the user callback was called by the thread that
            // called BeginRead or BeginWrite.  If we use an async delegate or
            // threadpool thread internally, this will be false.  This is used
            // by code to determine whether a successive call to BeginRead needs
            // to be done on their main thread or in their callback to avoid a
            // stack overflow on many reads or writes.
            public bool CompletedSynchronously {
                get { return _completedSynchronously; }
            }
        }
    }


}
