using EasyNetQ;
using NUnit.Framework;
using RabbitCache.Caches.Serialization;

namespace RabbitCache.Test.Caches.Serialization
{
    [TestFixture]
    public class CacheEntryJsonSerializerTest
    {
        [Test]
        public void ImplementsISerializerInterfaceTest()
        {
            Assert.IsTrue(typeof(ISerializer).IsAssignableFrom(typeof(CacheEntryJsonSerializer)));
        }

        [Test]
        public void BytesToMessageTest()
        {
            const string OBJECT = "Test";

            var _valueTypeJsonSerializer = new CacheEntryJsonSerializer();
            var _bytes = _valueTypeJsonSerializer.MessageToBytes(OBJECT);
            var _message = _valueTypeJsonSerializer.BytesToMessage<string>(_bytes);

            Assert.AreEqual(OBJECT, _message);
        }
        [Test]
        public void MessageToBytesTest()
        {
            const string OBJECT = "Test";
            var _valueTypeJsonSerializer = new CacheEntryJsonSerializer();
            var _message = _valueTypeJsonSerializer.MessageToBytes(OBJECT);

            Assert.AreEqual(OBJECT, _valueTypeJsonSerializer.BytesToMessage<string>(_message));
        }

        // TODO: CacheEntryJsonSerializerTest: Implement Complex Tests (cover all Serializable cases)
        //[Test]
        //public void MessageToBytesForComplexObjecTest()
        //{
        //    var _complexObject = this.GetClientMock();
        //    using (var _transaction = new Transaction())
        //    {
        //        _transaction.Begin();
        //        _complexObject.Add();
        //        _transaction.Commit();
        //    }

        //    _complexObject = _complexObject.Refresh();

        //    var _valueTypeJsonSerializer = new CacheEntryJsonSerializer();
        //    var _message = _valueTypeJsonSerializer.MessageToBytes(_complexObject);
        //    var _bytesToMessage = _valueTypeJsonSerializer.BytesToMessage<Client>(_message);
        //    var _key = _bytesToMessage;

        //    Assert.AreEqual(_complexObject.Id, _key.Id);
        //    Assert.AreEqual(_complexObject.AffiliateCode, _key.AffiliateCode);
        //    Assert.AreEqual(_complexObject.CreatedDateTime, _key.CreatedDateTime);
        //    Assert.AreEqual(_complexObject.FirstName, _key.FirstName);
        //    Assert.AreEqual(_complexObject.LastName, _key.LastName);
        //    Assert.AreEqual(_complexObject.HasAcceptedTermsOfConditions, _key.HasAcceptedTermsOfConditions);
        //    Assert.AreEqual(_complexObject.HasRecievedWelcomeEmail, _key.HasRecievedWelcomeEmail);
        //    Assert.AreEqual(_complexObject.Name, _key.Name);
        //    Assert.AreEqual(_complexObject.UniqueIdentifier, _key.UniqueIdentifier);
        //    Assert.AreEqual(_complexObject.UpdatedDateTime, _key.UpdatedDateTime);
        //    Assert.AreEqual(_complexObject.Id, _key.Id);
        //}
        //[Test]
        //public void MessageToBytesForComplexObjectWhenTupleTest()
        //{
        //    var _coordinate = new Coordinate(1, 1);
        //    var _complexObject = this.GetClientMock();
        //    var _complexObject2 = this.GetVehicleMock();
        //    var _complexObject3 = this.GetPartnerMock();

        //    using (var _transaction = new Transaction())
        //    {
        //        _transaction.Begin();

        //        _complexObject.Add();
        //        _complexObject2.Add();
        //        _complexObject3.Add();

        //        var _spatialCacheItem = new SpatialCacheItem<Coordinate, Tuple<IClient, IPartner>, IVehicle>(_coordinate, new Tuple<IClient, IPartner>(_complexObject, _complexObject3), _complexObject2);
        //        var _cacheEntry = new CacheEntry
        //        {
        //            CacheType = typeof(SpatialCacheItem<Coordinate, Tuple<IClient, IPartner>, IVehicle>),
        //            ExpireAt = DateTimeOffset.UtcNow.AddMinutes(2),
        //            Key = _coordinate,
        //            Value = _spatialCacheItem,
        //            RegionName = "Test"
        //        };

        //        var _valueTypeJsonSerializer = new CacheEntryJsonSerializer();
        //        var _message = _valueTypeJsonSerializer.MessageToBytes(_cacheEntry);
        //        var _bytesToMessage = _valueTypeJsonSerializer.BytesToMessage<CacheEntry>(_message);

        //        var _key = (Coordinate)_bytesToMessage.Key;
        //        var _value = (SpatialCacheItem<Coordinate, Tuple<IClient, IPartner>, IVehicle>)_bytesToMessage.Value;

        //        Assert.AreEqual(_coordinate.X, _key.X);
        //        Assert.AreEqual(_coordinate.Y, _key.Y);

        //        Assert.IsNotNull(_value);
        //        Assert.AreEqual(_complexObject.Id, _value.SpatialValue.Item1.Id);
        //        Assert.AreEqual(_complexObject3.Id, _value.SpatialValue.Item2.Id);
        //        Assert.AreEqual(_complexObject2.Id, _value.ObjectKeyValue.Id);

        //        _transaction.Commit();
        //    }
        //}
        //[Test]
        //public void MessageToBytesForComplexObjectWhenCacheEntryObjectTest()
        //{
        //    var _complexObject = this.GetClientMock();
        //    var _complexObject2 = this.GetVehicleToGeoCoordinateMock();
        //    _complexObject2.Direction = VehicleGeoCoordinateDirection.INACCURATE;
        //    using (var _transaction = new Transaction())
        //    {
        //        _transaction.Begin();
                
        //        _complexObject.Add();
        //        _complexObject2.Add();

        //        _transaction.Commit();
        //    }

        //    _complexObject = _complexObject.Refresh();
        //    _complexObject2 = _complexObject2.Refresh();

        //    var _cacheEntry = new CacheEntry
        //    {
        //        CacheType = typeof(SpatialCacheItem<Coordinate, IClient, IVehicleToGeoCoordinate>),
        //        ExpireAt = DateTimeOffset.UtcNow.AddMinutes(2),
        //        Key = _complexObject,
        //        Value = _complexObject2,
        //        RegionName = "Test"
        //    };

        //    var _valueTypeJsonSerializer = new CacheEntryJsonSerializer();
        //    var _message = _valueTypeJsonSerializer.MessageToBytes(_cacheEntry);
        //    var _bytesToMessage = _valueTypeJsonSerializer.BytesToMessage<CacheEntry>(_message);

        //    var _key = (IClient)_bytesToMessage.Key;
        //    var _value = (IVehicleToGeoCoordinate)_bytesToMessage.Value;

        //    Assert.AreEqual(_complexObject.Id, _key.Id);
        //    Assert.AreEqual(_complexObject.AffiliateCode, _key.AffiliateCode);
        //    Assert.AreEqual(_complexObject.CreatedDateTime, _key.CreatedDateTime);
        //    Assert.AreEqual(_complexObject.FirstName, _key.FirstName);
        //    Assert.AreEqual(_complexObject.LastName, _key.LastName);
        //    Assert.AreEqual(_complexObject.HasAcceptedTermsOfConditions, _key.HasAcceptedTermsOfConditions);
        //    Assert.AreEqual(_complexObject.HasRecievedWelcomeEmail, _key.HasRecievedWelcomeEmail);
        //    Assert.AreEqual(_complexObject.Name, _key.Name);
        //    Assert.AreEqual(_complexObject.UniqueIdentifier, _key.UniqueIdentifier);
        //    Assert.AreEqual(_complexObject.UpdatedDateTime, _key.UpdatedDateTime);
        //    Assert.AreEqual(_complexObject.Id, _key.Id);

        //    Assert.AreEqual(_complexObject2.Id, _value.Id);
        //    Assert.AreEqual(_complexObject2.Direction, _value.Direction);
        //    Assert.AreEqual(_complexObject2.ArrivalTimeInSeconds, _value.ArrivalTimeInSeconds);
        //    Assert.AreEqual(_complexObject2.CreatedDateTime, _value.CreatedDateTime);
        //    Assert.AreEqual(_complexObject2.DistanceInMeters, _value.DistanceInMeters);
        //    Assert.AreEqual(_complexObject2.IsWithTraffic, _value.IsWithTraffic);
        //}
        //[Test]
        //public void MessageToBytesForComplexObjectWhenSpatialCacheItemObjectTest()
        //{
        //    var _coordinate = new Coordinate(1, 1);
        //    var _complexObject = this.GetClientMock();
        //    var _complexObject2 = this.GetVehicleMock();

        //    using (var _transaction = new Transaction())
        //    {
        //        _transaction.Begin();

        //        _complexObject.Add();
        //        _complexObject2.Add();

        //        var _spatialCacheItem = new SpatialCacheItem<Coordinate, IClient, IVehicle>(_coordinate, _complexObject, _complexObject2);
        //        var _cacheEntry = new CacheEntry
        //        {
        //            CacheType = typeof(SpatialCacheItem<Coordinate, IClient, IVehicle>),
        //            ExpireAt = DateTimeOffset.UtcNow.AddMinutes(2),
        //            Key = _coordinate,
        //            Value = _spatialCacheItem,
        //            RegionName = "Test"
        //        };

        //        var _valueTypeJsonSerializer = new CacheEntryJsonSerializer();
        //        var _message = _valueTypeJsonSerializer.MessageToBytes(_cacheEntry);
        //        var _bytesToMessage = _valueTypeJsonSerializer.BytesToMessage<CacheEntry>(_message);

        //        var _key = (Coordinate)_bytesToMessage.Key;
        //        var _value = (SpatialCacheItem<Coordinate, IClient, IVehicle>)_bytesToMessage.Value;

        //        Assert.AreEqual(_coordinate.X, _key.X);
        //        Assert.AreEqual(_coordinate.Y, _key.Y);

        //        Assert.IsNotNull(_value);
        //        Assert.AreEqual(_complexObject.Id, _value.SpatialValue.Id);
        //        Assert.AreEqual(_complexObject2.Id, _value.ObjectKeyValue.Id);

        //        _transaction.Commit();
        //    }
        //}
        //[Test]
        //public void MessageToBytesForComplexObjectWhenSpatialCacheItemObjectAndPropertyStateMachineTest()
        //{
        //    var _coordinate = new Coordinate(1, 1);
        //    var _complexObject = this.GetDriverMock();
        //    var _complexObject2 = this.GetVehicleMock();

        //    using (var _transaction = new Transaction())
        //    {
        //        _transaction.Begin();

        //        _complexObject.Add();
        //        _complexObject2.Add();

        //        var _spatialCacheItem = new SpatialCacheItem<Coordinate, IDriver, IVehicle>(_coordinate, _complexObject, _complexObject2);
        //        var _cacheEntry = new CacheEntry
        //        {
        //            CacheType = typeof(SpatialCacheItem<Coordinate, IDriver, IVehicle>),
        //            ExpireAt = DateTimeOffset.UtcNow.AddMinutes(2),
        //            Key = _coordinate,
        //            Value = _spatialCacheItem,
        //            RegionName = "Test"
        //        };

        //        var _valueTypeJsonSerializer = new CacheEntryJsonSerializer();
        //        var _message = _valueTypeJsonSerializer.MessageToBytes(_cacheEntry);
        //        var _bytesToMessage = _valueTypeJsonSerializer.BytesToMessage<CacheEntry>(_message);

        //        var _key = (Coordinate)_bytesToMessage.Key;
        //        var _value = (SpatialCacheItem<Coordinate, IDriver, IVehicle>)_bytesToMessage.Value;

        //        Assert.AreEqual(_coordinate.X, _key.X);
        //        Assert.AreEqual(_coordinate.Y, _key.Y);

        //        Assert.IsNotNull(_value);
        //        Assert.AreEqual(_complexObject.Id, _value.SpatialValue.Id);
        //        Assert.AreEqual(_complexObject2.Id, _value.ObjectKeyValue.Id);

        //        _transaction.Commit();
        //    }
        //}
    }
}