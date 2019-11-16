using System.IO;
using CADPadServices.Interfaces;

namespace CADPadServices.Serilization
{
    public class NativeDrawingDeserializer : DrawingDeserializer
    {
        private Stream _stream;
        public NativeDrawingDeserializer(Stream stream)
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
