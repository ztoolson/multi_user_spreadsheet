// Zach Toolson, Michael Call, Mike Fleming, Mark Mouritsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CustomNetworking
{
    /// <summary> 
    /// A StringSocket is a wrapper around a Socket.  It provides methods that
    /// asynchronously read lines of text (strings terminated by newlines) and 
    /// write strings. (As opposed to Sockets, which read and write raw bytes.)  
    ///
    /// StringSockets are thread safe.  This means that two or more threads may
    /// invoke methods on a shared StringSocket without restriction.  The
    /// StringSocket takes care of the synchonization.
    /// 
    /// Each StringSocket contains a Socket object that is provided by the client.  
    /// A StringSocket will work properly only if the client refrains from calling
    /// the contained Socket's read and write methods.
    /// 
    /// If we have an open Socket s, we can create a StringSocket by doing
    /// 
    ///    StringSocket ss = new StringSocket(s, new UTF8Encoding());
    /// 
    /// We can write a string to the StringSocket by doing
    /// 
    ///    ss.BeginSend("Hello world", callback, payload);
    ///    
    /// where callback is a SendCallback (see below) and payload is an arbitrary object.
    /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
    /// successfully written the string to the underlying Socket, or failed in the 
    /// attempt, it invokes the callback.  The parameters to the callback are a
    /// (possibly null) Exception and the payload.  If the Exception is non-null, it is
    /// the Exception that caused the send attempt to fail.
    /// 
    /// We can read a string from the StringSocket by doing
    /// 
    ///     ss.BeginReceive(callback, payload)
    ///     
    /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
    /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
    /// string of text terminated by a newline character from the underlying Socket, or
    /// failed in the attempt, it invokes the callback.  The parameters to the callback are
    /// a (possibly null) string, a (possibly null) Exception, and the payload.  Either the
    /// string or the Exception will be non-null, but nor both.  If the string is non-null, 
    /// it is the requested string (with the newline removed).  If the Exception is non-null, 
    /// it is the Exception that caused the send attempt to fail.
    /// </summary>

    public class StringSocket
    {
        // These delegates describe the callbacks that are used for sending and receiving strings.
        public delegate void SendCallback(Exception e, object payload);
        public delegate void ReceiveCallback(String s, Exception e, object payload);

        // Member data
        private Socket socket;
        private Encoding encoding;

        // Data for sending messages

        // Structure to store incoming BeginSend requests
        private struct SendRequest
        {
            public String s;
            public SendCallback cb;
            public object payload;
        }

        private Queue<SendRequest> SendRequestQueue;

        private byte[] SendBuffer;

        private string OutgoingMessage = "";

        // Properties and data for incoming messages

        private struct ReceiveRequest
        {
            public ReceiveCallback cb;
            public object payload;
        }

        // Keep track of requests to receive data
        private Queue<ReceiveRequest> ReceiveRequestQueue;

        // Keep track of received messages
        private Queue<string> ReceivedMessages;

        // Maintain a partial method between calls
        private string IncomingMessage = "";

        /// <summary>
        /// Gives the status of the socket.
        /// </summary>
        public Boolean Connected
        {
            get
            {
                return socket.Connected;
            }
        }

        /// <summary>
        /// Creates a StringSocket from a regular Socket, which should already be connected.  
        /// The read and write methods of the regular Socket must not be called after the
        /// LineSocket is created.  Otherwise, the StringSocket will not behave properly.  
        /// The encoding to use to convert between raw bytes and strings is also provided.
        /// </summary>
        public StringSocket(Socket s, Encoding e)
        {
            // Store the socket and encoding
            this.socket = s;
            this.encoding = e;

            // Create a new empty send request queue
            SendRequestQueue = new Queue<SendRequest>();

            // Initialize the receive request and received string buffers
            ReceiveRequestQueue = new Queue<ReceiveRequest>();
            ReceivedMessages = new Queue<string>();
        }

        /// <summary>
        /// We can write a string to a StringSocket ss by doing
        /// 
        ///    ss.BeginSend("Hello world", callback, payload);
        ///    
        /// where callback is a SendCallback (see below) and payload is an arbitrary object.
        /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
        /// successfully written the string to the underlying Socket, or failed in the 
        /// attempt, it invokes the callback.  The parameters to the callback are a
        /// (possibly null) Exception and the payload.  If the Exception is non-null, it is
        /// the Exception that caused the send attempt to fail. 
        /// 
        /// This method is non-blocking.  This means that it does not wait until the string
        /// has been sent before returning.  Instead, it arranges for the string to be sent
        /// and then returns.  When the send is completed (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginSend
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginSend must take care of synchronization instead.  On a given StringSocket, each
        /// string arriving via a BeginSend method call must be sent (in its entirety) before
        /// a later arriving string can be sent.
        /// </summary>
        public void BeginSend(String s, SendCallback callback, object payload)
        {
            lock (SendRequestQueue)
            {
                // Package this request
                SendRequest sr = new SendRequest();
                sr.s = s;
                sr.cb = callback;
                sr.payload = payload;
                // If the SendRequestQueue is empty, then the socket
                // is not currently sending bytes
                if (SendRequestQueue.Count == 0)
                {
                    // Enqueue the send request
                    SendRequestQueue.Enqueue(sr);
                    // Launch SendBytes()
                    OutgoingMessage = sr.s;
                    SendBytes();
                }
                // Otherwise sending is ongoing. Just register the request.
                else
                {
                    SendRequestQueue.Enqueue(sr);
                }
            }
        }


        /// <summary>
        /// Sends encoded messages over the socket
        /// </summary>
        private void SendBytes()
        {
            lock (SendRequestQueue)
            {
                // We have finished sending. Cleanup the current request.
                if (OutgoingMessage == "")
                {
                    SendRequest r = SendRequestQueue.Peek();
                    // Service the callback
                    ThreadPool.QueueUserWorkItem((x) => { r.cb(null, r.payload); });
                    // Remove the current request from the queue
                    SendRequestQueue.Dequeue();
                    // If there are no pending requests, exit
                    if (SendRequestQueue.Count == 0)
                        return;
                    // Otherwise, start sending the next message.
                    else
                    {
                        OutgoingMessage = SendRequestQueue.Peek().s;
                        SendBytes();
                    }
                }
                // Start or continue the current outgoing message
                else
                {
                    SendBuffer = (byte[])encoding.GetBytes(OutgoingMessage);
                    OutgoingMessage = "";
                    socket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, MessageSent, SendBuffer);
                }
            }
        }

        /// <summary>
        /// Called when the socket has sent some bytes
        /// </summary>
        /// <param name="result"></param>
        private void MessageSent(IAsyncResult result)
        {
            lock (SendRequestQueue)
            {
                // Find out how many bytes were actually sent
                int bytes = socket.EndSend(result);

                // Get exclusive access to send mechanism
                // Get the bytes that we attempted to send
                byte[] outgoingBuffer = (byte[])result.AsyncState;

                // The socket has been closed
                if (bytes == 0)
                {
                    socket.Close();
                    return;
                }

                // Prepend the unsent bytes and try sending again.
                else
                {
                    OutgoingMessage = encoding.GetString(outgoingBuffer, bytes, outgoingBuffer.Length - bytes) + OutgoingMessage;
                    SendBytes();
                }
            }
        }

        /// <summary>
        /// 
        /// <para>
        /// We can read a string from the StringSocket by doing
        /// </para>
        /// 
        /// <para>
        ///     ss.BeginReceive(callback, payload)
        /// </para>
        /// 
        /// <para>
        /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
        /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
        /// string of text terminated by a newline character from the underlying Socket, or
        /// failed in the attempt, it invokes the callback.  The parameters to the callback are
        /// a (possibly null) string, a (possibly null) Exception, and the payload.  Either the
        /// string or the Exception will be non-null, but nor both.  If the string is non-null, 
        /// it is the requested string (with the newline removed).  If the Exception is non-null, 
        /// it is the Exception that caused the send attempt to fail.
        /// </para>
        /// 
        /// <para>
        /// This method is non-blocking.  This means that it does not wait until a line of text
        /// has been received before returning.  Instead, it arranges for a line to be received
        /// and then returns.  When the line is actually received (at some time in the future), the
        /// callback is called on another thread.
        /// </para>
        /// 
        /// <para>
        /// This method is thread safe.  This means that multiple threads can call BeginReceive
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginReceive must take care of synchronization instead.  On a given StringSocket, each
        /// arriving line of text must be passed to callbacks in the order in which the corresponding
        /// BeginReceive call arrived.
        /// </para>
        /// 
        /// <para>
        /// Note that it is possible for there to be incoming bytes arriving at the underlying Socket
        /// even when there are no pending callbacks.  StringSocket implementations should refrain
        /// from buffering an unbounded number of incoming bytes beyond what is required to service
        /// the pending callbacks.        
        /// </para>
        /// 
        /// <param name="callback"> The function to call upon receiving the data</param>
        /// <param name="payload"> 
        /// The payload is "remembered" so that when the callback is invoked, it can be associated
        /// with a specific Begin Receiver....
        /// </param>  
        /// 
        /// <example>
        ///   Here is how you might use this code:
        ///   <code>
        ///                    client = new TcpClient("localhost", port);
        ///                    Socket       clientSocket = client.Client;
        ///                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());
        ///                    receiveSocket.BeginReceive(CompletedReceive1, 1);
        /// 
        ///   </code>
        /// </example>
        /// </summary>
        /// 
        /// 
        public void BeginReceive(ReceiveCallback callback, object payload)
        {
            lock (ReceiveRequestQueue)
            {

                ReceiveRequest rr = new ReceiveRequest();
                rr.cb = callback;
                rr.payload = payload;
                ServicePendingRequests();
                if (ReceiveRequestQueue.Count == 0)
                {
                    ReceiveRequestQueue.Enqueue(rr);
                    // Begin receiving messages
                    byte[] buffer = new byte[1024];
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, MessageReceived, buffer);
                }
                else
                {
                    ReceiveRequestQueue.Enqueue(rr);
                }
            }
        }

        /// <summary>
        /// Services the pending receive requests.
        /// </summary>
        private void ServicePendingRequests()
        {
            lock (ReceiveRequestQueue)
            {
                // Since this method is only called when data is received or on a BeginReceive we need
                // to leave this method only when there are either no more pending callbacks to service
                // or there are no more buffered strings to service the pending call backs. This
                // invariant will be enforced by draining the shorter of the two queues
                // ReceivedMessages and ReceiveRequestQueue.

                // Service pending callbacks. We service the minimum of the length of the request queue
                // and the length of the received message queue, i.e., while both queues are not empty.
                while (ReceivedMessages.Count != 0 && ReceiveRequestQueue.Count != 0)
                {
                    ReceiveRequest rr = ReceiveRequestQueue.Dequeue();
                    string s = ReceivedMessages.Dequeue();
                    ThreadPool.QueueUserWorkItem(x => rr.cb(s, null, rr.payload));
                }
            }
        }
        /// <summary>
        /// Called whenever data on the underlying socket is received. Extracts messages from 
        /// the received data and triggers the callbacks to be serviced.
        /// </summary>
        /// <param name="result"></param>
        private void MessageReceived(IAsyncResult result)
        {
            lock (ReceiveRequestQueue)
            {
                // Get the buffer to which the data was written.
                byte[] buffer = (byte[])(result.AsyncState);
                int bytes = 0;
                // Figure out how many bytes have come in
                try
                {
                    bytes = socket.EndReceive(result);
                }
                catch
                {

                }

                // If no bytes were received, it means the client closed its side of the socket.
                // Close our socket.
                if (bytes == 0)
                {
                    socket.Close();
                    return;
                }

                // Otherwise, decode and use the incoming bytes. Service callbacks and request more data
                else
                {
                    // Convert the bytes into a string
                    IncomingMessage += encoding.GetString(buffer, 0, bytes);

                    // Extract and record messages
                    int index;
                    while ((index = IncomingMessage.IndexOf('\n')) >= 0)
                    {
                        String line = IncomingMessage.Substring(0, index);
                        IncomingMessage = IncomingMessage.Substring(index + 1);
                        ReceivedMessages.Enqueue(line);
                    }


                    // See if we can service some callbacks with the received messages
                    ServicePendingRequests();

                    // Check if there are pending requests...
                    if (ReceiveRequestQueue.Count != 0)
                    {
                        // Pending requests.
                        // Ask for some more data.
                        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, MessageReceived, buffer);
                    }
                    // If not, stop listening.
                    else
                        return;
                }
            }
        }

        /// <summary>
        /// Closes this StringSocket
        /// </summary>
        public void Close()
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close(0);
            }
        }
    }
}
