using Newtonsoft.Json;
using Potestas.Exceptions.ProcessorExceptions;
using System;
using System.IO;

namespace Potestas.Processors
{
    /* TASK. Implement processor which serializes IEnergyObservation to the provided stream.
     * 1. Use serialization mechanism here. 
     * 2. Some IEnergyObservation could not be serializable.
     */
    public class SerializeProcessor<T> : IStreamProcessor<T> where T : IEnergyObservation
    {
        private Stream _stream;

        public string Description => "Serialize observations to the provided stream.";

        /// <summary>
        /// Create instance of SerializeProcessor.
        /// Use this ctor when want to specify a stream later in the onNextMethod().
        /// </summary>
        public SerializeProcessor() { }

        /// <summary>
        /// Create instance of SerializeProcessor with the given stream.
        /// </summary>
        /// <param name="stream">he stream for serialization process.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="stream"/> is null.
        /// </exception>
        public SerializeProcessor(Stream stream)
        {
            _stream = stream ?? throw new ArgumentNullException($"The {nameof(stream)} can not be null.");
        }

        public void OnCompleted()
        {
            if (_stream != null)
            {
                _stream.Dispose(); // ask : should we dispose the stream here? 
                                   // what if the observable starts a new cycly of emit data, 
                                   // but stream is closed -> data will not be processed in case when strem is passed in ctor and used in OnNext(T value)
            }
        }

        public void OnError(Exception error)
        {
            throw new SerializeProcessorException($"Error in {Description}", error);
        }

        public void OnNext(T value)
        {
            OnNext(_stream, value);
        }

        public void OnNext(Stream stream, T value)
        {
            stream = stream ?? throw new ArgumentNullException($"The {nameof(stream)} can not be null.");

            string jsonValue = "";

            try
            {
                jsonValue = JsonConvert.SerializeObject(value); //if value = null -> jsonValue - "null"
            }
            catch (Exception exception)
            {
                throw new SerializeProcessorException($"Can not serialize value: {value}.", exception);
            }


            // Albahari - Chapter 15(p. 629) ----Closing a decorator stream closes both the decorator and its backing store stream.
            // With a chain of decorators, closing the outermost decorator(at the head of the chain) closes the whole lot.
            // Albahari - Chapter 15(p. 646) -----Because the nest disposes from the inside out, the adapter is first closed, and then
            // the stream. Furthermore, if an exception is thrown within the adapter’s constructor,the stream still closes

            // Albahari - Chapter 15(p. 629) The Flush method forces any internally buffered data to be written immediately.
            // Flush is called automatically when a stream is closed. 


            // for avoiding close input stream (access to it may be required in the calling code) 
            // value is serialized to the memoryStream and then copy to the input stream.
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(jsonValue);
                writer.Flush();              // In this case it needs to recive data before close StreamWriter stream
                memoryStream.WriteTo(stream);
            }
        }
    }
}
