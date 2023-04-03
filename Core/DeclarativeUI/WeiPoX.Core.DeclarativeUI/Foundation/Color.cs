using System.Globalization;

namespace WeiPoX.Core.DeclarativeUI.Foundation;

public record Color(byte R, byte G, byte B, byte A = 255)
{
    public Color(string hex) : this(
        byte.Parse(hex[1..3], NumberStyles.HexNumber),
        byte.Parse(hex[3..5], NumberStyles.HexNumber),
        byte.Parse(hex[5..7], NumberStyles.HexNumber),
        hex.Length == 9 ? byte.Parse(hex[7..9], NumberStyles.HexNumber) : (byte) 255
    )
    {
        
    }

    public static Color Transparent => new(0, 0, 0, 0);
    
}