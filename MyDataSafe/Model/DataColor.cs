
using System.Windows.Media;

namespace MyDataSafe.Model
{
    public class DataColor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Value { get; set; }
        public Brush Val
        {
            get =>
                (SolidColorBrush)new BrushConverter().ConvertFrom(this.Value);
        }

        public DataColor() { }
        public DataColor(string name, string value) : this()
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}


