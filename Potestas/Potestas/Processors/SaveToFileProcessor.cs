using Potestas.Exceptions.ProcessorExceptions;
using System;
using System.IO;

namespace Potestas.Processors
{
    /* TASK. Implement Processor which saves IEnergyObservation to the provided file.
     * 1. Try to decorate SerializeProcessor.
     * QUESTIONS:
     * Which bonuses does decoration have?
     * TEST: Which kind of tests should be written for this class?
     */
    public class SaveToFileProcessor<T> : IStreamProcessor<T> where T: IEnergyObservation
    {
        private string _filePath;
        private IStreamProcessor<T> _streamProcessor;

        public string Description => "Saves observations to the provided file.";

        /// <summary>
        /// Create instance of SaveToStorageProcessor with the given streamProcessor and filePath.
        /// </summary>
        /// <param name="streamProcessor"></param>
        /// <param name="filePath">Path of the file for the saving observable data.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when file does not exists for <paramref name="filePath"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="streamProcessor"/> is null.
        /// </exception>
        public SaveToFileProcessor(IStreamProcessor<T> streamProcessor, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File does not exist for the path {filePath} or you does not have permission to read this file.");
            }

            _streamProcessor = streamProcessor ?? throw new ArgumentNullException($"The {nameof(streamProcessor)} can not be null.");

            _filePath = filePath;
        }

        public void OnCompleted()
        {
            // or add loger 
            using (var stream = new FileStream(_filePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("Observable completed at: " + DateTime.UtcNow);
            }
        }

        public void OnError(Exception error)
        {
            throw new SaveToFileProcessorException($"Error in {Description}", error);
        }

        public void OnNext(T value)
        {
            using (var stream = new FileStream(_filePath, FileMode.Append)) // Stream.Synchronized ??
            using (var writer = new StreamWriter(stream)) // Encoding.Default
            {
                OnNext(stream, value);
                writer.Write(Environment.NewLine);
            }
        }

        public void OnNext(Stream stream, T value)
        {
            stream = stream ?? throw new ArgumentNullException($"The {nameof(stream)} can not be null.");
            _streamProcessor.OnNext(stream, value);
        }
    }
}
