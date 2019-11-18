using System.IO;
using CADPadServices.Interfaces;

namespace CADPadServices.Serilization
{
    public class NativeDrawingContentSerializer : DrawingContentSerializer
    {
        private Stream _stream;
        public NativeDrawingContentSerializer(Stream stream)
        {
            _stream = stream;
        }

        public override void Write(IDrawing drawing)
        {
            try
            {
                drawing.Document.Database.Write(_stream);
            }
            finally
            {
                if (_stream != null)
                    _stream.Close();
            }
        }
    }
}
