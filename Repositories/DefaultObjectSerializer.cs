using System.Text.Json;

namespace LogJson.AutoFarmer.Repositories
{
    public interface IObjectSerializer
    {
        byte[] Serialize<T>(T obj);

        T Deserialize<T>(byte[] bytes);
    }

    public interface IObjectSerializer<T>
    {
        byte[] Serialize(T obj);

        T Deserialize(byte[] bytes);
    }
    public class DefaultObjectSerializer : IObjectSerializer
    {
        public virtual byte[] Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            return AutoSerialize(obj);
        }

        public virtual T Deserialize<T>(byte[] bytes)
        {
            if (bytes == null)
            {
                return default(T);
            }

            return AutoDeserialize<T>(bytes);
        }

        protected virtual byte[] AutoSerialize<T>(T obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        protected virtual T AutoDeserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes) ?? default(T);
        }
    }
}
