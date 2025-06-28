namespace Yordanew.Domain.ValueObjects;

public record Tag(string Name) {
    public Color Color {
        get {
            var hash = Name.GetHashCode();
            var colorsCount = Enum.GetValues<Color>().Length;
            return (Color)(Math.Abs(hash) % colorsCount);
        }
    }
};