# MongoDB FrameworkSerializer
[![Build status](https://ci.appveyor.com/api/projects/status/s9sk5u3aa0plc3o1/branch/master?svg=true)](https://ci.appveyor.com/project/glucaci/mongodb-frameworkserializer/branch/master)
[![Tests](https://img.shields.io/appveyor/tests/glucaci/mongodb-frameworkserializer/master.svg)](https://ci.appveyor.com/project/glucaci/mongodb-frameworkserializer)

MongoDB support for using ISerializable interface from .NET

# Getting Started

## Defining a Serializable Object
The object must define ```[SerializableAlias]``` attribute which will describe a `TypeAlias` and this will be mapped to the concret type. Also the object must implement ```ISerializable```.
```csharp
[SerializableAlias("FullName")]
internal class FullName : ISerializable
{
    private readonly string _firstName;
    private readonly string _lastName;

    public FullName(string firstName, string lastName)
    {
        _firstName = firstName;
        _lastName = lastName;
    }

    public static explicit operator string(FullName fullName)
    {
        return $"{fullName._firstName}, {fullName._lastName}";
    }

    protected FullName(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info));
        }
        _firstName = info.GetString(nameof(_firstName));
        _lastName = info.GetString(nameof(_lastName));
    }

    void ISerializable.GetObjectData(SerializationInfo info,StreamingContext context)
    {
        info.AddValue(nameof(_firstName), _firstName);
        info.AddValue(nameof(_lastName), _lastName);
    }
}
```

## Map TypeAlias
The TypeAlias must be mapped on application startup.
```csharp
FrameworkSerializerRegistry.Map("FullName", typeof(FullName));
```
## Register FrameworkSerializer
The FrameworkSerializer must be registerd on application startup.
```csharp
BsonSerializer.RegisterSerializationProvider(FrameworkSerializerProvider.Instance);
```
## Use MongoDB Collection
```csharp
MongoClient client = new MongoClient("mongodb://localhost:27017");
IMongoDatabase db = client.GetDatabase("Users");
_collection = db.GetCollection<FullName>("Names");   
```
### Insert
```csharp
FullName fullName = new FullName("Foo", "Bar");
await _collection.InsertOneAsync(person);
```
```json
{
    "_id" : ObjectId("5b6085323a59c131abcfd10b"),
    "__typeAlias" : "FullName",
    "_firstName" : "Foo",
    "_lastName" : "Bar"
}
```
### Read
```csharp
FullName fullName = (await _collection.FindAsync(Builders<FullName>.Filter.Empty))
    .FirstOrDefault();
```