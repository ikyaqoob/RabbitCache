namespace RabbitCache.Caches.Entities
{
    public class Coordinate
    {
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public virtual double Z { get; set; }

        public virtual double Latitude
        {
            get { return this.Y; }
            set { this.Y = value; }
        }
        public virtual double Longitude
        {
            get { return this.X; }
            set { this.X = value; }
        }

        public Coordinate()
        {
            this.X = 0.00;
            this.Y = 0.00;
            this.Z = 0.00;
        }

        public NetTopologySuite.Geometries.Point ToPoint()
        {
            return new NetTopologySuite.Geometries.Point(this.X, this.Y, this.Z);
        }
        public GeoAPI.Geometries.Coordinate ToCoordinate()
        {
            return new GeoAPI.Geometries.Coordinate(this.X, this.Y, this.Z);
        }
    }
}