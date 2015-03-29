namespace RabbitCache.Test
{
    public class TestCacheObject
    {
        public virtual string Id { get; set; }

        public bool Equals(TestCacheObject _obj)
        {
            if (_obj == null)
                return false;

            return _obj.Id == this.Id;
        }
        public override bool Equals(object _obj)
        {
            if (_obj == null)
                return false;

            var _testCacheObject = _obj as TestCacheObject;
            if (_testCacheObject == null)
                return false;

            return _testCacheObject.Id == this.Id;
        }

        public static bool operator ==(TestCacheObject _obj1, TestCacheObject _obj2)
        {
            if (object.ReferenceEquals(_obj1, _obj2))
                return true;

            if (((object)_obj1 == null) || ((object)_obj2 == null))
                return false;

            return _obj1.Id == _obj2.Id;
        }
        public static bool operator !=(TestCacheObject _obj1, TestCacheObject _obj2)
        {
            return _obj1 != _obj2;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}