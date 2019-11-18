using System.IO;
using CADPadServices.Interfaces;

namespace CADPadServices.Serilization
{
    public class NativeDrawingContentDeserializer : DrawingContentDeserializer
    {
        private Stream _stream;
        public NativeDrawingContentDeserializer(Stream stream)
        {
            _stream = stream;
        }

        public override void Read(IDrawing drawing)
        {
            try
            {
                drawing.Document.Database.Read(_stream);
            }
            finally
            {
                if (_stream != null)
                    _stream.Close();
            }
        }
    }
}
