using System.IO;

namespace Netronics.Microthreading.IO
{
    public static class NioStream
    {
        public static IYield NioRead(this Stream stream, byte[] buffer, int offset, int count)
        {
            var waitEvent = new WaitEvent();
            var thread = Microthread.CurrentMicrothread;
            stream.BeginRead(buffer, offset, count, ar =>
                                                        {
                                                            thread.Result = stream.EndRead(ar);
                                                            ((WaitEvent)ar.AsyncState).Set();
                                                        }, waitEvent);
            return Microthread.Wait(waitEvent);
        }

        public static IYield NioWrite(this Stream stream, byte[] buffer, int offset, int count)
        {
            var waitEvent = new WaitEvent();
            stream.BeginWrite(buffer, offset, count, ar =>
                                                         {
                                                             stream.EndWrite(ar);
                                                             ((WaitEvent) ar.AsyncState).Set();
                                                         }, waitEvent);

            return Microthread.Wait(waitEvent);
        }
    }
}
